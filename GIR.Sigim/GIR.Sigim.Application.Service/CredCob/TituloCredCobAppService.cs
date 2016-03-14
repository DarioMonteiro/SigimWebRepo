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
        private ICotacaoValoresRepository cotacaoValoresRepository;


        #endregion

        #region Construtor

        public TituloCredCobAppService(IUsuarioAppService usuarioAppService,
                                       ITituloCredCobRepository tituloCredCobRepository,
                                       IClasseRepository classeRepository, 
                                       IClienteFornecedorRepository clienteFornecedorRepository,
                                       IParametrosSigimRepository parametrosSigimRepository,
                                       IModuloSigimAppService moduloSigimAppService,
                                       ICotacaoValoresRepository cotacaoValoresRepository, 
                                       MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.tituloCredCobRepository = tituloCredCobRepository;
            this.usuarioAppService = usuarioAppService;
            this.classeRepository = classeRepository;
            this.clienteFornecedorRepository = clienteFornecedorRepository;
            this.parametrosSigimRepository = parametrosSigimRepository;
            this.moduloSigimAppService = moduloSigimAppService;
            this.cotacaoValoresRepository = cotacaoValoresRepository;
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
                        string[] arrayCodigoClasse = PopulaArrayComCodigosDeClassesSelecionadas(filtro.ListaClasseReceita);

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

        public List<TituloDetalheCredCob> RecTit(List<TituloCredCob> listaTituloCredCob,
                                                 Nullable<DateTime> dataReferencia,
                                                 bool excluiTabelaTemporaria = false,
                                                 bool corrigeParcelaResiduo = false)
        {
            List<TituloDetalheCredCob> listaTituloDetalheCredCob = new List<TituloDetalheCredCob>();

            int clienteTitularIdAnterior = 0;
            int clienteTitularId = 0;
            ClienteFornecedor clienteTitular = new ClienteFornecedor();

            ParametrosSigim parametrosSigim = parametrosSigimRepository.Obter();

            foreach (TituloCredCob titulo in listaTituloCredCob)
            {
                DateTime dataVencimentoDefasada;
                DateTime dataReferenciaDefasada;
                decimal valorIndiceDataVencimentoDefasada = 0;
                decimal valorIndiceDataReferenciaDefasada = 0;
                decimal valorIndiceDataVencimento = 0;
                decimal valorIndiceDataReferencia = 0;
                decimal valorPresente = 0;
                decimal valorDescontoAntecipacao = 0;
                int qtdDiasAtraso = 0;
                int qtdMesesAtraso = 0;
                decimal valorTituloDataReferenciaCorrigido = 0;
                decimal valorIndiceDataReferenciaDefasadaAtrasado = 0;
                DateTime dataReferenciaDefasadaAtrasado;
                decimal valorIndiceDataVencimentoDefasadaAtrasado = 0;
                DateTime dataVencimentoDefasadaAtrasado;
                decimal fatorCorrecao = 0;
                decimal fatorCorrecaoProrrata = 0;
                decimal valorCorrecaoAtraso = 0;
                decimal valorCorrecaoProrrata = 0;
                decimal valorMulta = 0;
                decimal valorEncargos = 0;
                decimal valorMultaBanco = 0;
                decimal valorEncargosBanco = 0;
                decimal valorTituloPenalidades = 0;
                decimal valorDevido = 0;
                decimal valorDevidoBanco = 0;
                decimal valorNominal = 0;
                decimal valorTituloDataBase = 0;
                decimal valorAmortizacaoDataBase = 0;
                decimal valorJurosDataBase = 0;
                decimal valorIndiceBaseTemp = 0;
                decimal valorTituloDataReferencia = 0;
                decimal valorAmortizacaoDataReferencia = 0;
                decimal valorJurosDataReferencia = 0;
                decimal valorTituloAtrasado = 0;
                DateTime dataPagamentoDefasada;
                decimal valorIndiceDataPagamentoDefasada = 0;
                decimal valorTituloPagoCorrigido = 0;
                decimal qtdIndicePagamento = 0;
                decimal valorTituloOriginal = 0;
                decimal qtdIndiceJurosOriginal = 0;
                decimal qtdIndiceAmortizacaoOriginal = 0;
                decimal valorDesconto = 0;
                decimal valorDiferencaBaixa = 0;
                decimal valorAtualizado = 0;
                decimal valorRecebido = 0;
                decimal valorVencido = 0;
                decimal valorAVencer = 0;
                decimal valorVinculado = 0;
                decimal valorNaoVinculado = 0;
                decimal valorSaldoDevedor = 0;
                decimal valorPenalidades = 0;
                decimal valorTotalCorrecaoAtraso = 0;
                decimal valorTituloPresente = 0;
                decimal valorTituloPresenteCheio = 0;
                decimal valorTituloPresenteProRataDia = 0;

                TituloDetalheCredCob tituloDetalhe = new TituloDetalheCredCob();

                tituloDetalhe = titulo.To<TituloDetalheCredCob>();

                decimal valorIndiceOriginal = tituloDetalhe.ValorIndiceOriginal.HasValue ? tituloDetalhe.ValorIndiceOriginal.Value : 0;
                decimal qtdIndiceOriginal = tituloDetalhe.QtdIndiceOriginal.HasValue ? tituloDetalhe.QtdIndiceOriginal.Value : 0;
                decimal multaPorAtraso = tituloDetalhe.Contrato.Unidade.MultaPorAtraso.HasValue ? tituloDetalhe.Contrato.Unidade.MultaPorAtraso.Value : 0;
                decimal taxaPermanenciaDiaria = tituloDetalhe.Contrato.Unidade.TaxaPermanenciaDiaria.HasValue ? tituloDetalhe.Contrato.Unidade.TaxaPermanenciaDiaria.Value : 0;
                decimal qtdIndiceAmortizacao = tituloDetalhe.QtdIndiceAmortizacao.HasValue ? tituloDetalhe.QtdIndiceAmortizacao.Value : 0;
                decimal qtdIndiceJuros = tituloDetalhe.QtdIndiceJuros.HasValue ? tituloDetalhe.QtdIndiceJuros.Value : 0;
                decimal valorIndicePagamento = tituloDetalhe.ValorIndicePagamento.HasValue ? tituloDetalhe.ValorIndicePagamento.Value : 0;
                DateTime dataPagamento = tituloDetalhe.DataPagamento.HasValue ? tituloDetalhe.DataPagamento.Value : new DateTime();
                decimal valorBaixa = tituloDetalhe.ValorBaixa.HasValue ? tituloDetalhe.ValorBaixa.Value : 0;
                decimal valorPercentualJuros = tituloDetalhe.ValorPercentualJuros.HasValue ? tituloDetalhe.ValorPercentualJuros.Value : 0;

                if (dataReferencia.HasValue)
                {
                    dataReferencia = dataReferencia.Value.Date;
                }

                if ((tituloDetalhe.Situacao == "P") || (tituloDetalhe.Situacao == "Q"))
                {
                    if (!dataReferencia.HasValue)
                    {
                        dataReferencia = tituloDetalhe.DataVencimento;
                    }

                    #region "Trata dia util"
                    //Trata data referencia
                    if (dataReferencia.HasValue)
                    {
                        if (tituloDetalhe.DataVencimento < dataReferencia)
                        {
                            clienteTitularId = tituloDetalhe.Contrato.ListaVendaParticipante.Where(l => l.TipoParticipanteId == 1).FirstOrDefault().ClienteId;

                            if (clienteTitularId != clienteTitularIdAnterior)
                            {
                                clienteTitular = clienteFornecedorRepository.ObterPeloId(clienteTitularId,
                                                                                         l => l.EnderecoResidencial,
                                                                                         l => l.EnderecoComercial,
                                                                                         l => l.EnderecoOutro);
                                clienteTitularIdAnterior = clienteTitularId;
                            }

                            DateTime dataUtil = tituloDetalhe.DataVencimento.AddDays(-1);

                            if (clienteTitular.Correspondencia == "R")
                            {
                                dataUtil = moduloSigimAppService.RecuperaProximoDiaUtil(dataUtil, clienteTitular.EnderecoResidencial.UnidadeFederacaoSigla);
                            }

                            if (clienteTitular.Correspondencia == "C")
                            {
                                dataUtil = moduloSigimAppService.RecuperaProximoDiaUtil(dataUtil, clienteTitular.EnderecoComercial.UnidadeFederacaoSigla);
                            
                            }

                            if (clienteTitular.Correspondencia == "O")
                            {
                                dataUtil = moduloSigimAppService.RecuperaProximoDiaUtil(dataUtil, clienteTitular.EnderecoOutro.UnidadeFederacaoSigla);
                            }

                            if (dataUtil >= dataReferencia.Value)
                            {
                                dataReferencia = tituloDetalhe.DataVencimento;
                            }

                        }
                    }
                    //Trata data referencia
                    #endregion
                }


                #region "Trata valorAmortizacaoOriginal"

                decimal valorAmortizacaoOriginal = valorIndiceOriginal * qtdIndiceOriginal;

                #endregion

                #region "Trata titulo Pendente"

                if (tituloDetalhe.Situacao == "P") 
                {

                    #region "Calcula cotacao data referencia defasada"

                    valorIndiceDataReferencia = cotacaoValoresRepository.RecuperaCotacao(tituloDetalhe.IndiceId.Value, dataReferencia.Value.Date);
                    
                    #endregion

                    #region "Calcula cotacao data referencia defasada"

                    dataReferenciaDefasada = dataReferencia.Value.Date.AddMonths(tituloDetalhe.VendaSerie.DefasagemMesIndiceCorrecao * -1);
                    valorIndiceDataReferenciaDefasada = cotacaoValoresRepository.RecuperaCotacao(tituloDetalhe.IndiceId.Value, dataReferenciaDefasada.Date);

                    #endregion

                    #region "Calcula cotacao data vencimento"
                    
                    valorIndiceDataVencimento = cotacaoValoresRepository.RecuperaCotacao(tituloDetalhe.IndiceId.Value, tituloDetalhe.DataVencimento.Date);

                    #endregion

                    #region "Calcula cotacao data vencimento defasada"

                    dataVencimentoDefasada = tituloDetalhe.DataVencimento.Date.AddMonths(tituloDetalhe.VendaSerie.DefasagemMesIndiceCorrecao * -1);
                    valorIndiceDataVencimentoDefasada = cotacaoValoresRepository.RecuperaCotacao(tituloDetalhe.IndiceId.Value, dataVencimentoDefasada.Date);
                    
                    #endregion


                    if ((tituloDetalhe.DataVencimento.Day > dataReferencia.Value.Day) &&
                        (tituloDetalhe.DataVencimento < dataReferencia))
                    {
                        #region "Calcula cotacao data referencia"

                        dataReferenciaDefasada = dataReferencia.Value.Date.AddMonths(-1);
                        valorIndiceDataReferenciaDefasada = cotacaoValoresRepository.RecuperaCotacao(tituloDetalhe.IndiceId.Value, dataReferenciaDefasada.Date);
                        
                        #endregion

                    }

                    if (parametrosSigim.CorrecaoMesCheioDiaPrimeiro.HasValue && parametrosSigim.CorrecaoMesCheioDiaPrimeiro.Value)
                    {
                        #region "Calcula cotacao data referencia"

                        dataReferenciaDefasada = dataReferencia.Value.Date.AddMonths(tituloDetalhe.VendaSerie.DefasagemMesIndiceCorrecao * -1);
                        valorIndiceDataReferenciaDefasada = cotacaoValoresRepository.RecuperaCotacao(tituloDetalhe.IndiceId.Value, dataReferenciaDefasada.Date);
                        
                        #endregion


                    }

                    #region "Calcula valor atualizado"

                    if (dataReferencia.Value >= tituloDetalhe.DataVencimento)
                    {
                        valorTituloDataReferencia = tituloDetalhe.QtdIndice * valorIndiceDataVencimentoDefasada;
                        valorAmortizacaoDataReferencia = qtdIndiceAmortizacao * valorIndiceDataVencimentoDefasada;
                        valorJurosDataReferencia = qtdIndiceJuros * valorIndiceDataVencimentoDefasada;
                        valorIndiceBaseTemp = valorIndiceDataVencimentoDefasada;
                    }
                    else
                    {
                        valorTituloDataReferencia = tituloDetalhe.QtdIndice * valorIndiceDataReferenciaDefasada;
                        valorAmortizacaoDataReferencia = qtdIndiceAmortizacao * valorIndiceDataReferenciaDefasada;
                        valorJurosDataReferencia = qtdIndiceJuros * valorIndiceDataReferenciaDefasada;
                        valorIndiceBaseTemp = valorIndiceDataReferenciaDefasada;
                    }

                    #endregion

                    #region "Trata residuo anual"
                    if ((!corrigeParcelaResiduo) & (tituloDetalhe.VendaSerie.CobrancaResiduo == "S"))
                    {
                        valorTituloDataReferencia = tituloDetalhe.QtdIndice * tituloDetalhe.ValorIndiceBase;
                    }

                    if ((corrigeParcelaResiduo) & (tituloDetalhe.VendaSerie.CobrancaResiduo == "S") & (dataReferencia.Value > tituloDetalhe.DataVencimento))
                    {
                        valorTituloDataReferencia = tituloDetalhe.QtdIndice * tituloDetalhe.ValorIndiceBase;
                    }
                    #endregion

                    valorTituloDataBase = valorTituloDataReferencia;
                    valorAmortizacaoDataBase = valorAmortizacaoDataReferencia;
                    valorJurosDataBase = valorJurosDataReferencia;

                    if ((!corrigeParcelaResiduo) & (tituloDetalhe.VendaSerie.CobrancaResiduo == "S"))
                    {
                        valorTituloDataBase = tituloDetalhe.QtdIndice * tituloDetalhe.ValorIndiceBase;
                        valorAmortizacaoDataBase = qtdIndiceAmortizacao * tituloDetalhe.ValorIndiceBase;
                        valorJurosDataBase = qtdIndiceJuros * tituloDetalhe.ValorIndiceBase;
                    }

                    if ((corrigeParcelaResiduo) & (tituloDetalhe.VendaSerie.CobrancaResiduo == "S") & (dataReferencia.Value > tituloDetalhe.DataVencimento))
                    {
                        valorTituloDataBase = tituloDetalhe.QtdIndice * tituloDetalhe.ValorIndiceBase;
                        valorAmortizacaoDataBase = qtdIndiceAmortizacao * tituloDetalhe.ValorIndiceBase;
                        valorJurosDataBase = qtdIndiceJuros * tituloDetalhe.ValorIndiceBase;
                    }

                    #region "Inicializa valor atrasado"

                    valorTituloAtrasado = valorTituloDataBase;
                    decimal percentualJuros = 0;
                    double periodicidade = Convert.ToDouble(tituloDetalhe.VendaSerie.Periodicidade);

                    #endregion

                    #region "Calcula percentual de juros conforme periodicidade da serie"

                    if (tituloDetalhe.VendaSerie.FormaFinanciamento == "1")
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
                    if (tituloDetalhe.DataVencimento < dataReferencia.Value)
                    {
                        qtdMesesDefasagemCorrecaoAtraso =  moduloSigimAppService.ObtemQuantidadeDeMeses(tituloDetalhe.DataVencimento, dataReferencia.Value);
                        //Calcula Dias Prorrata quando a Data de Vencimento for menor que a Data de Referência
                        qtdDiasProrrata = moduloSigimAppService.ObtemQuantidadeDeDias(moduloSigimAppService.AcertaData(dataReferencia.Value.Year, dataReferencia.Value.Month,tituloDetalhe.DataVencimento.Day), dataReferencia.Value);
                        qtdDiasProrrataDescapitalizacao = qtdDiasProrrata;
                    }

                    //Calcula Dias Prorrata quando a Data de Vencimento for menor que a Data de Referência 
                    //e o dia da Data de Vencimento for maior que o dia da Data de Referência

                    DateTime dataAux = new DateTime();
                    if ((tituloDetalhe.DataVencimento < dataReferencia.Value) & (tituloDetalhe.DataVencimento.Day > dataReferencia.Value.Day))
                    {
                        dataAux = moduloSigimAppService.AcertaData(dataReferencia.Value.Year, dataReferencia.Value.Month,tituloDetalhe.DataVencimento.Day);
                        qtdDiasProrrata = moduloSigimAppService.ObtemQuantidadeDeDias(dataAux.AddMonths(-1), dataReferencia.Value);
                        qtdDiasProrrataDescapitalizacao = qtdDiasProrrata;
                    }

                    //Calcula Dias Prorrata quando a Data de Vencimento for maior que a Data de Referência
                    if (tituloDetalhe.DataVencimento > dataReferencia.Value)
                    {
                        qtdDiasProrrata = moduloSigimAppService.ObtemQuantidadeDeDias(dataReferencia.Value,moduloSigimAppService.AcertaData(dataReferencia.Value.Year, dataReferencia.Value.Month, tituloDetalhe.DataVencimento.Day));
                        qtdDiasProrrataDescapitalizacao = qtdDiasProrrata;
                    }

                    //Calcula Dias Prorrata quando a Data de Vencimento for maior que a Data de Referência 
                    //e o dia da Data de Vencimento for menor que o dia da Data de Referência
                    if ((tituloDetalhe.DataVencimento > dataReferencia.Value) & (tituloDetalhe.DataVencimento.Day < dataReferencia.Value.Day))
                    {
                        dataAux = moduloSigimAppService.AcertaData(dataReferencia.Value.Year, dataReferencia.Value.Month, tituloDetalhe.DataVencimento.Day);
                        qtdDiasProrrata = moduloSigimAppService.ObtemQuantidadeDeDias(dataReferencia.Value, dataAux.AddMonths(1));
                        qtdDiasProrrataDescapitalizacao = qtdDiasProrrata;
                    }

                    //Cria a data de referência para descapitalização
                    DateTime dataReferenciaDescapitalizacao = dataReferencia.Value;
                    if (parametrosSigim.MetodoDescapitalizacao == "DV")
                    {
                        dataReferenciaDescapitalizacao = moduloSigimAppService.AcertaData(dataReferencia.Value.Year, dataReferencia.Value.Month, tituloDetalhe.DataVencimento.Day);
                        qtdDiasProrrataDescapitalizacao = 0;
                    }

                    #endregion

                    DateTime dataBaseJuros = tituloDetalhe.VendaSerie.DataBaseJuros.HasValue ? tituloDetalhe.VendaSerie.DataBaseJuros.Value : new DateTime();
                    int defasagemMes = tituloDetalhe.VendaSerie.DefasagemMes.HasValue ? tituloDetalhe.VendaSerie.DefasagemMes.Value : 0;
                    int defasagemDia = tituloDetalhe.VendaSerie.DefasagemDia.HasValue ? tituloDetalhe.VendaSerie.DefasagemDia.Value : 0;

                    DateTime dataBaseJurosDefasadaAux = moduloSigimAppService.CalculaDataDefasagem(dataBaseJuros, (defasagemMes * -1), (defasagemDia * -1));
                    DateTime dataVencimentoDefasadaAux = moduloSigimAppService.CalculaDataDefasagem(tituloDetalhe.DataVencimento, (defasagemMes * -1), (defasagemDia * -1));

                    #region "Tabela Price e SACJS"

                    if ((tituloDetalhe.DataVencimento >= dataReferenciaDescapitalizacao) && 
                        (valorPercentualJuros > 0) &&
                        ((tituloDetalhe.VendaSerie.FormaFinanciamento == "1") || 
                         (tituloDetalhe.VendaSerie.FormaFinanciamento == "2") || 
                         (tituloDetalhe.VendaSerie.FormaFinanciamento == "4"))) 
                    {
                        if (dataReferenciaDescapitalizacao < dataBaseJurosDefasadaAux)
                        {
                            valorTituloPresenteCheio = moduloSigimAppService.CalculaPV(valorTituloDataBase,
                                                                                       (valorPercentualJuros / 1200),
                                                                                       dataBaseJurosDefasadaAux,
                                                                                       tituloDetalhe.DataVencimento,
                                                                                       0);
                        }
                        else
                        {
                            valorTituloPresenteCheio = moduloSigimAppService.CalculaPV(valorTituloDataBase,
                                                                                       (valorPercentualJuros / 1200),
                                                                                       dataReferenciaDescapitalizacao,
                                                                                       tituloDetalhe.DataVencimento,
                                                                                       qtdDiasProrrataDescapitalizacao);
                        }
                    }

                    if ((tituloDetalhe.DataVencimento >= dataReferenciaDescapitalizacao) &&
                        (tituloDetalhe.DataVencimento.Day < dataReferenciaDescapitalizacao.Day) &&
                        (valorPercentualJuros > 0) &&
                        ((tituloDetalhe.VendaSerie.FormaFinanciamento == "1") ||
                         (tituloDetalhe.VendaSerie.FormaFinanciamento == "2") ||
                         (tituloDetalhe.VendaSerie.FormaFinanciamento == "4")))
                    {
                        if (dataReferenciaDescapitalizacao < dataBaseJurosDefasadaAux)
                        {
                            valorTituloPresenteCheio = moduloSigimAppService.CalculaPV(valorTituloDataBase,
                                                                                       (valorPercentualJuros / 1200),
                                                                                       dataBaseJurosDefasadaAux,
                                                                                       tituloDetalhe.DataVencimento,
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
                                                                                       tituloDetalhe.DataVencimento.AddMonths(-1),
                                                                                       qtdDiasProrrataAux);
                        }
                    }

                    #endregion 

                    #region "SACHJS"

                    if ((tituloDetalhe.DataVencimento >= dataReferenciaDescapitalizacao) &&
                        (valorPercentualJuros > 0) &&
                        (tituloDetalhe.VendaSerie.FormaFinanciamento == "3"))
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

                    if ((tituloDetalhe.DataVencimento >= dataReferenciaDescapitalizacao) &&
                        (tituloDetalhe.DataVencimento.Day > dataReferenciaDescapitalizacao.Day) &&
                        (valorPercentualJuros > 0) &&
                        (tituloDetalhe.VendaSerie.FormaFinanciamento == "3"))
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

                    bool condicao = false;

                    if (tituloDetalhe.VendaSerie.RenegociacaoId.HasValue)
                    {
                        condicao = dataReferenciaDescapitalizacao <= tituloDetalhe.VendaSerie.Renegociacao.DataReferencia;
                    }
                    else
                    {
                        if (tituloDetalhe.Contrato.Venda.DataVenda.HasValue)
                        {
                            condicao = dataReferenciaDescapitalizacao <= tituloDetalhe.Contrato.Venda.DataVenda.Value;
                        }
                    }
                    if (condicao &&
                        (valorPercentualJuros > 0) &&
                        (tituloDetalhe.VendaSerie.FormaFinanciamento == "3"))
                    {
                        valorTituloPresenteCheio = valorAmortizacaoDataReferencia;
                    }

                    #endregion

                    #region "JS_Residuo"

                    if ((valorPercentualJuros > 0) && (tituloDetalhe.VendaSerie.FormaFinanciamento == "5"))
                    {
                        valorTituloPresenteCheio = valorTituloDataBase;
                    }

                    #endregion

                    #region "Atualiza valor presente"

                    if (tituloDetalhe.DataVencimento >= dataReferenciaDescapitalizacao)
                    {
                        if (valorPercentualJuros > 0)
                        {
                            valorPresente = valorTituloPresenteCheio;
                        }
                        else
                        {
                            valorPresente = valorTituloDataBase;
                        }
                    }

                    #endregion

                    #region "Atualiza valor do desconto por antecipacao"

                    if (tituloDetalhe.DataVencimento >= dataReferencia)
                    {
                        valorDescontoAntecipacao = valorTituloDataBase - valorPresente;
                    }

                    #endregion

                    #region "Calcula valores de titulos com data de vencimento MENOR que data de referencia e penalidades"

                    if (tituloDetalhe.DataVencimento < dataReferencia)
                    {
                        valorPresente = valorTituloDataBase;
		                qtdDiasAtraso = moduloSigimAppService.ObtemQuantidadeDeDias(tituloDetalhe.DataVencimento, dataReferencia.Value);
		                qtdMesesAtraso = moduloSigimAppService.ObtemQuantidadeDeMeses(tituloDetalhe.DataVencimento, dataReferencia.Value);
                        valorTituloDataReferenciaCorrigido = valorTituloDataReferencia;
                    }

                    #endregion

                    #region "Recupera cotacoes para calculo do atraso"

                    if (tituloDetalhe.DataVencimento < dataReferencia)
                    {
                        #region "Calcula cotacao data referencia atrasado"

                        dataReferenciaDefasadaAtrasado = dataReferencia.Value.AddMonths(1).AddMonths(tituloDetalhe.VendaSerie.DefasagemMesIndiceCorrecao * -1);
                        valorIndiceDataReferenciaDefasadaAtrasado = cotacaoValoresRepository.RecuperaCotacao(tituloDetalhe.IndiceAtrasoCorrecaoId.Value, dataReferenciaDefasadaAtrasado.Date);

                        #endregion

                        #region "Calcula cotacao data vencimento atrasado"

                        dataVencimentoDefasadaAtrasado = dataReferencia.Value.AddMonths(tituloDetalhe.VendaSerie.DefasagemMesIndiceCorrecao * -1);
                        valorIndiceDataVencimentoDefasadaAtrasado = cotacaoValoresRepository.RecuperaCotacao(tituloDetalhe.IndiceAtrasoCorrecaoId.Value, dataVencimentoDefasadaAtrasado.Date);

                        #endregion
                    }

                    if ((tituloDetalhe.DataVencimento < dataReferencia) && (tituloDetalhe.DataVencimento.Day > dataReferencia.Value.Day))
                    {
                        #region "Calcula cotacao data referencia atrasado"

                        dataReferenciaDefasadaAtrasado = dataReferencia.Value.AddMonths(tituloDetalhe.VendaSerie.DefasagemMesIndiceCorrecao * -1);
                        valorIndiceDataReferenciaDefasadaAtrasado = cotacaoValoresRepository.RecuperaCotacao(tituloDetalhe.IndiceAtrasoCorrecaoId.Value, dataReferenciaDefasadaAtrasado.Date);

                        #endregion

                        #region "Calcula cotacao data vencimento atrasado"

                        dataVencimentoDefasadaAtrasado = dataReferencia.Value.AddMonths(-1).AddMonths(tituloDetalhe.VendaSerie.DefasagemMesIndiceCorrecao * -1);
                        valorIndiceDataVencimentoDefasadaAtrasado = cotacaoValoresRepository.RecuperaCotacao(tituloDetalhe.IndiceAtrasoCorrecaoId.Value, dataVencimentoDefasadaAtrasado.Date);

                        #endregion


                    }

                    #endregion

                    #region "Calcula Fator de correção"

                    dataAux = new DateTime();
                    decimal valorCotacaoAux1 = 0;
                    decimal valorCotacaoAux2 = 0;

                    if ((tituloDetalhe.DataVencimento < dataReferencia) && 
                        (tituloDetalhe.IndiceAtrasoCorrecaoId.HasValue && tituloDetalhe.IndiceAtrasoCorrecaoId > 1))
                    {
                        dataAux = dataReferencia.Value.AddMonths(tituloDetalhe.VendaSerie.DefasagemMesIndiceCorrecao * -1);
                        valorCotacaoAux1 = cotacaoValoresRepository.RecuperaCotacao(tituloDetalhe.IndiceAtrasoCorrecaoId.Value, dataAux.Date);

                        dataAux = tituloDetalhe.DataVencimento.AddMonths(tituloDetalhe.VendaSerie.DefasagemMesIndiceCorrecao * -1);
                        valorCotacaoAux2 = cotacaoValoresRepository.RecuperaCotacao(tituloDetalhe.IndiceAtrasoCorrecaoId.Value, dataAux.Date);

                        fatorCorrecao = (valorCotacaoAux1 / valorCotacaoAux2) - 1;
                        fatorCorrecaoProrrata = (((valorIndiceDataReferenciaDefasadaAtrasado / valorIndiceDataVencimentoDefasadaAtrasado) - 1) / 30);
                    }

                    if ((tituloDetalhe.DataVencimento < dataReferencia) &&
                        (tituloDetalhe.IndiceAtrasoCorrecaoId.HasValue && tituloDetalhe.IndiceAtrasoCorrecaoId > 1) &&
                        (tituloDetalhe.DataVencimento.Day > dataReferencia.Value.Day))
                    {
                        dataAux = dataReferencia.Value.AddMonths(-1).AddMonths(tituloDetalhe.VendaSerie.DefasagemMesIndiceCorrecao * -1);
                        valorCotacaoAux1 = cotacaoValoresRepository.RecuperaCotacao(tituloDetalhe.IndiceAtrasoCorrecaoId.Value, dataAux.Date);

                        dataAux = tituloDetalhe.DataVencimento.AddMonths(tituloDetalhe.VendaSerie.DefasagemMesIndiceCorrecao * -1);
                        valorCotacaoAux2 = cotacaoValoresRepository.RecuperaCotacao(tituloDetalhe.IndiceAtrasoCorrecaoId.Value, dataAux.Date);

                        fatorCorrecao = (valorCotacaoAux1 / valorCotacaoAux2) - 1;
                        fatorCorrecaoProrrata = (((valorIndiceDataReferenciaDefasadaAtrasado / valorIndiceDataVencimentoDefasadaAtrasado) - 1) / 30);
                    }

                    #endregion

                    #region "Correcão mês cheio - 1º dia"

                    if ((tituloDetalhe.DataVencimento < dataReferencia) && 
                        (tituloDetalhe.IndiceAtrasoCorrecaoId.HasValue && tituloDetalhe.IndiceAtrasoCorrecaoId > 1) &&
                        ((parametrosSigim.CorrecaoMesCheioDiaPrimeiro.HasValue) && (parametrosSigim.CorrecaoMesCheioDiaPrimeiro.Value)))
                    {
                        dataAux = dataReferencia.Value.AddMonths(tituloDetalhe.VendaSerie.DefasagemMesIndiceCorrecao * -1);
                        valorCotacaoAux1 = cotacaoValoresRepository.RecuperaCotacao(tituloDetalhe.IndiceAtrasoCorrecaoId.Value, dataAux.Date);

                        dataAux = tituloDetalhe.DataVencimento.AddMonths(tituloDetalhe.VendaSerie.DefasagemMesIndiceCorrecao * -1);
                        valorCotacaoAux2 = cotacaoValoresRepository.RecuperaCotacao(tituloDetalhe.IndiceAtrasoCorrecaoId.Value, dataAux.Date);

                        fatorCorrecao = (valorCotacaoAux1 / valorCotacaoAux2) - 1;

                        fatorCorrecaoProrrata = 0;
                    }

                    #endregion

                    #region "Calcula valor corrigido do titulo"

                    if ((tituloDetalhe.DataVencimento < dataReferencia) &&
                        (tituloDetalhe.IndiceAtrasoCorrecaoId.HasValue && tituloDetalhe.IndiceAtrasoCorrecaoId > 1))
                    {
                        valorTituloDataReferenciaCorrigido = valorTituloDataReferencia + (valorTituloDataReferencia * fatorCorrecao);

                        valorCorrecaoAtraso = valorTituloDataReferenciaCorrigido - valorTituloDataReferencia;
                    }

                    #endregion

                    #region "Calcula valor prorrata dia"
                    if ((tituloDetalhe.DataVencimento < dataReferencia) &&
                        (tituloDetalhe.IndiceAtrasoCorrecaoId.HasValue && tituloDetalhe.IndiceAtrasoCorrecaoId > 1) &&
                        parametrosSigim.CorrecaoProRata == 1)
                    {
                        valorCorrecaoProrrata = ((qtdDiasProrrata * fatorCorrecaoProrrata) * valorTituloDataReferenciaCorrigido);
                    }

                    #endregion

                    #region "Atualiza valor do atraso"

                    if (tituloDetalhe.DataVencimento < dataReferencia) 
                    {
                        valorCorrecaoAtraso = valorTituloDataReferenciaCorrigido - valorTituloDataReferencia;
                    }

                    #endregion

                    #region "Calcula penalidades"

                    if (tituloDetalhe.DataVencimento < dataReferencia)
                    {
                        if ((tituloDetalhe.Contrato.Unidade.ConsiderarParametroUnidade.HasValue) && (tituloDetalhe.Contrato.Unidade.ConsiderarParametroUnidade.Value))
                        {
                            valorMulta = (valorTituloDataBase + valorCorrecaoAtraso + valorCorrecaoProrrata) * Convert.ToDecimal(((double)multaPorAtraso / 100.0));

                            valorEncargos = (valorTituloDataBase + valorCorrecaoAtraso + valorCorrecaoProrrata) * Convert.ToDecimal(((double)taxaPermanenciaDiaria / 100.0)) * qtdDiasAtraso;

                            valorMultaBanco = valorTituloDataBase * Convert.ToDecimal(((double)multaPorAtraso / 100.0));

                            valorEncargosBanco = valorTituloDataBase * Convert.ToDecimal(((double)taxaPermanenciaDiaria / 100.0)) * qtdDiasAtraso;

                        }
                        else
                        {
                            valorMulta = (valorTituloDataBase + valorCorrecaoAtraso + valorCorrecaoProrrata) * Convert.ToDecimal(((double)parametrosSigim.PercentualMultaAtraso / 100.0));

                            valorEncargos = (valorTituloDataBase + valorCorrecaoAtraso + valorCorrecaoProrrata) * Convert.ToDecimal(((double)parametrosSigim.PercentualEncargosAtraso / 100.0)) * qtdDiasAtraso;

                            valorMultaBanco = valorTituloDataBase * Convert.ToDecimal(((double)parametrosSigim.PercentualMultaAtraso / 100.0));

                            valorEncargosBanco = valorTituloDataBase * Convert.ToDecimal(((double)parametrosSigim.PercentualEncargosAtraso / 100.0)) * qtdDiasAtraso;
                        }

                    }

                    #endregion

                    #region "Calcula penalidades encargos mês cheio"

                    if ((tituloDetalhe.DataVencimento < dataReferencia) && ((parametrosSigim.AplicaEncargosPorMes.HasValue) && parametrosSigim.AplicaEncargosPorMes.Value))
                    {
                        if ((tituloDetalhe.Contrato.Unidade.ConsiderarParametroUnidade.HasValue) && (tituloDetalhe.Contrato.Unidade.ConsiderarParametroUnidade.Value))
                        {
                            valorEncargos = (valorTituloDataBase + valorCorrecaoAtraso + valorCorrecaoProrrata) * Math.Round(Convert.ToDecimal((((double)taxaPermanenciaDiaria * 30) / 100.0)), 2) * (qtdMesesAtraso + 1);
                        }
                        else
                        {
                            valorEncargos = (valorTituloDataBase + valorCorrecaoAtraso + valorCorrecaoProrrata) * Math.Round(Convert.ToDecimal((((double)parametrosSigim.PercentualEncargosAtraso * 30) / 100.0)), 2) * (qtdMesesAtraso + 1);
                        }

                    }

                    if (tituloDetalhe.DataVencimento < dataReferencia)
                    {
                        valorTituloAtrasado = valorTituloDataBase + valorMulta + valorEncargos + valorCorrecaoAtraso + valorCorrecaoProrrata;
                        valorTituloPenalidades = valorEncargos + valorMulta + valorCorrecaoAtraso + valorCorrecaoProrrata;
                    }

                    #endregion

                    valorDevido = valorTituloAtrasado;
                    valorDevidoBanco = valorTituloDataBase + valorMultaBanco + valorEncargosBanco;
                    valorNominal = valorTituloDataBase;

                }

                #endregion

                #region "Trata titulo cancelado"

                if (tituloDetalhe.Situacao == "C")
                {
                    valorTituloDataBase = tituloDetalhe.QtdIndice * tituloDetalhe.ValorIndiceBase;
                    valorAmortizacaoDataBase = qtdIndiceAmortizacao * tituloDetalhe.ValorIndiceBase;
                    valorJurosDataBase = qtdIndiceJuros * tituloDetalhe.ValorIndiceBase;
                }

                #endregion

                #region "Trata titulo quitado"

                if (tituloDetalhe.Situacao == "Q")
                {

                    #region "Trata titulo quitado com resíduo mensal

                    if (tituloDetalhe.VendaSerie.CobrancaResiduo == "S")
                    {
                        tituloDetalhe.ValorIndiceBase = valorIndicePagamento; 
		                valorIndiceBaseTemp = valorIndicePagamento;
		                valorTituloDataBase = tituloDetalhe.QtdIndice * valorIndicePagamento;
		                valorAmortizacaoDataBase = qtdIndiceAmortizacao * valorIndicePagamento;
		                valorJurosDataBase = qtdIndiceJuros * valorIndicePagamento;
                    }

                    #endregion

		            valorTituloDataReferencia = valorTituloDataBase;
		            valorAmortizacaoDataReferencia = valorAmortizacaoDataBase;
		            valorJurosDataReferencia = valorJurosDataBase;
                    valorNominal = tituloDetalhe.QtdIndice * tituloDetalhe.ValorIndiceBase;

                    valorTituloAtrasado = valorTituloDataBase + valorMulta + valorEncargos + valorCorrecaoAtraso + valorCorrecaoProrrata;
                    valorTituloPenalidades = valorMulta + valorEncargos + valorCorrecaoAtraso + valorCorrecaoProrrata;

                    dataReferenciaDefasada = dataReferencia.Value.Date.AddMonths(tituloDetalhe.VendaSerie.DefasagemMesIndiceCorrecao * -1);
                    dataPagamentoDefasada = dataPagamento.Date.AddMonths(tituloDetalhe.VendaSerie.DefasagemMesIndiceCorrecao * -1);

                    #region "Calcula Valor Pago Corrigido"

                    valorIndiceDataReferencia = cotacaoValoresRepository.RecuperaCotacao(tituloDetalhe.IndiceId.Value, dataReferencia.Value.Date);
                    valorIndiceDataReferenciaDefasada = cotacaoValoresRepository.RecuperaCotacao(tituloDetalhe.IndiceId.Value, dataReferenciaDefasada);
                    valorIndiceDataPagamentoDefasada = cotacaoValoresRepository.RecuperaCotacao(tituloDetalhe.IndiceId.Value, dataPagamentoDefasada);

                    if (dataPagamento <= tituloDetalhe.DataVencimento)
                    {
                        valorTituloPagoCorrigido = valorBaixa;
                    }
                    else
                    {
                        valorTituloPagoCorrigido = valorTituloDataBase;
                    }

                    qtdIndicePagamento = valorTituloPagoCorrigido / valorIndiceDataPagamentoDefasada;
                    valorTituloPagoCorrigido = qtdIndicePagamento * valorIndiceDataReferenciaDefasada;

                    #endregion

                }

                #endregion

                #region "Calcula Valor original do título"

                valorTituloOriginal = qtdIndiceOriginal * valorIndiceOriginal;

                #endregion

		        tituloDetalhe.QtdIndice = moduloSigimAppService.AplicaPercentual(tituloDetalhe.QtdIndice, tituloDetalhe.PercentualReceita);
		        tituloDetalhe.QtdIndiceAmortizacao = moduloSigimAppService.AplicaPercentual(qtdIndiceAmortizacao, tituloDetalhe.PercentualReceita);
		        tituloDetalhe.QtdIndiceJuros = moduloSigimAppService.AplicaPercentual(qtdIndiceJuros, tituloDetalhe.PercentualReceita);
		        tituloDetalhe.QtdIndiceJurosOriginal = moduloSigimAppService.AplicaPercentual(qtdIndiceJurosOriginal, tituloDetalhe.PercentualReceita);
		        tituloDetalhe.QtdIndiceOriginal = moduloSigimAppService.AplicaPercentual(qtdIndiceOriginal, tituloDetalhe.PercentualReceita);
		        tituloDetalhe.QtdIndiceAmortizacaoOriginal = moduloSigimAppService.AplicaPercentual(qtdIndiceAmortizacaoOriginal, tituloDetalhe.PercentualReceita);
		        tituloDetalhe.ValorBaixa = moduloSigimAppService.AplicaPercentual(valorBaixa, tituloDetalhe.PercentualReceita);
		        tituloDetalhe.ValorDesconto = moduloSigimAppService.AplicaPercentual(valorDesconto, tituloDetalhe.PercentualReceita);
		        tituloDetalhe.ValorDescontoAntecipacao = moduloSigimAppService.AplicaPercentual(valorDescontoAntecipacao, tituloDetalhe.PercentualReceita);
		        tituloDetalhe.ValorDiferencaBaixa = moduloSigimAppService.AplicaPercentual(valorDiferencaBaixa, tituloDetalhe.PercentualReceita);
		        tituloDetalhe.ValorCorrecaoAtraso = moduloSigimAppService.AplicaPercentual(valorCorrecaoAtraso, tituloDetalhe.PercentualReceita);
		        tituloDetalhe.ValorCorrecaoProrrata = moduloSigimAppService.AplicaPercentual(valorCorrecaoProrrata, tituloDetalhe.PercentualReceita);
		        tituloDetalhe.ValorEncargos = moduloSigimAppService.AplicaPercentual(valorEncargos, tituloDetalhe.PercentualReceita);
		        tituloDetalhe.ValorMulta = moduloSigimAppService.AplicaPercentual(valorMulta, tituloDetalhe.PercentualReceita);
		        tituloDetalhe.ValorPercentualJuros = moduloSigimAppService.AplicaPercentual(valorPercentualJuros, tituloDetalhe.PercentualReceita);
		        tituloDetalhe.ValorNominal = moduloSigimAppService.AplicaPercentual(valorNominal, tituloDetalhe.PercentualReceita);
		        tituloDetalhe.ValorDevido = moduloSigimAppService.AplicaPercentual(valorDevido, tituloDetalhe.PercentualReceita);
		        tituloDetalhe.ValorAtualizado = moduloSigimAppService.AplicaPercentual(valorAtualizado, tituloDetalhe.PercentualReceita);
		        tituloDetalhe.ValorRecebido = moduloSigimAppService.AplicaPercentual(valorRecebido, tituloDetalhe.PercentualReceita);
		        tituloDetalhe.ValorVencido = moduloSigimAppService.AplicaPercentual(valorVencido, tituloDetalhe.PercentualReceita);
		        tituloDetalhe.ValorAVencer = moduloSigimAppService.AplicaPercentual(valorAVencer, tituloDetalhe.PercentualReceita);
		        tituloDetalhe.ValorVinculado = moduloSigimAppService.AplicaPercentual(valorVinculado, tituloDetalhe.PercentualReceita);
		        tituloDetalhe.ValorNaoVinculado = moduloSigimAppService.AplicaPercentual(valorNaoVinculado, tituloDetalhe.PercentualReceita);
		        tituloDetalhe.ValorSaldoDevedor = moduloSigimAppService.AplicaPercentual(valorSaldoDevedor, tituloDetalhe.PercentualReceita);
		        tituloDetalhe.ValorPenalidades = moduloSigimAppService.AplicaPercentual(valorPenalidades, tituloDetalhe.PercentualReceita);
		        tituloDetalhe.ValorTotalCorrecaoAtraso = moduloSigimAppService.AplicaPercentual(valorTotalCorrecaoAtraso, tituloDetalhe.PercentualReceita);
		        tituloDetalhe.ValorPresente = moduloSigimAppService.AplicaPercentual(valorPresente, tituloDetalhe.PercentualReceita);
		        tituloDetalhe.ValorTituloDataReferencia = moduloSigimAppService.AplicaPercentual(valorTituloDataReferencia, tituloDetalhe.PercentualReceita);
		        tituloDetalhe.ValorAmortizacaoDataReferencia = moduloSigimAppService.AplicaPercentual(valorAmortizacaoDataReferencia, tituloDetalhe.PercentualReceita);
		        tituloDetalhe.ValorJurosDataReferencia = moduloSigimAppService.AplicaPercentual(valorJurosDataReferencia, tituloDetalhe.PercentualReceita);
		        tituloDetalhe.ValorTituloDataBase = moduloSigimAppService.AplicaPercentual(valorTituloDataBase, tituloDetalhe.PercentualReceita);
		        tituloDetalhe.ValorAmortizacaoDataBase = moduloSigimAppService.AplicaPercentual(valorAmortizacaoDataBase, tituloDetalhe.PercentualReceita);
		        tituloDetalhe.ValorTituloPresente = moduloSigimAppService.AplicaPercentual(valorTituloPresente, tituloDetalhe.PercentualReceita);
		        tituloDetalhe.ValorTituloPresenteCheio = moduloSigimAppService.AplicaPercentual(valorTituloPresenteCheio, tituloDetalhe.PercentualReceita);
		        tituloDetalhe.ValorTituloPresenteProRataDia = moduloSigimAppService.AplicaPercentual(valorTituloPresenteProRataDia, tituloDetalhe.PercentualReceita);
		        tituloDetalhe.ValorTituloAtrasado = moduloSigimAppService.AplicaPercentual(valorTituloAtrasado, tituloDetalhe.PercentualReceita);
		        tituloDetalhe.ValorTituloDataReferenciaCorrigido = moduloSigimAppService.AplicaPercentual(valorTituloDataReferenciaCorrigido, tituloDetalhe.PercentualReceita);
		        tituloDetalhe.ValorTituloPenalidades = moduloSigimAppService.AplicaPercentual(valorTituloPenalidades, tituloDetalhe.PercentualReceita);
		        tituloDetalhe.ValorTituloPagoCorrigido = moduloSigimAppService.AplicaPercentual(valorTituloPagoCorrigido, tituloDetalhe.PercentualReceita);
		        tituloDetalhe.QtdIndicePagamento = moduloSigimAppService.AplicaPercentual(qtdIndicePagamento, tituloDetalhe.PercentualReceita);
		        tituloDetalhe.ValorTituloOriginal = moduloSigimAppService.AplicaPercentual(valorTituloOriginal, tituloDetalhe.PercentualReceita);
		        tituloDetalhe.ValorMultaBanco = moduloSigimAppService.AplicaPercentual(valorMultaBanco, tituloDetalhe.PercentualReceita);
		        tituloDetalhe.ValorEncargosBanco = moduloSigimAppService.AplicaPercentual(valorEncargosBanco, tituloDetalhe.PercentualReceita);
                tituloDetalhe.ValorDevidoBanco = moduloSigimAppService.AplicaPercentual(valorDevidoBanco, tituloDetalhe.PercentualReceita);

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
