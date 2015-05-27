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

        #endregion

        #region Construtor

        public ContratoRetificacaoItemMedicaoAppService(IContratoRetificacaoItemMedicaoRepository contratoRetificacaoItemMedicaoRepository,
                                                        ITituloPagarRepository tituloPagarRepository,
                                                        IParametrosContratoRepository parametrosContratoRepository,
                                                        IBloqueioContabilRepository bloqueioContabilRepository,
                                                        ITituloPagarAppService tituloPagarAppService,
                                                        IParametrosContratoAppService parametrosContratoAppService,
                                                        IBloqueioContabilAppService bloqueioContabilAppService,
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
        }

        #endregion

        #region Métodos IContratoRetificacaoItemMedicaoAppService

        public void ObterQuantidadesEhValoresMedicao(int ContratoId,
                                                     int SequencialItem,
                                                     int SequencialCronograma,
                                                     ref decimal QuantidadeTotalMedido,
                                                     ref decimal ValorTotalMedido,
                                                     ref decimal QuantidadeTotalLiberado,
                                                     ref decimal ValorTotalLiberado)
        {

            List<ContratoRetificacaoItemMedicao> listaContratoRetificacaoItemMedicao = 
                    contratoRetificacaoItemMedicaoRepository.ListarPeloFiltro(  ( l => 
                                                                                    l.ContratoId == ContratoId
                                                                                 ), 
                                                                                l => l.ContratoRetificacaoItem,
                                                                                l => l.ContratoRetificacaoItemCronograma).Where(c =>
                                                                                    (c.SequencialItem == SequencialItem &&
                                                                                     c.SequencialCronograma == SequencialCronograma)
                                                                                ).ToList <ContratoRetificacaoItemMedicao>();

            //var queryMedido =
            //            from c in listaContratoRetificacaoItemMedicao
            //            where (
            //                    ((c.Situacao == SituacaoMedicao.AguardandoAprovacao) ||
            //                     (c.Situacao == SituacaoMedicao.AguardandoLiberacao))
            //                  )
            //            group c by c.ContratoRetificacaoItemId into g
            //            select new
            //            {
            //                ContratoRetificacaoItemId   = g.Key,
            //                QuantidadeTotalMedido       = g.Sum(i => i.Quantidade),
            //                ValorTotalMedido            = g.Sum(i => i.Valor)
            //            };

            var queryMedido =
                        from c in listaContratoRetificacaoItemMedicao
                        where (
                                ((c.Situacao == SituacaoMedicao.AguardandoAprovacao) ||
                                 (c.Situacao == SituacaoMedicao.AguardandoLiberacao) ||
                                 (c.Situacao == SituacaoMedicao.Liberado))
                              )
                        group c by c.Situacao into g
                        select g;

            QuantidadeTotalMedido = 0;
            ValorTotalMedido = 0;
            QuantidadeTotalLiberado = 0;
            ValorTotalLiberado = 0;

            foreach (var medicaoGrupo in queryMedido)
            {
                if ((medicaoGrupo.Key == SituacaoMedicao.AguardandoAprovacao) || (medicaoGrupo.Key == SituacaoMedicao.AguardandoLiberacao))
                {

                    foreach (var medicao in medicaoGrupo)
                    {
                        QuantidadeTotalMedido = QuantidadeTotalMedido + medicao.Quantidade;
                        ValorTotalMedido = ValorTotalMedido + medicao.Valor;
                    }
                }
                if (medicaoGrupo.Key == SituacaoMedicao.Liberado)
                {

                    foreach (var medicao in medicaoGrupo)
                    {
                        QuantidadeTotalLiberado = QuantidadeTotalLiberado + medicao.Quantidade;
                        ValorTotalLiberado = ValorTotalLiberado + medicao.Valor;
                    }
                }

            }

            //var queryLiberado = 
            //            from c in listaContratoRetificacaoItemMedicao
            //            where (
            //                    (c.Situacao == SituacaoMedicao.Liberado)
            //                  )
            //            group c by c.ContratoRetificacaoItemId into g
            //            select new
            //            {
            //                ContratoRetificacaoItemId   = g.Key,
            //                QuantidadeTotalLiberado     = g.Sum(i => i.Quantidade),
            //                ValorTotalLiberado          = g.Sum(i => i.Valor)
            //            };

            //foreach (var item in queryLiberado)
            //{
            //    QuantidadeTotalLiberado = item.QuantidadeTotalLiberado;
            //    ValorTotalLiberado = item.ValorTotalLiberado;
            //}
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

            bool novoRegistro = false;

            ContratoRetificacaoItemMedicao contratoRetificacaoItemMedicao = contratoRetificacaoItemMedicaoRepository.ObterPeloId(dto.Id);

            if (contratoRetificacaoItemMedicao == null)
            {
                novoRegistro = true;
                contratoRetificacaoItemMedicao = new ContratoRetificacaoItemMedicao();
                contratoRetificacaoItemMedicao.DataCadastro = DateTime.Now;
                contratoRetificacaoItemMedicao.UsuarioMedicao = AuthenticationService.GetUser().Login;
                contratoRetificacaoItemMedicao.Situacao = SituacaoMedicao.AguardandoAprovacao;
            }

            contratoRetificacaoItemMedicao.ContratoId = dto.ContratoId;
 
            if (!EhValido(dto))
            {
                return false;
            }

            MapearEntity(dto, contratoRetificacaoItemMedicao);


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
                    dto.Id = contratoRetificacaoItemMedicao.ContratoId;
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

        #endregion

        #region Métodos privados

        private bool EhValido(ContratoRetificacaoItemMedicaoDTO dto)
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
                if (dto.Valor > dto.ValorPendente)
                {
                    string msg = string.Format(Resource.Contrato.ErrorMessages.ValorMaiorQue, "Valor medição atual", "Valor pendente");
                    messageQueue.Add(msg, TypeMessage.Error);
                    return false;
                }
            }
            else if (dto.ContratoRetificacaoItem.NaturezaItem == NaturezaItem.PrecoUnitario)
            {
                if (dto.Quantidade > dto.QuantidadePendente)
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

        private static void MapearEntity(ContratoRetificacaoItemMedicaoDTO dto, ContratoRetificacaoItemMedicao contratoRetificacaoItemMedicao)
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

        private static decimal CalculaValorRetido(ContratoRetificacaoItemMedicaoDTO medicao)
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

        #endregion
    }
}
