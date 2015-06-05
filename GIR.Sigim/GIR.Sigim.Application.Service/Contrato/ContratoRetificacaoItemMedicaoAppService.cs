using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Domain.Repository.Contrato;
using System.Threading.Tasks;
using GIR.Sigim.Application.Adapter;
using GIR.Sigim.Domain.Entity.Contrato;
using GIR.Sigim.Application.DTO.Contrato;
using GIR.Sigim.Application.Service.Financeiro;
using GIR.Sigim.Domain.Repository.Financeiro;
using GIR.Sigim.Domain.Repository.Sigim;
using GIR.Sigim.Application.Service.Sigim;
using GIR.Sigim.Application.DTO.Sigim;

namespace GIR.Sigim.Application.Service.Contrato
{
    public class ContratoRetificacaoItemMedicaoAppService : BaseAppService, IContratoRetificacaoItemMedicaoAppService
    {
        #region Declaração

        private IContratoRetificacaoItemMedicaoRepository contratoRetificacaoItemMedicaoRepository;
        private ITituloPagarRepository tituloPagarRepository;
        private IParametrosContratoRepository parametrosContratoRepository;
        private IBloqueioContabilRepository bloqueioContabilRepository;
        private ITituloPagarAppService tituloPagarAppService;
        private IParametrosContratoAppService parametrosContratoAppService;
        private IBloqueioContabilAppService bloqueioContabilAppService;
        private ILogOperacaoAppService logOperacaoAppService;

        #endregion

        #region Construtor

        public ContratoRetificacaoItemMedicaoAppService(IContratoRetificacaoItemMedicaoRepository contratoRetificacaoItemMedicaoRepository,
                                                        ITituloPagarRepository tituloPagarRepository,
                                                        IParametrosContratoRepository parametrosContratoRepository,
                                                        IBloqueioContabilRepository bloqueioContabilRepository,
                                                        ITituloPagarAppService tituloPagarAppService,
                                                        IParametrosContratoAppService parametrosContratoAppService,
                                                        IBloqueioContabilAppService bloqueioContabilAppService,
                                                        ILogOperacaoAppService logOperacaoAppService,
                                                        MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.contratoRetificacaoItemMedicaoRepository = contratoRetificacaoItemMedicaoRepository;
            this.tituloPagarRepository = tituloPagarRepository;
            this.parametrosContratoRepository = parametrosContratoRepository;
            this.bloqueioContabilRepository = bloqueioContabilRepository;
            this.tituloPagarAppService = tituloPagarAppService;
            this.parametrosContratoAppService = parametrosContratoAppService;
            this.bloqueioContabilAppService = bloqueioContabilAppService;
            this.logOperacaoAppService = logOperacaoAppService;
        }

        #endregion

        #region Métodos IContratoRetificacaoItemMedicaoAppService

        public void ObterQuantidadesEhValoresMedicao(int contratoId,
                                                     int sequencialItem,
                                                     int sequencialCronograma,
                                                     ref decimal quantidadeTotalMedido,
                                                     ref decimal valorTotalMedido,
                                                     ref decimal quantidadeTotalLiberado,
                                                     ref decimal valorTotalLiberado)
        {

            List<ContratoRetificacaoItemMedicao> listaContratoRetificacaoItemMedicao = 
                    contratoRetificacaoItemMedicaoRepository.ListarPeloFiltro(  ( l => 
                                                                                    l.ContratoId == contratoId
                                                                                 ), 
                                                                                l => l.ContratoRetificacaoItem,
                                                                                l => l.ContratoRetificacaoItemCronograma).Where(c =>
                                                                                    (c.SequencialItem == sequencialItem &&
                                                                                     c.SequencialCronograma == sequencialCronograma)
                                                                                ).ToList <ContratoRetificacaoItemMedicao>();

            var queryMedido =
                        from c in listaContratoRetificacaoItemMedicao
                        where (
                                ((c.Situacao == SituacaoMedicao.AguardandoAprovacao) ||
                                 (c.Situacao == SituacaoMedicao.AguardandoLiberacao) ||
                                 (c.Situacao == SituacaoMedicao.Liberado))
                              )
                        group c by c.Situacao into g
                        select g;

            quantidadeTotalMedido = 0;
            valorTotalMedido = 0;
            quantidadeTotalLiberado = 0;
            valorTotalLiberado = 0;

            foreach (var medicaoGrupo in queryMedido)
            {
                if ((medicaoGrupo.Key == SituacaoMedicao.AguardandoAprovacao) || (medicaoGrupo.Key == SituacaoMedicao.AguardandoLiberacao))
                {

                    foreach (var medicao in medicaoGrupo)
                    {
                        quantidadeTotalMedido = quantidadeTotalMedido + medicao.Quantidade;
                        valorTotalMedido = valorTotalMedido + medicao.Valor;
                    }
                }
                if (medicaoGrupo.Key == SituacaoMedicao.Liberado)
                {

                    foreach (var medicao in medicaoGrupo)
                    {
                        quantidadeTotalLiberado = quantidadeTotalLiberado + medicao.Quantidade;
                        valorTotalLiberado = valorTotalLiberado + medicao.Valor;
                    }
                }

            }

        }

        public bool ExisteNumeroDocumento(Nullable<DateTime> dataEmissao, string numeroDocumento, int? contratadoId)
        {
            bool existe = false;

            if (!string.IsNullOrEmpty(numeroDocumento) && (contratadoId.HasValue))
            {
                List<ContratoRetificacaoItemMedicao> listaContratoRetificacaoItemMedicao;
                string numeroNotaFiscal = RetiraZerosIniciaisNumeroDocumento(numeroDocumento);

                listaContratoRetificacaoItemMedicao =
                contratoRetificacaoItemMedicaoRepository.ListarPeloFiltro((l =>
                                                                                l.NumeroDocumento.EndsWith(numeroNotaFiscal) &&
                                                                                (
                                                                                    (dataEmissao == null) || 
                                                                                    ((dataEmissao != null) && (l.DataEmissao.Year == dataEmissao.Value.Year))
                                                                                ) &&
                                                                                (
                                                                                    (l.MultiFornecedorId == contratadoId) ||
                                                                                    (l.MultiFornecedorId == null && l.Contrato.ContratadoId == contratadoId)
                                                                                )
                                                                            ),
                                                                            l => l.Contrato).ToList<ContratoRetificacaoItemMedicao>();
                if (listaContratoRetificacaoItemMedicao.Count() > 0)
                {
                    string numeroDeZerosIniciais;

                    foreach (var item in listaContratoRetificacaoItemMedicao)
                    {
                        var quantidadeDeZerosIniciais = item.NumeroDocumento.Length - numeroNotaFiscal.Length;
                        numeroDeZerosIniciais = item.NumeroDocumento.Substring(0, quantidadeDeZerosIniciais);
                        if (string.IsNullOrEmpty(numeroDeZerosIniciais))
                        {
                            numeroDeZerosIniciais = "0";
                        }
                        int resultado;
                        if (int.TryParse(numeroDeZerosIniciais,out resultado))
                        {
                            if (Convert.ToInt32(resultado) == 0)
                            {
                                existe = true;
                                break;
                            }
                        }

                    }
                }
            }

            return existe;
        }

        public bool Salvar(ContratoRetificacaoItemMedicaoDTO dto)
        {
            if ((dto == null) )
            {
                throw new ArgumentNullException("dto");
            }

            bool novoRegistro = false;

            ContratoRetificacaoItemMedicao contratoRetificacaoItemMedicao = contratoRetificacaoItemMedicaoRepository.ObterPeloId(dto.Id);

            if (contratoRetificacaoItemMedicao == null)
            {
                novoRegistro = true;
                contratoRetificacaoItemMedicao = new ContratoRetificacaoItemMedicao();
                contratoRetificacaoItemMedicao.DataCadastro = DateTime.Now;
                contratoRetificacaoItemMedicao.UsuarioMedicao = UsuarioLogado.Login;
                contratoRetificacaoItemMedicao.Situacao = SituacaoMedicao.AguardandoAprovacao;
            }

            contratoRetificacaoItemMedicao.ContratoId = dto.ContratoId;
 
            if (!EhValidoSalvar(dto))
            {
                return false;
            }

            PopularEntity(dto, contratoRetificacaoItemMedicao);

            if (Validator.IsValid(contratoRetificacaoItemMedicao, out validationErrors))
            {
                try
                {
                    if (novoRegistro)
                    {
                        contratoRetificacaoItemMedicaoRepository.Inserir(contratoRetificacaoItemMedicao);
                    }
                    else
                    {
                        contratoRetificacaoItemMedicaoRepository.Alterar(contratoRetificacaoItemMedicao);
                    }

                    contratoRetificacaoItemMedicaoRepository.UnitOfWork.Commit();

                    GravarLogOperacao(contratoRetificacaoItemMedicao, novoRegistro ? "INSERT" : "UPDATE");

                    messageQueue.Add(Resource.Sigim.SuccessMessages.SalvoComSucesso, TypeMessage.Success);
                    return true;
                }
                catch (Exception exception)
                {
                    QueueExeptionMessages(exception);
                }
            }
            else
                messageQueue.AddRange(validationErrors, TypeMessage.Error);

            return false;
        }

        public List<ContratoRetificacaoItemMedicaoDTO> ObtemPorSequencialItem(int contratoId, int sequencialItem)
        {
            List<ContratoRetificacaoItemMedicaoDTO> listaMedicao = null; 

            listaMedicao = contratoRetificacaoItemMedicaoRepository.ListarPeloFiltro(l => l.ContratoId == contratoId && l.SequencialItem == sequencialItem, l => l.TipoDocumento).OrderBy(l => l.DataVencimento).To<List<ContratoRetificacaoItemMedicaoDTO>>();

            return listaMedicao;
        }

        public ContratoRetificacaoItemMedicaoDTO ObterPeloId(int contratoRetificacaoItemMedicaoId)
        {
            decimal quantidadeTotalMedido = 0;
            decimal valorTotalMedido = 0;
            decimal quantidadeTotalLiberado = 0;
            decimal valorTotalLiberado = 0;


            var medicao = contratoRetificacaoItemMedicaoRepository.ObterPeloId(contratoRetificacaoItemMedicaoId,
                                                                                l => l.ContratoRetificacaoItemCronograma).To<ContratoRetificacaoItemMedicaoDTO>();

            if (medicao != null)
            {

                ObterQuantidadesEhValoresMedicao(medicao.ContratoId,
                                                 medicao.SequencialItem,
                                                 medicao.SequencialCronograma,
                                                 ref quantidadeTotalMedido,
                                                 ref valorTotalMedido,
                                                 ref quantidadeTotalLiberado,
                                                 ref valorTotalLiberado);
            }

            medicao.Totalizadores.QuantidadeTotalMedida = quantidadeTotalMedido;
            medicao.Totalizadores.ValorTotalMedido = valorTotalMedido;
            medicao.Totalizadores.QuantidadeTotalLiberada = quantidadeTotalLiberado;
            medicao.Totalizadores.ValorTotalLiberado = valorTotalLiberado;

            return medicao;
        }

        public bool EhValidaMedicaoRecuperada(ContratoRetificacaoItemMedicaoDTO dto)
        {
            if (dto == null)
            {
                messageQueue.Add(Resource.Contrato.ErrorMessages.MedicaoNaoEncontrada, TypeMessage.Error);
                return false;
            }

            return true;
        }


        public bool Cancelar(int? contratoRetificacaoItemMedicaoId)
        {
            if (!EhValidoCancelar(contratoRetificacaoItemMedicaoId))
            {
                return false;
            }

            var contratoRetificacaoItemMedicao = contratoRetificacaoItemMedicaoRepository.ObterPeloId(contratoRetificacaoItemMedicaoId);

            try
            {
                contratoRetificacaoItemMedicaoRepository.Remover(contratoRetificacaoItemMedicao);
                contratoRetificacaoItemMedicaoRepository.UnitOfWork.Commit();
                messageQueue.Add(Resource.Sigim.SuccessMessages.ExcluidoComSucesso, TypeMessage.Success);
                return true;
            }
            catch (Exception)
            {
                messageQueue.Add(Resource.Sigim.ErrorMessages.GravacaoErro, TypeMessage.Error);
                return false;
            }
        }

        #endregion


        #region Métodos privados

        private bool EhValidoSalvar(ContratoRetificacaoItemMedicaoDTO dto)
        {
            int contratadoId;

            contratadoId = dto.Contrato.ContratadoId;

            if (contratadoId == 0)
            {
                messageQueue.Add(string.Format(Application.Resource.Sigim.ErrorMessages.CampoObrigatorio, "Contratado"), TypeMessage.Error);
                return false;
            }

            if (dto.MultiFornecedorId.HasValue){
                contratadoId = dto.MultiFornecedorId.Value;
            }

            if (dto.DataEmissao == null)
            {
                messageQueue.Add(string.Format(Application.Resource.Sigim.ErrorMessages.CampoObrigatorio,"Data emissão"), TypeMessage.Error);
                return false;
            }

            if (!ValidaData(dto.DataEmissao.ToShortDateString()))
            {
                messageQueue.Add(string.Format(Application.Resource.Sigim.ErrorMessages.CampoInvalido, "Data emissão"), TypeMessage.Error);
                return false;
            }

            if (dto.DataVencimento == null)
            {
                messageQueue.Add(string.Format(Application.Resource.Sigim.ErrorMessages.CampoObrigatorio, "Data vencimento"), TypeMessage.Error);
                return false;
            }

            if (!ValidaData(dto.DataVencimento.ToShortDateString()))
            {
                messageQueue.Add(string.Format(Application.Resource.Sigim.ErrorMessages.CampoInvalido, "Data vencimento"), TypeMessage.Error);
                return false;
            }

            if (ComparaDatas(dto.DataEmissao.ToShortDateString(), dto.DataVencimento.ToShortDateString()) < 0)
            {
                string msg = string.Format(Resource.Contrato.ErrorMessages.DataMaiorQue, "Data emissão", "Data vencimento");
                messageQueue.Add(msg, TypeMessage.Error);
                return false;
            }

            var parametros = parametrosContratoAppService.Obter().To<ParametrosContrato>();

            if (dto.TipoDocumentoId == 0)
            {
                messageQueue.Add(string.Format(Application.Resource.Sigim.ErrorMessages.CampoObrigatorio, "Tipo"), TypeMessage.Error);
            }

            if (string.IsNullOrEmpty(dto.NumeroDocumento))
            {
                messageQueue.Add(string.Format(Application.Resource.Sigim.ErrorMessages.CampoObrigatorio, "Documento"), TypeMessage.Error);
                return false;
            }

            bool condicao = !dto.Id.HasValue ? true: ( dto.Id.Value == 0 ? true : false );

            if (condicao)
            {
                if (ExisteNumeroDocumento(dto.DataEmissao,
                                          dto.NumeroDocumento,
                                          contratadoId))
                {
                    messageQueue.Add(Resource.Contrato.ErrorMessages.DocumentoExistente, TypeMessage.Error);
                    return false;
                }

                if (ExisteNumeroDocumentoTituloAPagar(dto.DataEmissao,
                                                      dto.DataVencimento,
                                                      dto.NumeroDocumento,
                                                      contratadoId))
                {
                    messageQueue.Add(Resource.Contrato.ErrorMessages.DocumentoExistente, TypeMessage.Error);
                    return false;
                }
            }

            if (dto.DataMedicao == null)
            {
                messageQueue.Add(string.Format(Application.Resource.Sigim.ErrorMessages.CampoObrigatorio, "Data medição"), TypeMessage.Error);
                return false;
            }

            if (!ValidaData(dto.DataMedicao.ToShortDateString()))
            {
                messageQueue.Add(string.Format(Application.Resource.Sigim.ErrorMessages.CampoInvalido, "Data medição"), TypeMessage.Error);
                return false;
            }

            if (ComparaDatas(dto.DataMedicao.ToShortDateString(), dto.DataVencimento.ToShortDateString()) < 0)
            {
                string msg = string.Format(Resource.Contrato.ErrorMessages.DataMaiorQue, "Data medição", "Data vencimento");
                messageQueue.Add(msg, TypeMessage.Error);
                return false;
            }

            DateTime DataLimiteMedicao = DateTime.Now;

            if (parametros != null)
            {
                if (parametros.DiasPagamento.HasValue)
                {

                    DataLimiteMedicao = DateTime.Now.AddDays((parametros.DiasPagamento.Value * -1));

                    if (parametros.DiasPagamento.Value > 0)
                    {
                        int numeroDias = (int)dto.DataVencimento.Subtract(dto.DataMedicao).TotalDays;
                        if (numeroDias < parametros.DiasPagamento.Value)
                        {
                            messageQueue.Add(Application.Resource.Contrato.ErrorMessages.DataVencimentoForaDoLimite, TypeMessage.Error);
                            return false;
                        }
                    }
                }
            }

            if (ComparaDatas(dto.DataMedicao.ToShortDateString(), DataLimiteMedicao.ToShortDateString()) < 0)
            {
                messageQueue.Add(Application.Resource.Contrato.ErrorMessages.DataMedicaoMenorQueDataLimite, TypeMessage.Error);
                return false;
            }

            if (dto.Quantidade == 0)
            {
                messageQueue.Add(string.Format(Application.Resource.Sigim.ErrorMessages.CampoObrigatorio, "Quantidade medição atual"), TypeMessage.Error);
                return false;
            }

            if (dto.ContratoRetificacaoItem.NaturezaItem == NaturezaItem.PrecoGlobal)
            {
                if (dto.Valor == 0)
                {
                    messageQueue.Add(string.Format(Application.Resource.Sigim.ErrorMessages.CampoObrigatorio, "Valor medição atual"), TypeMessage.Error);
                    return false;
                }
                if (dto.Valor > dto.Totalizadores.ValorPendente)
                {
                    string msg = string.Format(Resource.Contrato.ErrorMessages.ValorMaiorQue, "Valor medição atual", "Valor pendente");
                    messageQueue.Add(msg, TypeMessage.Error);
                    return false;
                }
            }
            else if (dto.ContratoRetificacaoItem.NaturezaItem == NaturezaItem.PrecoUnitario)
            {
                if (dto.Quantidade > dto.Totalizadores.QuantidadePendente)
                {
                    string msg = string.Format(Resource.Contrato.ErrorMessages.ValorMaiorQue, "Quantidade medição atual", "Quantidade pendente");
                    messageQueue.Add(msg, TypeMessage.Error);
                    return false;
                }
            }

            if (dto.Desconto > 0) 
            {
                if (string.IsNullOrEmpty(dto.MotivoDesconto))
                {
                    messageQueue.Add(string.Format(Application.Resource.Sigim.ErrorMessages.CampoObrigatorio, "Motivo desconto"), TypeMessage.Error);
                    return false;
                }
                if (dto.Desconto > dto.Valor)
                {
                    string msg = string.Format(Resource.Contrato.ErrorMessages.ValorMaiorQue, "Desconto", "Valor medição atual");
                    messageQueue.Add(msg, TypeMessage.Error);
                    return false;
                }
            }

            Nullable<DateTime> dataBloqueio;

            if (!bloqueioContabilAppService.ValidaBloqueioContabil(dto.Contrato.CentroCusto.Codigo, 
                                                                   dto.DataEmissao,
                                                                   out dataBloqueio))
            {
                string msg = string.Format(Resource.Sigim.ErrorMessages.BloqueioContabilEncontrado, dataBloqueio.Value.ToShortDateString(), dto.Contrato.CentroCusto.Codigo);
                messageQueue.Add(msg, TypeMessage.Error);
                return false;
            }

            if (parametros == null) return true;

            if (!parametros.DadosSped.HasValue) return true;

            if (parametros.DadosSped.Value)
            {
                if (string.IsNullOrEmpty(dto.TipoCompraCodigo))
                {
                    messageQueue.Add(string.Format(Application.Resource.Sigim.ErrorMessages.CampoObrigatorio, "Tipo compra"), TypeMessage.Error);
                    return false;
                }

                if ((!dto.CifFobId.HasValue) || (dto.CifFobId.Value == 0))
                {
                    messageQueue.Add(string.Format(Application.Resource.Sigim.ErrorMessages.CampoObrigatorio, "CIF/FOB"), TypeMessage.Error);
                    return false;
                }

                if ((string.IsNullOrEmpty(dto.NaturezaOperacaoCodigo)))
                {
                    messageQueue.Add(string.Format(Application.Resource.Sigim.ErrorMessages.CampoObrigatorio, "Natureza de operação"), TypeMessage.Error);
                    return false;
                }

                if ((!dto.SerieNFId.HasValue) || (dto.SerieNFId.Value == 0))
                {
                    messageQueue.Add(string.Format(Application.Resource.Sigim.ErrorMessages.CampoObrigatorio, "Série"), TypeMessage.Error);
                    return false;
                }

                if ((string.IsNullOrEmpty(dto.CSTCodigo)))
                {
                    messageQueue.Add(string.Format(Application.Resource.Sigim.ErrorMessages.CampoObrigatorio, "CST"), TypeMessage.Error);
                    return false;
                }

                if ((string.IsNullOrEmpty(dto.CodigoContribuicaoCodigo)))
                {
                    messageQueue.Add(string.Format(Application.Resource.Sigim.ErrorMessages.CampoObrigatorio, "Contribuição"), TypeMessage.Error);
                    return false;
                }
            }

            return true;
        }

        private void PopularEntity(ContratoRetificacaoItemMedicaoDTO dto, ContratoRetificacaoItemMedicao contratoRetificacaoItemMedicao)
        {
            contratoRetificacaoItemMedicao.Id = dto.Id;
            contratoRetificacaoItemMedicao.ContratoId = dto.ContratoId;
            contratoRetificacaoItemMedicao.ContratoRetificacaoId = dto.ContratoRetificacaoId;
            contratoRetificacaoItemMedicao.ContratoRetificacaoItemId = dto.ContratoRetificacaoItemId;
            contratoRetificacaoItemMedicao.ContratoRetificacaoItemCronogramaId = dto.ContratoRetificacaoItemCronogramaId;
            contratoRetificacaoItemMedicao.SequencialItem = dto.SequencialItem;
            contratoRetificacaoItemMedicao.SequencialCronograma = dto.SequencialCronograma;
            contratoRetificacaoItemMedicao.Situacao = SituacaoMedicao.AguardandoAprovacao;
            contratoRetificacaoItemMedicao.TipoDocumentoId = dto.TipoDocumentoId;
            contratoRetificacaoItemMedicao.NumeroDocumento = dto.NumeroDocumento;
            contratoRetificacaoItemMedicao.DataMedicao = dto.DataMedicao;
            contratoRetificacaoItemMedicao.DataEmissao = dto.DataEmissao;
            contratoRetificacaoItemMedicao.DataVencimento = dto.DataVencimento;
            contratoRetificacaoItemMedicao.Quantidade = dto.Quantidade;
            contratoRetificacaoItemMedicao.Valor = dto.Valor;
            contratoRetificacaoItemMedicao.MultiFornecedorId = dto.MultiFornecedorId;
            contratoRetificacaoItemMedicao.Observacao = dto.Observacao;
            contratoRetificacaoItemMedicao.Desconto = dto.Desconto;
            contratoRetificacaoItemMedicao.MotivoDesconto = dto.MotivoDesconto;

            decimal valorRetido = CalculaValorRetido(dto);

            if (valorRetido > 0)
            {
                contratoRetificacaoItemMedicao.ValorRetido = valorRetido;
            }

            contratoRetificacaoItemMedicao.TipoCompraCodigo = dto.TipoCompraCodigo;
            contratoRetificacaoItemMedicao.CifFobId = dto.CifFobId;
            contratoRetificacaoItemMedicao.NaturezaOperacaoCodigo = dto.NaturezaOperacaoCodigo;
            contratoRetificacaoItemMedicao.SerieNFId = dto.SerieNFId;
            contratoRetificacaoItemMedicao.CSTCodigo = dto.CSTCodigo;
            contratoRetificacaoItemMedicao.CodigoContribuicaoCodigo = dto.CodigoContribuicaoCodigo;
            contratoRetificacaoItemMedicao.CodigoBarras = dto.CodigoBarras;
        }

        private bool ExisteNumeroDocumentoTituloAPagar(DateTime dataEmissao, DateTime dataVencimento, string numeroDocumento, int contratadoId)
        {

            if (tituloPagarAppService.ExisteNumeroDocumento(dataEmissao,
                                                            dataVencimento,
                                                            numeroDocumento,
                                                            contratadoId))
            {
                return false;
            }

            return false;
        }

        private decimal CalculaValorRetido(ContratoRetificacaoItemMedicaoDTO medicao)
        {
            decimal valorRetido = 0.0m;
            decimal percentualRetencao = 0.0m;
            decimal percentualBaseCalculo = 0.0m;

            if (medicao.ContratoRetificacao.RetencaoContratual.HasValue)
            {
                percentualRetencao = medicao.ContratoRetificacao.RetencaoContratual.Value;
                percentualBaseCalculo = 100.0m;
            }

            if (medicao.ContratoRetificacaoItem.RetencaoItem.HasValue)
            {
                percentualRetencao = medicao.ContratoRetificacaoItem.RetencaoItem.Value;
                percentualBaseCalculo = medicao.ContratoRetificacaoItem.BaseRetencaoItem.Value;
            }

            if (percentualRetencao > 0.0m)
            {
                decimal valorBaseCalculo = ((medicao.Valor * percentualBaseCalculo) / 100);
                valorRetido = decimal.Round(((valorBaseCalculo * percentualRetencao) / 100), 2);
            }

            return valorRetido;
        }

        private bool EhValidoCancelar(int? contratoRetificacaoItemMedicaoId)
        {

            if (!contratoRetificacaoItemMedicaoId.HasValue)
            {
                messageQueue.Add(Application.Resource.Contrato.ErrorMessages.SelecioneUmaMedicao, TypeMessage.Error);
                return false;
            }


            if (contratoRetificacaoItemMedicaoId.Value == 0)
            {
                messageQueue.Add(Application.Resource.Contrato.ErrorMessages.SelecioneUmaMedicao, TypeMessage.Error);
                return false;
            }

            return true;
        }

        private string MedicaoToXML(ContratoRetificacaoItemMedicao contratoRetificacaoItemMedicao)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<contratoRetificacaoItemMedicao>");
            sb.Append("<Contrato.contratoRetificacaoItemMedicao>");
            sb.Append("<codigo>" + contratoRetificacaoItemMedicao.Id.ToString() + "</codigo>");
            sb.Append("<contrato>" + contratoRetificacaoItemMedicao.ContratoId.ToString() + "</contrato>");
            //sb.Append("<descricaoContrato>" + contratoRetificacaoItemMedicao.Contrato.ContratoDescricao.Descricao + "</descricaoContrato>");
            sb.Append("<contratoRetificacao>" + contratoRetificacaoItemMedicao.ContratoRetificacaoId.ToString() +  "</contratoRetificacao>");
            sb.Append("<contratoRetificacaoItem>" + contratoRetificacaoItemMedicao.ContratoRetificacaoItemId.ToString() + "</contratoRetificacaoItem>");
            sb.Append("<sequencialItem>" + contratoRetificacaoItemMedicao.SequencialItem.ToString() + "</sequencialItem>");
            //sb.Append("<servico>" + contratoRetificacaoItemMedicao.ContratoRetificacaoItem.ServicoId + "</servico>");
            sb.Append("<contratoRetificacaoItemCronograma>" + contratoRetificacaoItemMedicao.ContratoRetificacaoItemCronogramaId.ToString() + "</contratoRetificacaoItemCronograma>");
            sb.Append("<sequencialCronograma>" + contratoRetificacaoItemMedicao.SequencialCronograma.ToString() + "</sequencialCronograma>");
            sb.Append("<tipoDocumento>" + contratoRetificacaoItemMedicao.TipoDocumentoId.ToString() + "</tipoDocumento>");
            //sb.Append("<tipoDocumentoDescricao>" + contratoRetificacaoItemMedicao.TipoDocumento.Descricao + "</tipoDocumentoDescricao>");
            //sb.Append("<tipoDocumentoSigla>" + contratoRetificacaoItemMedicao.TipoDocumento.Sigla + "</tipoDocumentoSigla>");
            sb.Append("<numeroDocumento>" +  contratoRetificacaoItemMedicao.NumeroDocumento + "</numeroDocumento>");
            sb.Append("<dataCadastro>" + contratoRetificacaoItemMedicao.DataCadastro.ToString() + "</dataCadastro>");
            sb.Append("<dataVencimento>" + contratoRetificacaoItemMedicao.DataVencimento.ToString() + "</dataVencimento>");
            sb.Append("<dataEmissao>" +  contratoRetificacaoItemMedicao.DataEmissao.ToString() + "</dataEmissao>");
            sb.Append("<dataMedicao>" +  contratoRetificacaoItemMedicao.DataMedicao.ToString() + "</dataMedicao>");
            sb.Append("<usuarioMedicao>" + contratoRetificacaoItemMedicao.UsuarioMedicao + "</usuarioMedicao>");
            sb.Append("<valor>" + contratoRetificacaoItemMedicao.Valor.ToString("0.00000") + "</valor>");
            sb.Append("<quantidade>" + contratoRetificacaoItemMedicao.Quantidade.ToString("0.00000") + "</quantidade>");
            if (contratoRetificacaoItemMedicao.ValorRetido.HasValue)
            {
                sb.Append("<valorRetido>" + contratoRetificacaoItemMedicao.ValorRetido.Value.ToString("0.00000") + "</valorRetido>");
            }
            else{
                sb.Append("<valorRetido />");
            }
            sb.Append("<observacao>" + contratoRetificacaoItemMedicao.Observacao + "</observacao>");
            sb.Append("<situacao>" + ((int)contratoRetificacaoItemMedicao.Situacao).ToString()  + "</situacao>");
            if (contratoRetificacaoItemMedicao.Desconto.HasValue)
            {
                sb.Append("<desconto>" +  contratoRetificacaoItemMedicao.Desconto.Value.ToString("0.00000") + "</desconto>");
                sb.Append("<motivoDesconto>" + contratoRetificacaoItemMedicao.MotivoDesconto + "</motivoDesconto>");
            }
            else{
                sb.Append("<desconto />");
                sb.Append("<motivoDesconto />");
            }
            //sb.Append("<situacaoContrato>" + ((int)contratoRetificacaoItemMedicao.Contrato.Situacao).ToString() + "</situacaoContrato>");
            //sb.Append("<tipoContrato>" + contratoRetificacaoItemMedicao.Contrato.TipoContrato.ToString() + "</tipoContrato>");
            //sb.Append("<contratado>" + contratoRetificacaoItemMedicao.Contrato.ContratadoId.ToString() + "</contratado>");
            //sb.Append("<nomeContratado>" + contratoRetificacaoItemMedicao.Contrato.Contratado.Nome + "</nomeContratado>");
            //if (contratoRetificacaoItemMedicao.Contrato.Contratado.TipoPessoa == "F"){
            //    sb.Append("<CPFCNPJContratado>" + contratoRetificacaoItemMedicao.Contrato.Contratado.PessoaFisica.Cpf + "</CPFCNPJContratado>");
            //}
            //else if (contratoRetificacaoItemMedicao.Contrato.Contratado.TipoPessoa == "J"){
            //    sb.Append("<CPFCNPJContratado>" + contratoRetificacaoItemMedicao.Contrato.Contratado.PessoaJuridica.Cnpj + "</CPFCNPJContratado>");
            //}
            //sb.Append("<contratante>" + contratoRetificacaoItemMedicao.Contrato.ContratanteId + "</contratante>");
            //sb.Append("<nomeContratante>" + contratoRetificacaoItemMedicao.Contrato.Contratante.Nome + "</nomeContratante>");
            //if (contratoRetificacaoItemMedicao.Contrato.Contratante.TipoPessoa == "F"){
            //    sb.Append("<CPFCNPJContratante>" + contratoRetificacaoItemMedicao.Contrato.Contratante.PessoaFisica.Cpf + "</CPFCNPJContratante>");
            //}
            //else if (contratoRetificacaoItemMedicao.Contrato.Contratado.TipoPessoa == "J"){
            //    sb.Append("<CPFCNPJContratante>" + contratoRetificacaoItemMedicao.Contrato.Contratante.PessoaJuridica.Cnpj + "</CPFCNPJContratante>");
            //}
            //if (contratoRetificacaoItemMedicao.Contrato.TipoContrato == 0){
            //    sb.Append("<codigoFornecedor>" + contratoRetificacaoItemMedicao.Contrato.ContratadoId + "</codigoFornecedor>");
            //}
            //else{
            //    sb.Append("<codigoFornecedor>" + contratoRetificacaoItemMedicao.Contrato.ContratanteId + "</codigoFornecedor>");
            //}

            //A tag abaixo é um sum do campo valor para todas as medições que ocorreram no contrato, não fiz esse campo"
            //sb.Append("<valorTotalMedidoLiberadoContrato>" +  + "</valorTotalMedidoLiberadoContrato>");
            //decimal valorContrato = 0;
            //if (contratoRetificacaoItemMedicao.Contrato.ValorContrato.HasValue){
            //    sb.Append("<valorContrato>" + contratoRetificacaoItemMedicao.Contrato.ValorContrato.Value.ToString("0.00000") + "</valorContrato>");
            //    valorContrato = contratoRetificacaoItemMedicao.Contrato.ValorContrato.Value;
            //}
            //else{
            //    sb.Append("<valorContrato />");
            //}
            //decimal saldoContrato = 0;
            //saldoContrato = valorContrato - <valorTotalMedidoLiberadoContrato>;
            //sb.Append("<saldoContrato>" + saldoContrato.ToString("0.00000") + "</saldoContrato>");
            //sb.Append("<centroCusto>" + contratoRetificacaoItemMedicao.Contrato.CodigoCentroCusto + "</centroCusto>");
            //sb.Append("<descricaoCentroCusto>" + contratoRetificacaoItemMedicao.Contrato.CentroCusto.Descricao + "</descricaoCentroCusto>");
            //sb.Append("<situacaoCentroCusto>" + contratoRetificacaoItemMedicao.Contrato.CentroCusto.Situacao + "</situacaoCentroCusto>");
            //sb.Append("<classe>" + contratoRetificacaoItemMedicao.ContratoRetificacaoItem.CodigoClasse + "</classe>");
            //sb.Append("<descricaoClasse>" + contratoRetificacaoItemMedicao.ContratoRetificacaoItem.Classe.Descricao + "</descricaoClasse>");
            //sb.Append("<precoUnitario>" + contratoRetificacaoItemMedicao.ContratoRetificacaoItem.PrecoUnitario.ToString("0.00000") + "</precoUnitario>");
            //if (contratoRetificacaoItemMedicao.ContratoRetificacaoItem.ValorItem.HasValue){
            //    sb.Append("<valorItem>" + contratoRetificacaoItemMedicao.ContratoRetificacaoItem.ValorItem.Value.ToString("0.00000") + "</valorItem>");
            //}
            //else{
            //    sb.Append("<valorItem />");
            //}
            //if (contratoRetificacaoItemMedicao.ContratoRetificacaoItem.Quantidade.HasValue){
            //    sb.Append("<quantidadeItem>" + contratoRetificacaoItemMedicao.ContratoRetificacaoItem.Quantidade.Value.ToString("0.00000") + "</valorItem>");
            //}
            //else{
            //    sb.Append("<quantidadeItem />");
            //}
            //sb.Append("<descricaoCronograma>" + contratoRetificacaoItemMedicao.ContratoRetificacaoItemCronograma.Descricao + "</descricaoCronograma>");
            //sb.Append("<quantidadeCronograma>" + contratoRetificacaoItemMedicao.ContratoRetificacaoItemCronograma.Quantidade.ToString("0.00000") + "</quantidadeCronograma>");
            //sb.Append("<valorCronograma>" + contratoRetificacaoItemMedicao.ContratoRetificacaoItemCronograma.Valor.ToString("0.00000") + "</valorCronograma>");
            //sb.Append("<unidadeMedida>" + contratoRetificacaoItemMedicao.ContratoRetificacaoItem.Servico.SiglaUnidadeMedida + "</unidadeMedida>");
            //sb.Append("<descricaoItem>" + contratoRetificacaoItemMedicao.ContratoRetificacaoItem.Servico.Descricao + "</descricaoItem>");
            //sb.Append("<complementoDescricaoItem>" + contratoRetificacaoItemMedicao.ContratoRetificacaoItem.ComplementoDescricao + "</complementoDescricaoItem>");
            sb.Append("<descricaoSituacaoMedicao>" + contratoRetificacaoItemMedicao.Situacao.ObterDescricao() + "</descricaoSituacaoMedicao>");
            //sb.Append("<naturezaItem>" + ((int)contratoRetificacaoItemMedicao.ContratoRetificacaoItem.NaturezaItem).ToString() + "</naturezaItem>");
            sb.Append("<codigoBarras>" + contratoRetificacaoItemMedicao.CodigoBarras + "</codigoBarras>");
            //sb.Append("<valorTotalMedido>" +   + "</valorTotalMedido>");
            //sb.Append("<quantidadeTotalMedida>" +  + "</quantidadeTotalMedida>");
            //sb.Append("<valorTotalLiberado>" +  + "</valorTotalLiberado>");
            //sb.Append("<quantidadeTotalLiberada>" + + "</quantidadeTotalLiberada>");
            //sb.Append("<valorTotalMedidoLiberado>" + + "</valorTotalMedidoLiberado>");
            //sb.Append("<quantidadeTotalMedidaLiberada>" +  + "</quantidadeTotalMedidaLiberada>");
            //sb.Append("<valorImpostoRetido>" + + "</valorImpostoRetido>");
            //sb.Append("<valorImpostoRetidoMedicao>" +  + "</valorImpostoRetidoMedicao>");
            //sb.Append("<valorTotalMedidoNota>" + + "</valorTotalMedidoNota>");
            //sb.Append("<valorImpostoIndiretoMedicao>" +  + "</valorImpostoIndiretoMedicao>");
            //sb.Append("<valorTotalMedidoIndireto>" +  +"</valorTotalMedidoIndireto>");
            sb.Append("</Contrato.contratoRetificacaoItemMedicao>");
            sb.Append("</contratoRetificacaoItemMedicao>");

            return sb.ToString();
        }


        private void GravarLogOperacao(ContratoRetificacaoItemMedicao contratoRetificacaoItemMedicao, string operacao)
        {
            logOperacaoAppService.Gravar("Atualização da medição",
                "Contrato.contratoRetificacaoItemMedicao_Atualiza",
                "Contrato.contratoRetificacaoItemMedicao",
                operacao,
                MedicaoToXML(contratoRetificacaoItemMedicao));

        }

        #endregion
    }
}
