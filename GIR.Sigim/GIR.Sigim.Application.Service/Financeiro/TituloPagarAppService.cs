using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Repository.Financeiro;
using GIR.Sigim.Domain.Entity.Financeiro;
using GIR.Sigim.Application.Constantes;
using GIR.Sigim.Application.DTO.Financeiro;
using GIR.Sigim.Application.Filtros.Financeiro;
using GIR.Sigim.Domain.Specification;
using GIR.Sigim.Application.Service.Admin;
using GIR.Sigim.Application.Adapter;
using GIR.Sigim.Domain.Specification.Financeiro;

namespace GIR.Sigim.Application.Service.Financeiro
{
    public class TituloPagarAppService : BaseAppService, ITituloPagarAppService
    {
        #region Declaração

        private ITituloPagarRepository tituloPagarRepository;
        private IUsuarioAppService usuarioAppService;

        #endregion

        #region Construtor

        public TituloPagarAppService(ITituloPagarRepository tituloPagarRepository, 
                                     IUsuarioAppService usuarioAppService,
                                     MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.tituloPagarRepository = tituloPagarRepository;
            this.usuarioAppService = usuarioAppService;
        }

        #endregion

        #region Métodos ITituloPagarAppService

        public bool ExisteNumeroDocumento(Nullable<DateTime> DataEmissao, Nullable<DateTime> DataVencimento, string NumeroDocumento, int? ClienteId)
        {
            bool existe = false;

            if (!string.IsNullOrEmpty(NumeroDocumento) && (ClienteId.HasValue) && (DataEmissao.HasValue))
            {
                List<TituloPagar> listaTituloPagar;
                string numeroNotaFiscal = RetiraZerosIniciaisNumeroDocumento(NumeroDocumento);

                listaTituloPagar =
                    tituloPagarRepository.ListarPeloFiltro(l => l.ClienteId == ClienteId &&
                                                            l.Documento.EndsWith(NumeroDocumento) &&
                                                            l.DataEmissaoDocumento.Year == DataEmissao.Value.Year &&
                                                            ((DataVencimento == null) || ((DataVencimento != null) && (l.DataVencimento == DataVencimento)))).ToList<TituloPagar>();
                if (listaTituloPagar.Count() > 0)
                {
                    string numeroDeZerosIniciais;

                    foreach (var item in listaTituloPagar)
                    {
                        if ((item.Situacao != SituacaoTituloPagar.Cancelado) && (item.TipoTitulo != TipoTitulo.Pai))
                        {
                            var quantidadeDeZerosIniciais = item.Documento.Length - numeroNotaFiscal.Length;
                            numeroDeZerosIniciais = item.Documento.Substring(0, quantidadeDeZerosIniciais);
                            if (string.IsNullOrEmpty(numeroDeZerosIniciais))
                            {
                                numeroDeZerosIniciais = "0";
                            }
                            int resultado;
                            if (int.TryParse(numeroDeZerosIniciais, out resultado))
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
            }

            return existe;
        }

        public bool EhPermitidoImprimirRelContasPagarTitulo()
        {
            return UsuarioLogado.IsInRole(Funcionalidade.RelatorioContasAPagarTitulosImprimir);
        }

        public List<RelContasPagarTitulosDTO> ListarPeloFiltroRelContasPagarTitulos(RelContasPagarTitulosFiltro filtro, int? usuarioId, out int totalRegistros)
        {

            bool situacaoPagamentoPendente = filtro.EhSituacaoAPagarProvisionado || filtro.EhSituacaoAPagarAguardandoLiberacao || filtro.EhSituacaoAPagarLiberado || filtro.EhSituacaoAPagarCancelado;
            bool situacaoPagamentoPago = filtro.EhSituacaoAPagarEmitido || filtro.EhSituacaoAPagarPago || filtro.EhSituacaoAPagarBaixado;

            totalRegistros = 0;


            List<TituloPagar> listaTitulosPagar = new List<TituloPagar>();

            if ((situacaoPagamentoPendente) || ((!situacaoPagamentoPendente) && (!situacaoPagamentoPago)))
            {
                var specification = (Specification<TituloPagar>)new TrueSpecification<TituloPagar>();

                specification &= MontarSpecificationSituacaoPendentesRelContasPagarTitulos(filtro, usuarioId);

                var listaTitulosPagarPendentes =
                 tituloPagarRepository.ListarPeloFiltroComPaginacao(specification,
                                                                    filtro.PaginationParameters.PageIndex,
                                                                    filtro.PaginationParameters.PageSize,
                                                                    filtro.PaginationParameters.OrderBy,
                                                                    filtro.PaginationParameters.Ascending,
                                                                    out totalRegistros,
                                                                    l => l.Cliente.PessoaFisica,
                                                                    l => l.Cliente.PessoaJuridica,
                                                                    l => l.Movimento,
                                                                    l => l.ContaCorrente,
                                                                    l => l.TipoCompromisso,
                                                                    l => l.ListaApropriacao.Select(a => a.CentroCusto),
                                                                    l => l.ListaApropriacao.Select(a => a.Classe),
                                                                    l => l.MotivoCancelamento).To<List<TituloPagar>>();

                listaTitulosPagar.AddRange(listaTitulosPagarPendentes);

            }

            if ((situacaoPagamentoPago) || ((!situacaoPagamentoPendente) && (!situacaoPagamentoPago)))
            {
                var specification = (Specification<TituloPagar>)new TrueSpecification<TituloPagar>();

                specification &= MontarSpecificationSituacaoPagosRelContasPagarTitulos(filtro, usuarioId);

                var listaTitulosPagarPagos =
                 tituloPagarRepository.ListarPeloFiltroComPaginacao(specification,
                                                                    filtro.PaginationParameters.PageIndex,
                                                                    filtro.PaginationParameters.PageSize,
                                                                    filtro.PaginationParameters.OrderBy,
                                                                    filtro.PaginationParameters.Ascending,
                                                                    out totalRegistros,
                                                                    l => l.Cliente.PessoaFisica,
                                                                    l => l.Cliente.PessoaJuridica,
                                                                    l => l.Movimento,
                                                                    l => l.ContaCorrente,
                                                                    l => l.TipoCompromisso,
                                                                    l => l.ListaApropriacao.Select(a => a.CentroCusto),
                                                                    l => l.ListaApropriacao.Select(a => a.Classe),
                                                                    l => l.MotivoCancelamento).To<List<TituloPagar>>();

                listaTitulosPagar.AddRange(listaTitulosPagarPagos);
            }


            List<RelContasPagarTitulosDTO> listaRelContasPagarTitulos = new List<RelContasPagarTitulosDTO>();

            foreach (var item in listaTitulosPagar)
            {
                RelContasPagarTitulosDTO relat = new RelContasPagarTitulosDTO();

                relat.TituloPagar = item.To<TituloPagarDTO>();

                listaRelContasPagarTitulos.Add(relat);
            }

            return listaRelContasPagarTitulos;

        }

        #endregion

        #region "Métodos privados"

        private Specification<TituloPagar> MontarSpecificationSituacaoPendentesRelContasPagarTitulos(RelContasPagarTitulosFiltro filtro, int? idUsuario)
        {
            bool situacaoPagamentoPendente = filtro.EhSituacaoAPagarProvisionado || filtro.EhSituacaoAPagarAguardandoLiberacao || filtro.EhSituacaoAPagarLiberado || filtro.EhSituacaoAPagarCancelado;
            bool situacaoPagamentoPago = filtro.EhSituacaoAPagarEmitido || filtro.EhSituacaoAPagarPago || filtro.EhSituacaoAPagarBaixado;

            var specification = (Specification<TituloPagar>)new TrueSpecification<TituloPagar>();

            specification &= TituloPagarSpecification.DataPeriodoMaiorOuIgualSituacaoPendentesRelContasPagarTitulos(filtro.DataInicial);
            specification &= TituloPagarSpecification.DataPeriodoMenorOuIgualSituacaoPendentesRelContasPagarTitulos(filtro.DataFinal);

            if ((!situacaoPagamentoPendente) && (!situacaoPagamentoPago))
            {
                specification &= (TituloPagarSpecification.EhSituacaoAPagarProvisionado(true)) ||
                    (TituloPagarSpecification.EhSituacaoAPagarAguardandoLiberacao(true)) ||
                    (TituloPagarSpecification.EhSituacaoAPagarLiberado(true));
            }
            else
            {
                if (filtro.EhSituacaoAPagarProvisionado || filtro.EhSituacaoAPagarAguardandoLiberacao || filtro.EhSituacaoAPagarLiberado || filtro.EhSituacaoAPagarCancelado)
                {
                    specification &= (TituloPagarSpecification.EhSituacaoAPagarProvisionado(filtro.EhSituacaoAPagarProvisionado)) ||
                        (TituloPagarSpecification.EhSituacaoAPagarAguardandoLiberacao(filtro.EhSituacaoAPagarAguardandoLiberacao)) ||
                        (TituloPagarSpecification.EhSituacaoAPagarLiberado(filtro.EhSituacaoAPagarLiberado)) ||
                        (TituloPagarSpecification.EhSituacaoAPagarCancelado(filtro.EhSituacaoAPagarCancelado));
                }
            }

            specification &= TituloPagarSpecification.PertenceAoCentroCustoIniciadoPor(filtro.CentroCusto.Codigo);

            specification &= TituloPagarSpecification.PertenceAClasseIniciadaPor(filtro.Classe.Codigo);

            specification &= TituloPagarSpecification.MatchingClienteId(filtro.ClienteFornecedor.Id);

            specification &= TituloPagarSpecification.IdentificacaoContem(filtro.Identificacao);

            string documento = !string.IsNullOrEmpty(filtro.Documento) ? RetiraZerosIniciaisNumeroDocumento(filtro.Documento) : null;

            specification &= TituloPagarSpecification.DocumentoContem(documento);

            specification &= TituloPagarSpecification.MatchingTipoCompromissoId(filtro.TipoCompromissoId);

            specification &= TituloPagarSpecification.PertenceAhFormaPagamento(filtro.FormaPagamentoCodigo);

            specification &= TituloPagarSpecification.DocumentoPagamentoContem(filtro.DocumentoPagamento);

            return specification;
        }

        private Specification<TituloPagar> MontarSpecificationSituacaoPagosRelContasPagarTitulos(RelContasPagarTitulosFiltro filtro, int? idUsuario)
        {
            bool situacaoPagamentoPendente = filtro.EhSituacaoAPagarProvisionado || filtro.EhSituacaoAPagarAguardandoLiberacao || filtro.EhSituacaoAPagarLiberado || filtro.EhSituacaoAPagarCancelado;
            bool situacaoPagamentoPago = filtro.EhSituacaoAPagarEmitido || filtro.EhSituacaoAPagarPago || filtro.EhSituacaoAPagarBaixado;

            string tipoPesquisa = filtro.EhPorCompetencia ? "V" : "";

            var specification = (Specification<TituloPagar>)new TrueSpecification<TituloPagar>();

            SituacaoTituloPagar situacaoTitulo = SituacaoTituloPagar.Emitido;

            if ((!situacaoPagamentoPendente) && (!situacaoPagamentoPago))
            {
                specification &= (TituloPagarSpecification.EhSituacaoAPagarEmitido(true)) ||
                    (TituloPagarSpecification.EhSituacaoAPagarPago(true)) ||
                    (TituloPagarSpecification.EhSituacaoAPagarBaixado(true));
            }
            else
            {
                if (filtro.EhSituacaoAPagarPago && (!filtro.EhSituacaoAPagarEmitido && !filtro.EhSituacaoAPagarBaixado))
                {
                    situacaoTitulo = SituacaoTituloPagar.Pago;
                }
                else 
                {
                    if (filtro.EhSituacaoAPagarBaixado && (!filtro.EhSituacaoAPagarEmitido && !filtro.EhSituacaoAPagarPago))
                    {
                        situacaoTitulo = SituacaoTituloPagar.Baixado;
                    }
                }

                if (filtro.EhSituacaoAPagarEmitido || filtro.EhSituacaoAPagarPago || filtro.EhSituacaoAPagarBaixado)
                {
                    specification &= (TituloPagarSpecification.EhSituacaoAPagarEmitido(filtro.EhSituacaoAPagarEmitido)) ||
                        (TituloPagarSpecification.EhSituacaoAPagarPago(filtro.EhSituacaoAPagarPago)) ||
                        (TituloPagarSpecification.EhSituacaoAPagarBaixado(filtro.EhSituacaoAPagarBaixado));
                }
            }

            specification &= TituloPagarSpecification.DataPeriodoMaiorOuIgualSituacaoPagosRelContasPagarTitulos(situacaoTitulo,tipoPesquisa,filtro.DataInicial);
            specification &= TituloPagarSpecification.DataPeriodoMenorOuIgualSituacaoPagosRelContasPagarTitulos(situacaoTitulo,tipoPesquisa,filtro.DataFinal);

            specification &= TituloPagarSpecification.PertenceAoCentroCustoIniciadoPor(filtro.CentroCusto.Codigo);

            specification &= TituloPagarSpecification.PertenceAClasseIniciadaPor(filtro.Classe.Codigo);

            specification &= TituloPagarSpecification.MatchingClienteId(filtro.ClienteFornecedor.Id);

            specification &= TituloPagarSpecification.IdentificacaoContem(filtro.Identificacao);

            string documento = !string.IsNullOrEmpty(filtro.Documento) ? RetiraZerosIniciaisNumeroDocumento(filtro.Documento) : null;

            specification &= TituloPagarSpecification.DocumentoContem(documento);

            specification &= TituloPagarSpecification.MatchingTipoCompromissoId(filtro.TipoCompromissoId);

            specification &= TituloPagarSpecification.PertenceAhFormaPagamento(filtro.FormaPagamentoCodigo);

            specification &= TituloPagarSpecification.DocumentoPagamentoContem(filtro.DocumentoPagamento);

            return specification;
        }


        #endregion
    }
}
