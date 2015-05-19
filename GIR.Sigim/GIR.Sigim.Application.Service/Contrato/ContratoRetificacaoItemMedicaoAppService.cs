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

        #endregion

        #region Construtor

        public ContratoRetificacaoItemMedicaoAppService(IContratoRetificacaoItemMedicaoRepository contratoRetificacaoItemMedicaoRepository,
                                                        ITituloPagarRepository tituloPagarRepository,
                                                        IParametrosContratoRepository parametrosContratoRepository,
                                                        IBloqueioContabilRepository bloqueioContabilRepository,
                                                        MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.contratoRetificacaoItemMedicaoRepository = contratoRetificacaoItemMedicaoRepository;
            this.tituloPagarRepository = tituloPagarRepository;
            this.parametrosContratoRepository = parametrosContratoRepository;
            this.bloqueioContabilRepository = bloqueioContabilRepository;
        }

        #endregion

        #region Métodos IContratoRetificacaoItemMedicaoAppService

        public void ObterQuantidadesEhValoresMedicao(int ContratoRetificacaoItemId,
                                                     int ContratoRetificacaoItemCronogramaId, 
                                                     ref decimal QuantidadeTotalMedido,
                                                     ref decimal ValorTotalMedido,
                                                     ref decimal QuantidadeTotalLiberado,
                                                     ref decimal ValorTotalLiberado)
        {

            List<ContratoRetificacaoItemMedicao> listaContratoRetificacaoItemMedicao = 
                    contratoRetificacaoItemMedicaoRepository.ListarPeloFiltro(  ( l => 
                                                                                    l.ContratoRetificacaoItemId == ContratoRetificacaoItemId && 
                                                                                    l.ContratoRetificacaoItemCronogramaId == ContratoRetificacaoItemCronogramaId
                                                                                 ), 
                                                                                l => l.ContratoRetificacaoItem,
                                                                                l => l.ContratoRetificacaoItemCronograma).Where(c =>
                                                                                    (c.SequencialItem == c.ContratoRetificacaoItem.Sequencial &&
                                                                                     c.SequencialCronograma == c.ContratoRetificacaoItemCronograma.Sequencial)
                                                                                ).ToList <ContratoRetificacaoItemMedicao>();

            var queryMedido =
                        from c in listaContratoRetificacaoItemMedicao
                        where (
                                ((c.Situacao == SituacaoMedicao.AguardandoAprovacao) &&
                                 (c.Situacao == SituacaoMedicao.AguardandoLiberacao))
                              )
                        group c by c.ContratoRetificacaoItemId into g
                        select new
                        {
                            ContratoRetificacaoItemId   = g.Key,
                            QuantidadeTotalMedido       = g.Sum(i => i.Quantidade),
                            ValorTotalMedido            = g.Sum(i => i.Valor)
                        };

            foreach (var item in queryMedido)
            {
                QuantidadeTotalMedido = item.QuantidadeTotalMedido; 
                ValorTotalMedido = item.ValorTotalMedido;
            }

            var queryLiberado = 
                        from c in listaContratoRetificacaoItemMedicao
                        where (
                                (c.Situacao == SituacaoMedicao.Liberado)
                              )
                        group c by c.ContratoRetificacaoItemId into g
                        select new
                        {
                            ContratoRetificacaoItemId   = g.Key,
                            QuantidadeTotalLiberado     = g.Sum(i => i.Quantidade),
                            ValorTotalLiberado          = g.Sum(i => i.Valor)
                        };

            foreach (var item in queryLiberado)
            {
                QuantidadeTotalLiberado = item.QuantidadeTotalLiberado;
                ValorTotalLiberado = item.ValorTotalLiberado;
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
                        if (Convert.ToInt32(numeroDeZerosIniciais) == 0)
                        {
                            existe = true;
                            break;
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


            ContratoRetificacaoItemMedicao contratoRetificacaoItemMedicao = dto.To<ContratoRetificacaoItemMedicao>();
            ContratoRetificacaoItemMedicao entidade = contratoRetificacaoItemMedicaoRepository.ObterPeloId(dto.Id);

            if (entidade == null)
            {
                //contratoRetificacaoItemMedicao = new ContratoRetificacaoItemMedicao();
                contratoRetificacaoItemMedicao.DataCadastro = DateTime.Now;
                contratoRetificacaoItemMedicao.UsuarioMedicao = AuthenticationService.GetUser().Login;
            }

            if (!EhValido(contratoRetificacaoItemMedicao))
            {
                return false;
            }

            messageQueue.Add(Resource.Sigim.SuccessMessages.SalvoComSucesso, TypeMessage.Success);

            return true;
        }

        #endregion

        #region Métodos privados

        private bool EhValido(ContratoRetificacaoItemMedicao contratoRetificacaoItemMedicao)
        {
            int contratadoId;

            contratadoId = contratoRetificacaoItemMedicao.Contrato.ContratadoId;

            if (contratadoId == 0)
            {
                messageQueue.Add(string.Format(Application.Resource.Sigim.ErrorMessages.CampoObrigatorio, "Contratado"), TypeMessage.Error);
                return false;
            }

            if (contratoRetificacaoItemMedicao.MultiFornecedorId.HasValue){
                contratadoId = contratoRetificacaoItemMedicao.MultiFornecedorId.Value;
            }

            if (contratoRetificacaoItemMedicao.DataEmissao == null)
            {
                messageQueue.Add(string.Format(Application.Resource.Sigim.ErrorMessages.CampoObrigatorio,"Data emissão"), TypeMessage.Error);
                return false;
            }

            if (!ValidaData(contratoRetificacaoItemMedicao.DataEmissao.ToShortDateString()))
            {
                messageQueue.Add(string.Format(Application.Resource.Sigim.ErrorMessages.CampoInvalido, "Data emissão"), TypeMessage.Error);
                return false;
            }

            if (contratoRetificacaoItemMedicao.DataVencimento == null)
            {
                messageQueue.Add(string.Format(Application.Resource.Sigim.ErrorMessages.CampoObrigatorio, "Data vencimento"), TypeMessage.Error);
                return false;
            }

            if (!ValidaData(contratoRetificacaoItemMedicao.DataVencimento.ToShortDateString()))
            {
                messageQueue.Add(string.Format(Application.Resource.Sigim.ErrorMessages.CampoInvalido, "Data vencimento"), TypeMessage.Error);
                return false;
            }

            if (ComparaDatas(contratoRetificacaoItemMedicao.DataEmissao.ToShortDateString(), contratoRetificacaoItemMedicao.DataVencimento.ToShortDateString()) < 0)
            {
                string msg = string.Format(Resource.Contrato.ErrorMessages.DataMaiorQue, "Data emissão", "Data vencimento");
                messageQueue.Add(msg, TypeMessage.Error);
                return false;
            }

            IParametrosContratoAppService parametrosContratoAppService = new ParametrosContratoAppService(parametrosContratoRepository, messageQueue);
            var parametros = parametrosContratoAppService.Obter().To<ParametrosContrato>();

            if (contratoRetificacaoItemMedicao.TipoDocumentoId == 0)
            {
                messageQueue.Add(string.Format(Application.Resource.Sigim.ErrorMessages.CampoObrigatorio, "Tipo"), TypeMessage.Error);
            }

            if (string.IsNullOrEmpty(contratoRetificacaoItemMedicao.NumeroDocumento))
            {
                messageQueue.Add(string.Format(Application.Resource.Sigim.ErrorMessages.CampoObrigatorio, "Documento"), TypeMessage.Error);
                return false;
            }

            if (ExisteNumeroDocumento(contratoRetificacaoItemMedicao.DataEmissao, 
                                      contratoRetificacaoItemMedicao.NumeroDocumento,
                                      contratadoId))
            {
                messageQueue.Add(Resource.Contrato.ErrorMessages.DocumentoExistente, TypeMessage.Error);
                return false;
            }

            if (ExisteNumeroDocumentoTituloAPagar(contratoRetificacaoItemMedicao.DataEmissao, 
                                                  contratoRetificacaoItemMedicao.DataVencimento, 
                                                  contratoRetificacaoItemMedicao.NumeroDocumento, 
                                                  contratadoId))
            {
                messageQueue.Add(Resource.Contrato.ErrorMessages.DocumentoExistente, TypeMessage.Error);
                return false;
            }

            if (contratoRetificacaoItemMedicao.DataMedicao == null)
            {
                messageQueue.Add(string.Format(Application.Resource.Sigim.ErrorMessages.CampoObrigatorio, "Data medição"), TypeMessage.Error);
                return false;
            }

            if (!ValidaData(contratoRetificacaoItemMedicao.DataMedicao.ToShortDateString()))
            {
                messageQueue.Add(string.Format(Application.Resource.Sigim.ErrorMessages.CampoInvalido, "Data medição"), TypeMessage.Error);
                return false;
            }

            if (ComparaDatas(contratoRetificacaoItemMedicao.DataMedicao.ToShortDateString(), contratoRetificacaoItemMedicao.DataVencimento.ToShortDateString()) < 0)
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
                        int numeroDias = (int)contratoRetificacaoItemMedicao.DataVencimento.Subtract(contratoRetificacaoItemMedicao.DataMedicao).TotalDays;
                        if (numeroDias < parametros.DiasPagamento.Value)
                        {
                            messageQueue.Add(Application.Resource.Contrato.ErrorMessages.DataVencimentoForaDoLimite, TypeMessage.Error);
                            return false;
                        }
                    }
                }
            }

            if (ComparaDatas(contratoRetificacaoItemMedicao.DataMedicao.ToShortDateString(), DataLimiteMedicao.ToShortDateString()) < 0)
            {
                messageQueue.Add(Application.Resource.Contrato.ErrorMessages.DataMedicaoMenorQueDataLimite, TypeMessage.Error);
                return false;
            }

            if (contratoRetificacaoItemMedicao.Quantidade == 0)
            {
                messageQueue.Add(string.Format(Application.Resource.Sigim.ErrorMessages.CampoObrigatorio, "Quantidade medição atual"), TypeMessage.Error);
                return false;
            }

            if (contratoRetificacaoItemMedicao.ContratoRetificacaoItem.NaturezaItem == NaturezaItem.PrecoGlobal)
            {
                if (contratoRetificacaoItemMedicao.Valor == 0)
                {
                    messageQueue.Add(string.Format(Application.Resource.Sigim.ErrorMessages.CampoObrigatorio, "Valor medição atual"), TypeMessage.Error);
                    return false;
                }
                if (contratoRetificacaoItemMedicao.Valor > contratoRetificacaoItemMedicao.ValorPendente)
                {
                    string msg = string.Format(Resource.Contrato.ErrorMessages.ValorMaiorQue, "Valor medição atual", "Valor pendente");
                    messageQueue.Add(msg, TypeMessage.Error);
                    return false;
                }
            }
            else if (contratoRetificacaoItemMedicao.ContratoRetificacaoItem.NaturezaItem == NaturezaItem.PrecoUnitario)
            {
                if (contratoRetificacaoItemMedicao.Quantidade > contratoRetificacaoItemMedicao.QuantidadePendente)
                {
                    string msg = string.Format(Resource.Contrato.ErrorMessages.ValorMaiorQue, "Quantidade medição atual", "Quantidade pendente");
                    messageQueue.Add(msg, TypeMessage.Error);
                    return false;
                }

            }

            if (contratoRetificacaoItemMedicao.Desconto > 0) 
            {
                if (string.IsNullOrEmpty(contratoRetificacaoItemMedicao.MotivoDesconto))
                {
                    messageQueue.Add(string.Format(Application.Resource.Sigim.ErrorMessages.CampoObrigatorio, "Motivo desconto"), TypeMessage.Error);
                    return false;
                }
                if (contratoRetificacaoItemMedicao.Desconto > contratoRetificacaoItemMedicao.Valor)
                {
                    string msg = string.Format(Resource.Contrato.ErrorMessages.ValorMaiorQue, "Desconto", "Valor medição atual");
                    messageQueue.Add(msg, TypeMessage.Error);
                    return false;
                }
            }

            IBloqueioContabilAppService bloqueioContabilAppService = new BloqueioContabilAppService(bloqueioContabilRepository, messageQueue);
            Nullable<DateTime> dataBloqueio;

            if (!bloqueioContabilAppService.ValidaBloqueioContabil(contratoRetificacaoItemMedicao.Contrato.CodigoCentroCusto, 
                                                                   contratoRetificacaoItemMedicao.DataEmissao,
                                                                   out dataBloqueio))
            {
                string msg = string.Format(Resource.Sigim.ErrorMessages.BloqueioContabilEncontrado, dataBloqueio.Value.ToShortDateString(), contratoRetificacaoItemMedicao.Contrato.CodigoCentroCusto);
                messageQueue.Add(msg, TypeMessage.Error);
                return false;
            }


            if (parametros == null) return true;

            if (!parametros.DadosSped.HasValue) return true;

            if (parametros.DadosSped.Value)
            {
                if (string.IsNullOrEmpty(contratoRetificacaoItemMedicao.TipoCompraCodigo))
                {
                    messageQueue.Add(string.Format(Application.Resource.Sigim.ErrorMessages.CampoObrigatorio, "Tipo compra"), TypeMessage.Error);
                    return false;
                }

                if ((!contratoRetificacaoItemMedicao.CifFobId.HasValue) || (contratoRetificacaoItemMedicao.CifFobId.Value == 0))
                {
                    messageQueue.Add(string.Format(Application.Resource.Sigim.ErrorMessages.CampoObrigatorio, "CIF/FOB"), TypeMessage.Error);
                    return false;
                }

                if ((string.IsNullOrEmpty(contratoRetificacaoItemMedicao.NaturezaOperacaoCodigo)))
                {
                    messageQueue.Add(string.Format(Application.Resource.Sigim.ErrorMessages.CampoObrigatorio, "Natureza de operação"), TypeMessage.Error);
                    return false;
                }

                if ((!contratoRetificacaoItemMedicao.SerieNFId.HasValue) || (contratoRetificacaoItemMedicao.SerieNFId.Value == 0))
                {
                    messageQueue.Add(string.Format(Application.Resource.Sigim.ErrorMessages.CampoObrigatorio, "Série"), TypeMessage.Error);
                    return false;
                }

                if ((string.IsNullOrEmpty(contratoRetificacaoItemMedicao.CSTCodigo)))
                {
                    messageQueue.Add(string.Format(Application.Resource.Sigim.ErrorMessages.CampoObrigatorio, "CST"), TypeMessage.Error);
                    return false;
                }

                if ((string.IsNullOrEmpty(contratoRetificacaoItemMedicao.CodigoContribuicaoCodigo)))
                {
                    messageQueue.Add(string.Format(Application.Resource.Sigim.ErrorMessages.CampoObrigatorio, "Contribuição"), TypeMessage.Error);
                    return false;
                }

            }

            return true;
        }

        private bool ExisteNumeroDocumentoTituloAPagar(DateTime dataEmissao, DateTime dataVencimento, string numeroDocumento, int contratadoId)
        {
            ITituloPagarAppService tituloPagarAppService = new TituloPagarAppService(tituloPagarRepository, messageQueue);

            if (tituloPagarAppService.ExisteNumeroDocumento(dataEmissao,
                                                            dataVencimento,
                                                            numeroDocumento,
                                                            contratadoId))
            {
                return false;
            }

            return false;
        }

        #endregion
    }
}
