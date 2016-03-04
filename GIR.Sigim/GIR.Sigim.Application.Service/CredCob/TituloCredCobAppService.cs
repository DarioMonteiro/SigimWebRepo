using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using System.Threading.Tasks;
using GIR.Sigim.Application.Service.CredCob;
using GIR.Sigim.Application.Service.Admin;
using GIR.Sigim.Domain.Entity.CredCob;
using GIR.Sigim.Domain.Repository.CredCob;
using GIR.Sigim.Domain.Specification;
using GIR.Sigim.Application.Filtros.Financeiro;
using GIR.Sigim.Domain.Specification.CredCob;
using GIR.Sigim.Domain.Entity.Financeiro;
using GIR.Sigim.Application.DTO.Financeiro;
using GIR.Sigim.Domain.Repository.Financeiro;
using GIR.Sigim.Application.Adapter;
using GIR.Sigim.Application.DTO.CredCob;
using GIR.Sigim.Domain.Entity.Comercial;
using GIR.Sigim.Domain.Repository.Sigim;
using GIR.Sigim.Domain.Entity.Sigim;
using GIR.Sigim.Application.Service.Sigim;

namespace GIR.Sigim.Application.Service.CredCob
{
    public class TituloCredCobAppService : BaseAppService, ITituloCredCobAppService
    {
        #region Declaração

        private IUsuarioAppService usuarioAppService;
        private ITituloCredCobRepository tituloCredCobRepository;
        private IClasseRepository classeRepository;
        private IClienteFornecedorRepository clienteFornecedorRepository;
        private IParametrosSigimRepository parametrosSigimRepository;
        private IModuloSigimAppService moduloSigimAppService;

        #endregion

        #region Construtor

        public TituloCredCobAppService(IUsuarioAppService usuarioAppService,
                                       ITituloCredCobRepository tituloCredCobRepository,
                                       IClasseRepository classeRepository, 
                                       IClienteFornecedorRepository clienteFornecedorRepository,
                                       IParametrosSigimRepository parametrosSigimRepository,
                                       IModuloSigimAppService moduloSigimAppService,
                                       MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.tituloCredCobRepository = tituloCredCobRepository;
            this.usuarioAppService = usuarioAppService;
            this.classeRepository = classeRepository;
            this.clienteFornecedorRepository = clienteFornecedorRepository;
            this.parametrosSigimRepository = parametrosSigimRepository;
            this.moduloSigimAppService = moduloSigimAppService;
        }

        #endregion

        #region Métodos ITituloCredCobAppService

        public Specification<TituloCredCob> MontarSpecificationMovimentoCredCobRelApropriacaoPorClasse(RelApropriacaoPorClasseFiltro filtro, int? idUsuario)
        {
            var specification = (Specification<TituloCredCob>)new TrueSpecification<TituloCredCob>();

            if (usuarioAppService.UsuarioPossuiCentroCustoDefinidoNoModulo(idUsuario, Resource.Sigim.NomeModulo.Financeiro))
            {
                specification &= TituloCredCobSpecification.UsuarioPossuiAcessoAoCentroCusto(idUsuario, Resource.Sigim.NomeModulo.Financeiro);
            }
            else
            {
                specification &= TituloCredCobSpecification.EhCentroCustoAtivo();
            }

            if (filtro.CentroCusto != null)
            {
                specification &= TituloCredCobSpecification.PertenceAoCentroCustoIniciadoPor(filtro.CentroCusto.Codigo);
            }

            specification &= TituloCredCobSpecification.EhTipoParticipanteTitular();
            specification &= TituloCredCobSpecification.EhSituacaoDiferenteDeCancelado();

            specification &= TituloCredCobSpecification.DataPeriodoMaiorOuIgualRelApropriacaoPorClasse(filtro.DataInicial);
            specification &= TituloCredCobSpecification.DataPeriodoMenorOuIgualRelApropriacaoPorClasse(filtro.DataFinal);

            if (filtro.OpcoesRelatorio.HasValue)
            {
                //if (filtro.OpcoesRelatorio.Value != (int)OpcoesRelatorioApropriacaoPorClasse.Sintetico)

                if (filtro.OpcoesRelatorio.Value == (int)OpcoesRelatorioApropriacaoPorClasse.Analitico)
                {
                    if (filtro.ListaClasseReceita.Count > 0)
                    {
                        string[] arrayCodigoClasse = PopulaArrayComCodigosDeClassesSelecionadas(filtro.ListaClasseDespesa);

                        if (arrayCodigoClasse.Length > 0)
                        {
                            specification &= TituloCredCobSpecification.SaoClassesExistentes(arrayCodigoClasse);
                        }
                    }
                }
            }

            if (!(filtro.EhSituacaoAReceberFaturado && filtro.EhSituacaoAReceberRecebido))
            {
                if (filtro.EhSituacaoAReceberFaturado)
                {
                    specification &= TituloCredCobSpecification.EhSituacaoPendente();

                }

                if (filtro.EhSituacaoAReceberRecebido)
                {
                    specification &= TituloCredCobSpecification.EhSituacaoQuitado();
                }
            }


            return specification;
        }

        public List<TituloDetalheCredCobDTO> RecTit(List<TituloCredCob> listaTituloCredCob,
                                                    Nullable<DateTime> dataReferencia,
                                                    bool excluiTabelaTemporaria = false,
                                                    bool corrigeParcelaResiduo = false)
        {
            List<TituloDetalheCredCobDTO> listaTituloDetalheCredCob = new List<TituloDetalheCredCobDTO>();

            int clienteTitularIdAnterior = 0;
            int clienteTitularId = 0;
            ClienteFornecedor clienteTitular = new ClienteFornecedor();

            ParametrosSigim parametrosSigim = parametrosSigimRepository.Obter();

            foreach (TituloCredCob titulo in listaTituloCredCob)
            {
                TituloDetalheCredCobDTO tituloDetalhe = new TituloDetalheCredCobDTO();

                tituloDetalhe = titulo.To<TituloDetalheCredCobDTO>();

                if (dataReferencia.HasValue)
                {
                    dataReferencia = dataReferencia.Value.Date;
                }

                if ((titulo.Situacao == "P") || (titulo.Situacao == "Q"))
                {
                    if (!dataReferencia.HasValue)
                    {
                        dataReferencia = titulo.DataVencimento;
                    }

                    #region "Trata dia util"
                    //Trata data referencia
                    if (dataReferencia.HasValue)
                    {
                        if (titulo.DataVencimento < dataReferencia)
                        {
                            clienteTitularId = titulo.Contrato.ListaVendaParticipante.Where(l => l.TipoParticipanteId == 1).FirstOrDefault().ClienteId;

                            if (clienteTitularId != clienteTitularIdAnterior)
                            {
                                clienteTitular = clienteFornecedorRepository.ObterPeloId(clienteTitularId,
                                                                                         l => l.EnderecoResidencial.UnidadeFederacao.ListaFeriado,
                                                                                         l => l.EnderecoComercial.UnidadeFederacao.ListaFeriado,
                                                                                         l => l.EnderecoOutro.UnidadeFederacao.ListaFeriado);
                                clienteTitularIdAnterior = clienteTitularId;
                            }

                            bool achouProximoDiaUtil = false;
                            DateTime dataUtil = titulo.DataVencimento.AddDays(-1);

                            while (!achouProximoDiaUtil)
                            {
                                dataUtil = dataUtil.AddDays(1);
                                GIR.Sigim.Domain.Entity.Sigim.Feriado feriado = null;
                                if (clienteTitular.Correspondencia == "R")
                                {
                                    feriado = clienteTitular.EnderecoResidencial.UnidadeFederacao.ListaFeriado.Where(l => l.Data.Value.Date == dataUtil.Date).FirstOrDefault();
                                }
                                if (clienteTitular.Correspondencia == "C")
                                {
                                    feriado = clienteTitular.EnderecoComercial.UnidadeFederacao.ListaFeriado.Where(l => l.Data.Value.Date == dataUtil.Date).FirstOrDefault();
                                }
                                if (clienteTitular.Correspondencia == "O")
                                {
                                    feriado = clienteTitular.EnderecoOutro.UnidadeFederacao.ListaFeriado.Where(l => l.Data.Value.Date == dataUtil.Date).FirstOrDefault();
                                }
                                if (feriado == null)
                                {
                                    if ((dataUtil.DayOfWeek != DayOfWeek.Saturday) && (dataUtil.DayOfWeek != DayOfWeek.Sunday))
                                    {
                                        achouProximoDiaUtil = true;
                                    }
                                }
                            }
                            if (dataUtil >= dataReferencia.Value)
                            {
                                dataReferencia = titulo.DataVencimento;
                            }
                        }
                    }
                    //Trata data referencia
                    #endregion
                }

                decimal valorIndiceOriginal = titulo.ValorIndiceOriginal.HasValue ? titulo.ValorIndiceOriginal.Value : 0;
                decimal qtdIndiceOriginal = titulo.QtdIndiceOriginal.HasValue ? titulo.QtdIndiceOriginal.Value : 0;

                DateTime dataVencimentoDefasada;
                DateTime dataReferenciaDefasada;
                DateTime ultimaData;
                decimal valorIndiceDataVencimentoDefasada = 0;
                decimal valorIndiceDataReferenciaDefasada = 0;

                decimal valorIndiceDataVencimento = 0;
                decimal valorIndiceDataReferencia = 0;

                decimal valorAmortizacaoOriginal = valorIndiceOriginal * qtdIndiceOriginal;

                if (titulo.Situacao == "P") 
                {
                    CotacaoValores cotacaoValores = null;

                    valorIndiceDataReferencia = 0;
                    #region "Calcula cotacao data referencia defasada"
                    ultimaData = titulo.Indice.ListaCotacaoValores.Where(l => l.Data <= dataReferencia.Value.Date).Select(l => l.Data.Value).Max();
                    cotacaoValores = titulo.Indice.ListaCotacaoValores.Where(l => l.Data == ultimaData).FirstOrDefault();
                    #endregion

                    valorIndiceDataReferencia = cotacaoValores.Valor.HasValue ? cotacaoValores.Valor.Value : 0;

                    #region "Calcula cotacao data referencia defasada"
                    dataReferenciaDefasada = dataReferencia.Value.Date.AddMonths(titulo.VendaSerie.DefasagemMesIndiceCorrecao * -1);
                    ultimaData =  titulo.Indice.ListaCotacaoValores.Where(l => l.Data <= dataReferenciaDefasada).Select(l => l.Data.Value).Max();
                    cotacaoValores = titulo.Indice.ListaCotacaoValores.Where(l => l.Data == ultimaData).FirstOrDefault();
                    #endregion

                    valorIndiceDataReferenciaDefasada = cotacaoValores.Valor.HasValue ? cotacaoValores.Valor.Value : 0;

                    #region "Calcula cotacao data vencimento"
                    ultimaData = titulo.Indice.ListaCotacaoValores.Where(l => l.Data <= titulo.DataVencimento.Date).Select(l => l.Data.Value).Max();
                    cotacaoValores = titulo.Indice.ListaCotacaoValores.Where(l => l.Data == ultimaData).FirstOrDefault();
                    #endregion

                    valorIndiceDataVencimento = cotacaoValores.Valor.HasValue ? cotacaoValores.Valor.Value : 0;

                    #region "Calcula cotacao data vencimento defasada"
                    dataVencimentoDefasada = titulo.DataVencimento.Date.AddMonths(titulo.VendaSerie.DefasagemMesIndiceCorrecao * -1);
                    ultimaData = titulo.Indice.ListaCotacaoValores.Where(l => l.Data <= dataVencimentoDefasada).Select(l => l.Data.Value).Max();
                    cotacaoValores = titulo.Indice.ListaCotacaoValores.Where(l => l.Data == ultimaData).FirstOrDefault();
                    #endregion

                    valorIndiceDataVencimentoDefasada = cotacaoValores.Valor.HasValue ? cotacaoValores.Valor.Value : 0;

                    if ((titulo.DataVencimento.Day > dataReferencia.Value.Day) &&
                        (titulo.DataVencimento < dataReferencia))
                    {
                        #region "Calcula cotacao data referencia"
                        dataReferenciaDefasada = dataReferencia.Value.Date.AddMonths(-1);
                        ultimaData = titulo.Indice.ListaCotacaoValores.Where(l => l.Data <= dataReferenciaDefasada).Select(l => l.Data.Value).Max();
                        cotacaoValores = titulo.Indice.ListaCotacaoValores.Where(l => l.Data == ultimaData).FirstOrDefault();
                        #endregion

                        valorIndiceDataReferenciaDefasada = cotacaoValores.Valor.HasValue ? cotacaoValores.Valor.Value : 0;
                    }

                    if (parametrosSigim.CorrecaoMesCheioDiaPrimeiro.HasValue && parametrosSigim.CorrecaoMesCheioDiaPrimeiro.Value)
                    {
                        #region "Calcula cotacao data referencia"
                        dataReferenciaDefasada = dataReferencia.Value.Date.AddMonths(titulo.VendaSerie.DefasagemMesIndiceCorrecao * -1);
                        ultimaData = titulo.Indice.ListaCotacaoValores.Where(l => l.Data <= dataReferenciaDefasada).Select(l => l.Data.Value).Max();
                        cotacaoValores = titulo.Indice.ListaCotacaoValores.Where(l => l.Data == ultimaData).FirstOrDefault();
                        #endregion

                        valorIndiceDataReferenciaDefasada = cotacaoValores.Valor.HasValue ? cotacaoValores.Valor.Value : 0;
                    }

                    decimal valorTituloDataReferencia = 0;
                    decimal valorAmortizacaoDataReferencia = 0;
                    decimal valorJurosDataReferencia = 0;
                    decimal valorIndiceBaseTemp = 0;
                    decimal qtdIndiceAmortizacao = titulo.QtdIndiceAmortizacao.HasValue ? titulo.QtdIndiceAmortizacao.Value: 0;
                    decimal qtdIndiceJuros = titulo.QtdIndiceJuros.HasValue ? titulo.QtdIndiceJuros.Value : 0;

                    #region "Calcula valor atualizado"
                    if (dataReferencia.Value >= titulo.DataVencimento)
                    {
                        valorTituloDataReferencia = titulo.QtdIndice * valorIndiceDataVencimentoDefasada;
                        valorAmortizacaoDataReferencia = qtdIndiceAmortizacao * valorIndiceDataVencimentoDefasada;
                        valorJurosDataReferencia = qtdIndiceJuros * valorIndiceDataVencimentoDefasada;
                        valorIndiceBaseTemp = valorIndiceDataVencimentoDefasada;
                    }
                    else
                    {
                        valorTituloDataReferencia = titulo.QtdIndice * valorIndiceDataReferenciaDefasada;
                        valorAmortizacaoDataReferencia = qtdIndiceAmortizacao * valorIndiceDataReferenciaDefasada;
                        valorJurosDataReferencia = qtdIndiceJuros * valorIndiceDataReferenciaDefasada;
                        valorIndiceBaseTemp = valorIndiceDataReferenciaDefasada;
                    }
                    #endregion

                    #region "Trata residuo anual"
                    if ((!corrigeParcelaResiduo) & (titulo.VendaSerie.CobrancaResiduo == "S"))
                    {
                        valorTituloDataReferencia = titulo.QtdIndice * titulo.ValorIndiceBase;
                    }

                    if ((corrigeParcelaResiduo) & (titulo.VendaSerie.CobrancaResiduo == "S") & (dataReferencia.Value > titulo.DataVencimento))
                    {
                        valorTituloDataReferencia = titulo.QtdIndice * titulo.ValorIndiceBase;
                    }
                    #endregion

                    decimal valorTituloDataBase = valorTituloDataReferencia;
                    decimal valorAmortizacaoDataBase = valorAmortizacaoDataReferencia;
                    decimal valorJurosDataBase = valorJurosDataReferencia;

                    if ((!corrigeParcelaResiduo) & (titulo.VendaSerie.CobrancaResiduo == "S"))
                    {
                        valorTituloDataBase = titulo.QtdIndice * titulo.ValorIndiceBase;
                        valorAmortizacaoDataBase = qtdIndiceAmortizacao * titulo.ValorIndiceBase;
                        valorJurosDataBase = qtdIndiceJuros * titulo.ValorIndiceBase;
                    }

                    if ((corrigeParcelaResiduo) & (titulo.VendaSerie.CobrancaResiduo == "S") & (dataReferencia.Value > titulo.DataVencimento))
                    {
                        valorTituloDataBase = titulo.QtdIndice * titulo.ValorIndiceBase;
                        valorAmortizacaoDataBase = qtdIndiceAmortizacao * titulo.ValorIndiceBase;
                        valorJurosDataBase = qtdIndiceJuros * titulo.ValorIndiceBase;
                    }

                    #region "Inicializa valor atrasado"
                    decimal valorTituloAtrasado = valorTituloDataBase;
                    decimal percentualJuros = 0;
                    decimal valorPercentualJuros = titulo.ValorPercentualJuros.HasValue ? titulo.ValorPercentualJuros.Value : 0;
                    double periodicidade = Convert.ToDouble(titulo.VendaSerie.Periodicidade);
                    #endregion

                    #region "Calcula percentual de juros conforme periodicidade da serie"
                    if (titulo.VendaSerie.FormaFinanciamento == "1")
                    {
                        double valor = 1 + (((double)valorPercentualJuros / 12) / 100);
                        percentualJuros = Convert.ToDecimal(Math.Pow(valor, periodicidade));
                        percentualJuros = percentualJuros - 1;
                    }
                    else
                    {
                        percentualJuros = ((valorPercentualJuros / 12) / 100);
                    }
                    #endregion

                    //Calcula valores de titulos com data de vencimento MAIOR que data de referencia 
                    //conforme forma de financiamento

                    #region "Calcula quantidade de meses e dias de defasagem"

                    int qtdMesesDefasagemCorrecaoAtraso = 0;
                    int qtdDiasProrrata = 0;
                    int qtdDiasProrrataDescapitalizacao = 0;
                    if (titulo.DataVencimento < dataReferencia.Value)
                    {
                        qtdMesesDefasagemCorrecaoAtraso =  moduloSigimAppService.ObtemQuantidadeDeMeses(titulo.DataVencimento, dataReferencia.Value);
                        //Calcula Dias Prorrata quando a Data de Vencimento for menor que a Data de Referência
                        qtdDiasProrrata = moduloSigimAppService.ObtemQuantidadeDeDias(moduloSigimAppService.AcertaData(dataReferencia.Value.Year, dataReferencia.Value.Month,titulo.DataVencimento.Day), dataReferencia.Value);
                        qtdDiasProrrataDescapitalizacao = qtdDiasProrrata;
                    }

                    //Calcula Dias Prorrata quando a Data de Vencimento for menor que a Data de Referência 
                    //e o dia da Data de Vencimento for maior que o dia da Data de Referência
                    if ((titulo.DataVencimento < dataReferencia.Value) & (titulo.DataVencimento.Day > dataReferencia.Value.Day))
                    {
                        DateTime dataAux = moduloSigimAppService.AcertaData(dataReferencia.Value.Year, dataReferencia.Value.Month,titulo.DataVencimento.Day);
                        qtdDiasProrrata = moduloSigimAppService.ObtemQuantidadeDeDias(dataAux.AddMonths(-1), dataReferencia.Value);
                        qtdDiasProrrataDescapitalizacao = qtdDiasProrrata;
                    }

                    //Calcula Dias Prorrata quando a Data de Vencimento for maior que a Data de Referência
                    if (titulo.DataVencimento > dataReferencia.Value)
                    {
                        qtdDiasProrrata = moduloSigimAppService.ObtemQuantidadeDeDias(dataReferencia.Value,moduloSigimAppService.AcertaData(dataReferencia.Value.Year, dataReferencia.Value.Month, titulo.DataVencimento.Day));
                        qtdDiasProrrataDescapitalizacao = qtdDiasProrrata;
                    }

                    //Calcula Dias Prorrata quando a Data de Vencimento for maior que a Data de Referência 
                    //e o dia da Data de Vencimento for menor que o dia da Data de Referência
                    if ((titulo.DataVencimento > dataReferencia.Value) & (titulo.DataVencimento.Day < dataReferencia.Value.Day))
                    {
                        DateTime dataAux = moduloSigimAppService.AcertaData(dataReferencia.Value.Year, dataReferencia.Value.Month, titulo.DataVencimento.Day);
                        qtdDiasProrrata = moduloSigimAppService.ObtemQuantidadeDeDias(dataReferencia.Value, dataAux.AddMonths(1));
                        qtdDiasProrrataDescapitalizacao = qtdDiasProrrata;
                    }

                    //Cria a data de referência para descapitalização
                    DateTime dataReferenciaDescapitalizacao = dataReferencia.Value;
                    if (parametrosSigim.MetodoDescapitalizacao == "DV")
                    {
                        dataReferenciaDescapitalizacao = moduloSigimAppService.AcertaData(dataReferencia.Value.Year, dataReferencia.Value.Month, titulo.DataVencimento.Day);
                        qtdDiasProrrataDescapitalizacao = 0;
                    }

                    #endregion

                    decimal valorTituloPresenteCheio = 0;
                    DateTime dataBaseJuros = titulo.VendaSerie.DataBaseJuros.HasValue ? titulo.VendaSerie.DataBaseJuros.Value : new DateTime();
                    int defasagemMes = titulo.VendaSerie.DefasagemMes.HasValue ? titulo.VendaSerie.DefasagemMes.Value : 0;
                    int defasagemDia = titulo.VendaSerie.DefasagemDia.HasValue ? titulo.VendaSerie.DefasagemDia.Value : 0;

                    DateTime dataBaseJurosDefasadaAux = moduloSigimAppService.CalculaDataDefasagem(dataBaseJuros, (defasagemMes * -1), (defasagemDia * -1));
                    DateTime dataVencimentoDefasadaAux = moduloSigimAppService.CalculaDataDefasagem(titulo.DataVencimento, (defasagemMes * -1), (defasagemDia * -1));

                    #region "Tabela Price e SACJS"

                    if ((titulo.DataVencimento >= dataReferenciaDescapitalizacao) && 
                        (valorPercentualJuros > 0) &&
                        ((titulo.VendaSerie.FormaFinanciamento == "1") || 
                         (titulo.VendaSerie.FormaFinanciamento == "2") || 
                         (titulo.VendaSerie.FormaFinanciamento == "4"))) 
                    {
                        if (dataReferenciaDescapitalizacao < dataBaseJurosDefasadaAux)
                        {
                            valorTituloPresenteCheio = moduloSigimAppService.CalculaPV(valorTituloDataBase,
                                                                                       (valorPercentualJuros / 1200),
                                                                                       dataBaseJurosDefasadaAux,
                                                                                       titulo.DataVencimento,
                                                                                       0);
                        }
                        else
                        {
                            valorTituloPresenteCheio = moduloSigimAppService.CalculaPV(valorTituloDataBase,
                                                                                       (valorPercentualJuros / 1200),
                                                                                       dataReferenciaDescapitalizacao,
                                                                                       titulo.DataVencimento,
                                                                                       qtdDiasProrrataDescapitalizacao);
                        }
                    }

                    if ((titulo.DataVencimento >= dataReferenciaDescapitalizacao) &&
                        (titulo.DataVencimento.Day < dataReferenciaDescapitalizacao.Day) &&
                        (valorPercentualJuros > 0) &&
                        ((titulo.VendaSerie.FormaFinanciamento == "1") ||
                         (titulo.VendaSerie.FormaFinanciamento == "2") ||
                         (titulo.VendaSerie.FormaFinanciamento == "4")))
                    {
                        if (dataReferenciaDescapitalizacao < dataBaseJurosDefasadaAux)
                        {
                            valorTituloPresenteCheio = moduloSigimAppService.CalculaPV(valorTituloDataBase,
                                                                                       (valorPercentualJuros / 1200),
                                                                                       dataBaseJurosDefasadaAux,
                                                                                       titulo.DataVencimento,
                                                                                       0);
                        }
                        else
                        {
                            int qtdDiasProrrataAux = 0;

                            if (moduloSigimAppService.AcertaData(dataReferenciaDescapitalizacao.Date.Year,
                                                                 dataReferenciaDescapitalizacao.Date.Month,
                                                                 31).Date.Day == 31)
                            {
                                qtdDiasProrrataAux = qtdDiasProrrataDescapitalizacao - 1;
                            }
                            else
                            {
                                qtdDiasProrrataAux = qtdDiasProrrataDescapitalizacao;
                            }

                            valorTituloPresenteCheio = moduloSigimAppService.CalculaPV(valorTituloDataBase,
                                                                                       (valorPercentualJuros / 1200),
                                                                                       dataReferenciaDescapitalizacao,
                                                                                       titulo.DataVencimento.AddMonths(-1),
                                                                                       qtdDiasProrrataAux);
                        }
                    }

                    #endregion 

                    #region "SACHJS"

                    if ((titulo.DataVencimento >= dataReferenciaDescapitalizacao) &&
                        (valorPercentualJuros > 0) &&
                        (titulo.VendaSerie.FormaFinanciamento == "3"))
                    {
                        if (dataReferenciaDescapitalizacao < dataBaseJurosDefasadaAux)
                        {
                            valorTituloPresenteCheio = 
                                valorAmortizacaoDataReferencia +
                                (valorAmortizacaoDataReferencia * 
                                (percentualJuros * 
                                 moduloSigimAppService.ObtemQuantidadeDeMeses(dataBaseJurosDefasadaAux, dataReferenciaDescapitalizacao)));
                        }
                        else
                        {
                            valorTituloPresenteCheio = 
                                valorAmortizacaoDataReferencia +
                                (valorAmortizacaoDataReferencia *
                                (percentualJuros *
                                 moduloSigimAppService.ObtemQuantidadeDeMeses(dataBaseJurosDefasadaAux, dataReferenciaDescapitalizacao))) +
                                (valorAmortizacaoDataReferencia *
                                ((percentualJuros / 30) *
                                 moduloSigimAppService.ObtemQuantidadeDeDias(
                                    moduloSigimAppService.AcertaData(dataReferenciaDescapitalizacao.Year, 
                                                                     dataReferenciaDescapitalizacao.Month,
                                                                     dataVencimentoDefasadaAux.Day), 
                                    dataReferenciaDescapitalizacao)));
                        }
                    }

                    if ((titulo.DataVencimento >= dataReferenciaDescapitalizacao) &&
                        (titulo.DataVencimento.Day > dataReferenciaDescapitalizacao.Day) &&
                        (valorPercentualJuros > 0) &&
                        (titulo.VendaSerie.FormaFinanciamento == "3"))
                    {
                        if (dataReferenciaDescapitalizacao < dataBaseJurosDefasadaAux)
                        {
                            valorTituloPresenteCheio =
                                valorAmortizacaoDataReferencia +
                                (valorAmortizacaoDataReferencia *
                                (percentualJuros *
                                 moduloSigimAppService.ObtemQuantidadeDeMeses(dataBaseJurosDefasadaAux, dataReferenciaDescapitalizacao)));
                        }
                        else
                        {
                            int complemento = 0;
                            if ((moduloSigimAppService.ObtemQuantidadeDeMeses(dataBaseJurosDefasadaAux, dataReferenciaDescapitalizacao) - 1) < 0)
                            {
                                complemento = 0;
                            }
                            else
                            {
                                complemento = (moduloSigimAppService.ObtemQuantidadeDeMeses(dataBaseJurosDefasadaAux, dataReferenciaDescapitalizacao) - 1);
                            }

                            int complemento1 = 0;
                            if (moduloSigimAppService.AcertaData(dataReferenciaDescapitalizacao.Year, (dataReferenciaDescapitalizacao.Month - 1), 31).Day == 31)
                            {
                                complemento1 = moduloSigimAppService.ObtemQuantidadeDeDias(
                                    moduloSigimAppService.AcertaData(dataReferenciaDescapitalizacao.Year, (dataReferenciaDescapitalizacao.Month - 1), dataVencimentoDefasadaAux.Day),
                                    dataReferenciaDescapitalizacao) - 1;
                            }
                            else
                            {
                                complemento1 = moduloSigimAppService.ObtemQuantidadeDeDias(
                                    moduloSigimAppService.AcertaData(dataReferenciaDescapitalizacao.Year, (dataReferenciaDescapitalizacao.Month - 1), dataVencimentoDefasadaAux.Day),
                                    dataReferenciaDescapitalizacao);
                            }


                            valorTituloPresenteCheio = 
                                valorAmortizacaoDataReferencia +
                                (valorAmortizacaoDataReferencia * 
                                 (percentualJuros * 
                                  complemento)) +
                                (valorAmortizacaoDataReferencia *
                                 (percentualJuros / 30) *
                                   complemento1);                                 
                        }
                    }

                    //bool condicao = false;

                    //if (titulo.VendaSerie.RenegociacaoId.HasValue)
                    //{
                    //    DateTime dataReferenciaRenegociacao = titulo.VendaSerie.Renegociacao.DataReferencia.HasValue ? titulo.VendaSerie.Renegociacao.DataReferencia.Value, new DateTime();
                    //    condicao = dataReferenciaDescapitalizacao <= titulo.VendaSerie.Renegociacao.DataReferencia
                    //}
                    //if (condicao &&
                    //    (valorPercentualJuros > 0) &&
                    //    (titulo.VendaSerie.FormaFinanciamento == "3"))
                    //{
                    //}

                    #endregion



                }



                listaTituloDetalheCredCob.Add(tituloDetalhe);
            }


            return listaTituloDetalheCredCob;
        }

        #endregion


        #region "Métodos privados"

        private string[] PopulaArrayComCodigosDeClassesSelecionadas(List<ClasseDTO> listaClasses)
        {
            List<Classe> listaClassesSelecionadas = new List<Classe>();

            foreach (var classeSelecionada in listaClasses)
            {
                var classe = classeRepository.ObterPeloCodigo(classeSelecionada.Codigo, l => l.ListaFilhos);

                MontaArrayClasseExistentes(classe, listaClassesSelecionadas);
            }

            int i = 0;
            string[] arrayCodigoClasse = new string[listaClassesSelecionadas.Count()];
            foreach (var classe in listaClassesSelecionadas)
            {
                arrayCodigoClasse[i] = classe.Codigo;
                i += 1;
            }

            return arrayCodigoClasse;

        }

        private void MontaArrayClasseExistentes(Classe classeSelecionada, List<Classe> listaClassesSelecionadas)
        {
            if (classeSelecionada.ListaFilhos.Count == 0)
            {
                if (!listaClassesSelecionadas.Any(l => l.Codigo == classeSelecionada.Codigo))
                {
                    listaClassesSelecionadas.Add(classeSelecionada);
                }
            }
            else
            {
                foreach (var classeFilha in classeSelecionada.ListaFilhos)
                {
                    MontaArrayClasseExistentes(classeFilha, listaClassesSelecionadas);
                }
            }
        }

        #endregion
    }
}
