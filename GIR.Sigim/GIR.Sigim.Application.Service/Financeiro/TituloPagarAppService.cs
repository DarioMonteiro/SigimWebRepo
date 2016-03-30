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
using GIR.Sigim.Application.Enums;
using GIR.Sigim.Domain.Entity.Sigim;
using GIR.Sigim.Application.Service.Sigim;

namespace GIR.Sigim.Application.Service.Financeiro
{
    public class TituloPagarAppService : BaseAppService, ITituloPagarAppService
    {
        #region Declaração

        private ITituloPagarRepository tituloPagarRepository;
        private IUsuarioAppService usuarioAppService;
        private IModuloSigimAppService moduloSigimAppService;

        #endregion

        #region Construtor

        public TituloPagarAppService(ITituloPagarRepository tituloPagarRepository, 
                                     IUsuarioAppService usuarioAppService,
                                     IModuloSigimAppService moduloSigimAppService,
                                     MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.tituloPagarRepository = tituloPagarRepository;
            this.usuarioAppService = usuarioAppService;
            this.moduloSigimAppService = moduloSigimAppService;
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

        public List<RelContasPagarTitulosDTO> ListarPeloFiltroRelContasPagarTitulos(RelContasPagarTitulosFiltro filtro, 
                                                                                    int? usuarioId, 
                                                                                    out int totalRegistros, 
                                                                                    out decimal totalValorTitulo, 
                                                                                    out decimal totalValorLiquido,
                                                                                    out decimal totalValorApropriado)
        {
            bool situacaoPagamentoPendente = filtro.EhSituacaoAPagarProvisionado || filtro.EhSituacaoAPagarAguardandoLiberacao || filtro.EhSituacaoAPagarLiberado || filtro.EhSituacaoAPagarCancelado;
            bool situacaoPagamentoPago = filtro.EhSituacaoAPagarEmitido || filtro.EhSituacaoAPagarPago || filtro.EhSituacaoAPagarBaixado;

            List<TituloPagar> listaTitulosPagar = new List<TituloPagar>();

            var specificationTeste1 = (Specification<TituloPagar>)new TrueSpecification<TituloPagar>();

            if ((situacaoPagamentoPendente) || ((!situacaoPagamentoPendente) && (!situacaoPagamentoPago)))
            {
                specificationTeste1 &= MontarSpecificationSituacaoPendentesRelContasPagarTitulos(filtro, usuarioId);
            }

            var specificationTeste2 = (Specification<TituloPagar>)new TrueSpecification<TituloPagar>();

            if ((situacaoPagamentoPago) || ((!situacaoPagamentoPendente) && (!situacaoPagamentoPago)))
            {
                specificationTeste2 &= MontarSpecificationSituacaoPagosRelContasPagarTitulos(filtro, usuarioId);
            }

            listaTitulosPagar =
             tituloPagarRepository.ListarPeloFiltroComUnion(specificationTeste1,
                                                            specificationTeste2,
                                                            l => l.Cliente.PessoaFisica,
                                                            l => l.Cliente.PessoaJuridica,
                                                            l => l.Movimento.ContaCorrente.Agencia,
                                                            l => l.Movimento.Caixa,
                                                            l => l.TipoCompromisso,
                                                            l => l.TipoDocumento,
                                                            l => l.ListaApropriacao.Select(a => a.CentroCusto),
                                                            l => l.ListaApropriacao.Select(a => a.Classe),
                                                            l => l.MotivoCancelamento).To<List<TituloPagar>>();

            var listaRelContasPagarTitulos = PopulaListaRelContasPagarTitulosDTO(filtro, 
                                                                                 listaTitulosPagar,
                                                                                 out totalRegistros,
                                                                                 out totalValorTitulo, 
                                                                                 out totalValorLiquido,
                                                                                 out totalValorApropriado);

            return listaRelContasPagarTitulos;

        }

        #endregion

        #region "Métodos privados"

        private List<RelContasPagarTitulosDTO> OrdenaListaRelContasPagarTitulosDTO(RelContasPagarTitulosFiltro filtro, List<RelContasPagarTitulosDTO> listaRelContasPagarTitulos)
        {
            int pageCount = filtro.PaginationParameters.PageSize;
            int pageIndex = filtro.PaginationParameters.PageIndex;

            switch (filtro.PaginationParameters.OrderBy)
            {
                case "tituloId":
                    if (filtro.PaginationParameters.Ascending) { listaRelContasPagarTitulos = listaRelContasPagarTitulos.OrderBy(l => l.TituloId).ToList<RelContasPagarTitulosDTO>(); }
                    if (!filtro.PaginationParameters.Ascending) { listaRelContasPagarTitulos = listaRelContasPagarTitulos.OrderByDescending(l => l.TituloId).ToList<RelContasPagarTitulosDTO>(); }
                    break;
                case "dataVencimento":
                    if (filtro.PaginationParameters.Ascending) { listaRelContasPagarTitulos = listaRelContasPagarTitulos.OrderBy(l => l.DataVencimento).ToList<RelContasPagarTitulosDTO>(); }
                    if (!filtro.PaginationParameters.Ascending) { listaRelContasPagarTitulos = listaRelContasPagarTitulos.OrderByDescending(l => l.DataVencimento).ToList<RelContasPagarTitulosDTO>(); }
                    break;
                case "dataEmissaoDocumento":
                    if (filtro.PaginationParameters.Ascending) { listaRelContasPagarTitulos = listaRelContasPagarTitulos.OrderBy(l => l.DataEmissaoDocumento).ToList<RelContasPagarTitulosDTO>(); }
                    if (!filtro.PaginationParameters.Ascending) { listaRelContasPagarTitulos = listaRelContasPagarTitulos.OrderByDescending(l => l.DataEmissaoDocumento).ToList<RelContasPagarTitulosDTO>(); }
                    break;
                case "documentoCompleto":
                    if (filtro.PaginationParameters.Ascending) { listaRelContasPagarTitulos = listaRelContasPagarTitulos.OrderBy(l => l.DocumentoCompleto).ToList<RelContasPagarTitulosDTO>(); }
                    if (!filtro.PaginationParameters.Ascending) { listaRelContasPagarTitulos = listaRelContasPagarTitulos.OrderByDescending(l => l.DocumentoCompleto).ToList<RelContasPagarTitulosDTO>(); }
                    break;
                case "valorTitulo":
                    if (filtro.PaginationParameters.Ascending) { listaRelContasPagarTitulos = listaRelContasPagarTitulos.OrderBy(l => l.ValorTitulo).ToList<RelContasPagarTitulosDTO>(); }
                    if (!filtro.PaginationParameters.Ascending) { listaRelContasPagarTitulos = listaRelContasPagarTitulos.OrderByDescending(l => l.ValorTitulo).ToList<RelContasPagarTitulosDTO>(); }
                    break;
                case "valorLiquido":
                    if (filtro.PaginationParameters.Ascending) { listaRelContasPagarTitulos = listaRelContasPagarTitulos.OrderBy(l => l.ValorLiquido).ToList<RelContasPagarTitulosDTO>(); }
                    if (!filtro.PaginationParameters.Ascending) { listaRelContasPagarTitulos = listaRelContasPagarTitulos.OrderByDescending(l => l.ValorLiquido).ToList<RelContasPagarTitulosDTO>(); }
                    break;
                case "nomeCliente":
                    if (filtro.PaginationParameters.Ascending) { listaRelContasPagarTitulos = listaRelContasPagarTitulos.OrderBy(l => l.NomeCliente).ToList<RelContasPagarTitulosDTO>(); }
                    if (!filtro.PaginationParameters.Ascending) { listaRelContasPagarTitulos = listaRelContasPagarTitulos.OrderByDescending(l => l.NomeCliente).ToList<RelContasPagarTitulosDTO>(); }
                    break;
                case "identificacao":
                    if (filtro.PaginationParameters.Ascending) { listaRelContasPagarTitulos = listaRelContasPagarTitulos.OrderBy(l => l.Identificacao).ToList<RelContasPagarTitulosDTO>(); }
                    if (!filtro.PaginationParameters.Ascending) { listaRelContasPagarTitulos = listaRelContasPagarTitulos.OrderByDescending(l => l.Identificacao).ToList<RelContasPagarTitulosDTO>(); }
                    break;
                case "descricaoFormaPagamento":
                    if (filtro.PaginationParameters.Ascending) { listaRelContasPagarTitulos = listaRelContasPagarTitulos.OrderBy(l => l.FormaPagamentoDescricao).ToList<RelContasPagarTitulosDTO>(); }
                    if (!filtro.PaginationParameters.Ascending) { listaRelContasPagarTitulos = listaRelContasPagarTitulos.OrderByDescending(l => l.FormaPagamentoDescricao).ToList<RelContasPagarTitulosDTO>(); }
                    break;
                case "documentoPagamento":
                    if (filtro.PaginationParameters.Ascending) { listaRelContasPagarTitulos = listaRelContasPagarTitulos.OrderBy(l => l.DocumentoPagamento).ToList<RelContasPagarTitulosDTO>(); }
                    if (!filtro.PaginationParameters.Ascending) { listaRelContasPagarTitulos = listaRelContasPagarTitulos.OrderByDescending(l => l.DocumentoPagamento).ToList<RelContasPagarTitulosDTO>(); }
                    break;
                case "agenciaConta":
                    if (filtro.PaginationParameters.Ascending) { listaRelContasPagarTitulos = listaRelContasPagarTitulos.OrderBy(l => l.AgenciaContaCorrente).ToList<RelContasPagarTitulosDTO>(); }
                    if (!filtro.PaginationParameters.Ascending) { listaRelContasPagarTitulos = listaRelContasPagarTitulos.OrderByDescending(l => l.AgenciaContaCorrente).ToList<RelContasPagarTitulosDTO>(); }
                    break;
                case "descricaoTipoCompromisso":
                    if (filtro.PaginationParameters.Ascending) { listaRelContasPagarTitulos = listaRelContasPagarTitulos.OrderBy(l => l.TipoCompromissoDescricao).ToList<RelContasPagarTitulosDTO>(); }
                    if (!filtro.PaginationParameters.Ascending) { listaRelContasPagarTitulos = listaRelContasPagarTitulos.OrderByDescending(l => l.TipoCompromissoDescricao).ToList<RelContasPagarTitulosDTO>(); }
                    break;
                case "descricaoSituacao":
                    if (filtro.PaginationParameters.Ascending) { listaRelContasPagarTitulos = listaRelContasPagarTitulos.OrderBy(l => l.SituacaoTituloDescricao).ToList<RelContasPagarTitulosDTO>(); }
                    if (!filtro.PaginationParameters.Ascending) { listaRelContasPagarTitulos = listaRelContasPagarTitulos.OrderByDescending(l => l.SituacaoTituloDescricao).ToList<RelContasPagarTitulosDTO>(); }
                    break;
                case "dataSelecao":
                    if (filtro.PaginationParameters.Ascending) { listaRelContasPagarTitulos = listaRelContasPagarTitulos.OrderBy(l => l.DataSelecao).ToList<RelContasPagarTitulosDTO>(); }
                    if (!filtro.PaginationParameters.Ascending) { listaRelContasPagarTitulos = listaRelContasPagarTitulos.OrderByDescending(l => l.DataSelecao).ToList<RelContasPagarTitulosDTO>(); }
                    break;
                case "dataEmissao":
                    if (filtro.PaginationParameters.Ascending) { listaRelContasPagarTitulos = listaRelContasPagarTitulos.OrderBy(l => l.DataEmissao).ToList<RelContasPagarTitulosDTO>(); }
                    if (!filtro.PaginationParameters.Ascending) { listaRelContasPagarTitulos = listaRelContasPagarTitulos.OrderByDescending(l => l.DataEmissao).ToList<RelContasPagarTitulosDTO>(); }
                    break;
                case "dataPagamento":
                    if (filtro.PaginationParameters.Ascending) { listaRelContasPagarTitulos = listaRelContasPagarTitulos.OrderBy(l => l.DataPagamento).ToList<RelContasPagarTitulosDTO>(); }
                    if (!filtro.PaginationParameters.Ascending) { listaRelContasPagarTitulos = listaRelContasPagarTitulos.OrderByDescending(l => l.DataPagamento).ToList<RelContasPagarTitulosDTO>(); }
                    break;
                case "dataBaixa":
                    if (filtro.PaginationParameters.Ascending) { listaRelContasPagarTitulos = listaRelContasPagarTitulos.OrderBy(l => l.DataBaixa).ToList<RelContasPagarTitulosDTO>(); }
                    if (!filtro.PaginationParameters.Ascending) { listaRelContasPagarTitulos = listaRelContasPagarTitulos.OrderByDescending(l => l.DataBaixa).ToList<RelContasPagarTitulosDTO>(); }
                    break;
                case "cpfCnpj":
                    if (filtro.PaginationParameters.Ascending) { listaRelContasPagarTitulos = listaRelContasPagarTitulos.OrderBy(l => l.CPFCNPJ).ToList<RelContasPagarTitulosDTO>(); }
                    if (!filtro.PaginationParameters.Ascending) { listaRelContasPagarTitulos = listaRelContasPagarTitulos.OrderByDescending(l => l.CPFCNPJ).ToList<RelContasPagarTitulosDTO>(); }
                    break;
                case "operadorCadastro":
                    if (filtro.PaginationParameters.Ascending) { listaRelContasPagarTitulos = listaRelContasPagarTitulos.OrderBy(l => l.LoginUsuarioCadastro).ToList<RelContasPagarTitulosDTO>(); }
                    if (!filtro.PaginationParameters.Ascending) { listaRelContasPagarTitulos = listaRelContasPagarTitulos.OrderByDescending(l => l.LoginUsuarioCadastro).ToList<RelContasPagarTitulosDTO>(); }
                    break;
                case "valorApropriacao":
                    if (filtro.PaginationParameters.Ascending) { listaRelContasPagarTitulos = listaRelContasPagarTitulos.OrderBy(l => l.ValorApropriado).ToList<RelContasPagarTitulosDTO>(); }
                    if (!filtro.PaginationParameters.Ascending) { listaRelContasPagarTitulos = listaRelContasPagarTitulos.OrderByDescending(l => l.ValorApropriado).ToList<RelContasPagarTitulosDTO>(); }
                    break;
                case "classe":
                    if (filtro.PaginationParameters.Ascending) { listaRelContasPagarTitulos = listaRelContasPagarTitulos.OrderBy(l => l.CodigoDescricaoClasse).ToList<RelContasPagarTitulosDTO>(); }
                    if (!filtro.PaginationParameters.Ascending) { listaRelContasPagarTitulos = listaRelContasPagarTitulos.OrderByDescending(l => l.CodigoDescricaoClasse).ToList<RelContasPagarTitulosDTO>(); }
                    break;
                case "centroCusto":
                    if (filtro.PaginationParameters.Ascending) { listaRelContasPagarTitulos = listaRelContasPagarTitulos.OrderBy(l => l.CodigoDescricaoCentroCusto).ToList<RelContasPagarTitulosDTO>(); }
                    if (!filtro.PaginationParameters.Ascending) { listaRelContasPagarTitulos = listaRelContasPagarTitulos.OrderByDescending(l => l.CodigoDescricaoCentroCusto).ToList<RelContasPagarTitulosDTO>(); }
                    break;
                default:
                    if (filtro.PaginationParameters.Ascending) { listaRelContasPagarTitulos = listaRelContasPagarTitulos.OrderBy(l => l.TituloId).ToList<RelContasPagarTitulosDTO>(); }
                    if (!filtro.PaginationParameters.Ascending) { listaRelContasPagarTitulos = listaRelContasPagarTitulos.OrderByDescending(l => l.TituloId).ToList<RelContasPagarTitulosDTO>(); }
                    break;
            }

            return listaRelContasPagarTitulos;
        }

        private List<RelContasPagarTitulosDTO> PopulaListaRelContasPagarTitulosDTO(RelContasPagarTitulosFiltro filtro, List<TituloPagar> listaTitulosPagar, out int totalRegistros,out decimal totalizadoValorTitulo, out decimal totalizadoValorLiquido, out decimal totalizadoValorApropriado)
        {
            int pageCount = filtro.PaginationParameters.PageSize;
            int pageIndex = filtro.PaginationParameters.PageIndex;

            bool situacaoPagamentoPendente = filtro.EhSituacaoAPagarProvisionado || filtro.EhSituacaoAPagarAguardandoLiberacao || filtro.EhSituacaoAPagarLiberado || filtro.EhSituacaoAPagarCancelado;
            bool situacaoPagamentoPago = filtro.EhSituacaoAPagarEmitido || filtro.EhSituacaoAPagarPago || filtro.EhSituacaoAPagarBaixado;

            string flagDataSituacao = "";
            if ((filtro.EhSituacaoAPagarEmitido) || ((!situacaoPagamentoPendente) && (!situacaoPagamentoPago)))
            {
                flagDataSituacao = "Emissao";
            }
            if (!filtro.EhSituacaoAPagarEmitido && filtro.EhSituacaoAPagarPago)
            {
                flagDataSituacao = "Pagamento";
            }
            if (!filtro.EhSituacaoAPagarEmitido && !filtro.EhSituacaoAPagarPago && filtro.EhSituacaoAPagarBaixado)
            {
                flagDataSituacao = "Baixa";
            }

            List<RelContasPagarTitulosDTO> listaRelContasPagarTitulos = new List<RelContasPagarTitulosDTO>();

            foreach (var tituloPagar in listaTitulosPagar)
            {
                RelContasPagarTitulosDTO relat = new RelContasPagarTitulosDTO();

                situacaoPagamentoPendente = false;
                situacaoPagamentoPago = false;

                relat.TituloId = tituloPagar.Id.Value;
                relat.DataVencimento = tituloPagar.DataVencimento;
                relat.DataEmissaoDocumento = tituloPagar.DataEmissaoDocumento;
                relat.DocumentoCompleto = "";
                if (tituloPagar.TipoDocumentoId.HasValue)
                {
                    relat.DocumentoCompleto = tituloPagar.TipoDocumento.Sigla + " " + tituloPagar.Documento;
                }
                relat.ValorTitulo = tituloPagar.ValorTitulo;

                relat.NomeCliente = tituloPagar.Cliente.Nome;
                relat.Identificacao = tituloPagar.Identificacao;
                relat.FormaPagamentoDescricao = "";
                if (tituloPagar.FormaPagamento.HasValue)
                {
                    switch ((FormaPagamento)tituloPagar.FormaPagamento.Value)
                    {
                        case FormaPagamento.Automatico:
                            relat.FormaPagamentoDescricao = FormaPagamento.Automatico.ObterDescricao();
                            break;
                        case FormaPagamento.Bordero:
                            relat.FormaPagamentoDescricao = FormaPagamento.Bordero.ObterDescricao();
                            break;
                        case FormaPagamento.BorderoEletrônico:
                            relat.FormaPagamentoDescricao = FormaPagamento.BorderoEletrônico.ObterDescricao();
                            break;
                        case FormaPagamento.Cheque:
                            relat.FormaPagamentoDescricao = FormaPagamento.Cheque.ObterDescricao();
                            break;
                        case FormaPagamento.Dinheiro:
                            relat.FormaPagamentoDescricao = FormaPagamento.Dinheiro.ObterDescricao();
                            break;
                        case FormaPagamento.OperacaoBancaria:
                            relat.FormaPagamentoDescricao = FormaPagamento.OperacaoBancaria.ObterDescricao();
                            break;
                        default:
                            relat.FormaPagamentoDescricao = "";
                            break;
                    }
                }
                relat.DocumentoPagamento = "";
                if (tituloPagar.MovimentoId.HasValue)
                {
                    relat.DocumentoPagamento = tituloPagar.Movimento.Documento;
                }
                relat.AgenciaContaCorrente = "";
                if ((tituloPagar.Movimento != null) && (tituloPagar.Movimento.ContaCorrente != null))
                {
                    ContaCorrente contaCorrente = tituloPagar.Movimento.ContaCorrente;
                    relat.AgenciaContaCorrente =
                        contaCorrente.Agencia.AgenciaCodigo + "-" + contaCorrente.Agencia.DVAgencia + " / " + contaCorrente.ContaCodigo + "-" + contaCorrente.DVConta;
                }
                relat.TipoCompromissoDescricao = "";
                if (tituloPagar.TipoCompromisso != null)
                {
                    relat.TipoCompromissoDescricao = tituloPagar.TipoCompromisso.Descricao;
                }
                relat.SituacaoTituloDescricao = tituloPagar.Situacao.ObterDescricao();
                relat.DataSelecao = null;

                switch (tituloPagar.Situacao)
                {
                    case SituacaoTituloPagar.AguardandoLiberacao:
                        relat.DataSelecao = tituloPagar.DataVencimento;
                        situacaoPagamentoPendente = true;
                        break;
                    case SituacaoTituloPagar.Provisionado:
                        relat.DataSelecao = tituloPagar.DataVencimento;
                        situacaoPagamentoPendente = true;
                        break;
                    case SituacaoTituloPagar.Liberado:
                        relat.DataSelecao = tituloPagar.DataVencimento;
                        situacaoPagamentoPendente = true;
                        break;
                    case SituacaoTituloPagar.Cancelado:
                        relat.DataSelecao = tituloPagar.DataVencimento;
                        situacaoPagamentoPendente = true;
                        break;
                    case SituacaoTituloPagar.Emitido:
                        if (flagDataSituacao == "Emissao") relat.DataSelecao = tituloPagar.DataEmissao;
                        if (flagDataSituacao == "Pagamento") relat.DataSelecao = tituloPagar.DataPagamento;
                        if (flagDataSituacao == "Baixa") relat.DataSelecao = tituloPagar.DataBaixa;
                        situacaoPagamentoPago = true;
                        break;
                    case SituacaoTituloPagar.Pago:
                        if (flagDataSituacao == "Emissao") relat.DataSelecao = tituloPagar.DataEmissao;
                        if (flagDataSituacao == "Pagamento") relat.DataSelecao = tituloPagar.DataPagamento;
                        if (flagDataSituacao == "Baixa") relat.DataSelecao = tituloPagar.DataBaixa;
                        situacaoPagamentoPago = true;
                        break;
                    case SituacaoTituloPagar.Baixado:
                        if (flagDataSituacao == "Emissao") relat.DataSelecao = tituloPagar.DataEmissao;
                        if (flagDataSituacao == "Pagamento") relat.DataSelecao = tituloPagar.DataPagamento;
                        if (flagDataSituacao == "Baixa") relat.DataSelecao = tituloPagar.DataBaixa;
                        situacaoPagamentoPago = true;
                        break;
                    default:
                        relat.FormaPagamentoDescricao = "";
                        break;
                }

                if (situacaoPagamentoPendente)
                {
                    relat.ValorLiquido = CalculaValorLiquido(tituloPagar,DateTime.Now.Date);
                }
                else
                {
                    decimal valorPago = tituloPagar.ValorPago.HasValue ? tituloPagar.ValorPago.Value : 0;
                    relat.ValorLiquido = valorPago;
                }

                relat.DataEmissao = null;
                if (tituloPagar.DataEmissao.HasValue)
                {
                    relat.DataEmissao = tituloPagar.DataEmissao.Value.Date;
                }
                relat.DataPagamento = null;
                if (tituloPagar.DataPagamento.HasValue)
                {
                    relat.DataPagamento = tituloPagar.DataPagamento.Value.Date;
                }
                relat.DataBaixa = null;
                if (tituloPagar.DataBaixa.HasValue)
                {
                    relat.DataBaixa = tituloPagar.DataBaixa.Value.Date;
                }
                relat.MotivoCancelamentoDescricao = "";
                if (tituloPagar.MotivoCancelamento != null)
                {
                    relat.MotivoCancelamentoDescricao = tituloPagar.MotivoCancelamento.Descricao;
                }
                if (tituloPagar.SistemaOrigem != "FINAN")
                {
                    relat.MotivoCancelamentoDescricao = tituloPagar.MotivoCancelamentoInterface;
                }
                relat.CPFCNPJ = "";
                if (tituloPagar.Cliente != null)
                {
                    if (tituloPagar.Cliente.TipoPessoa == "F")
                    {
                        relat.CPFCNPJ = tituloPagar.Cliente.PessoaFisica.Cpf;
                    }
                    else
                    {
                        if (tituloPagar.Cliente.TipoPessoa == "J")
                        {
                            relat.CPFCNPJ = tituloPagar.Cliente.PessoaJuridica.Cnpj;
                        }
                    }
                }
                relat.LoginUsuarioCadastro = tituloPagar.LoginUsuarioCadastro;
                relat.DataCadastro = null;
                if (tituloPagar.DataCadastro.HasValue)
                {
                    relat.DataCadastro = tituloPagar.DataCadastro.Value.Date;
                }

                if (!filtro.EhSemApropriacao)
                {
                    if (tituloPagar.ListaApropriacao.Count > 0)
                    {
                        decimal totalValorApropriado = 0;
                        foreach (var apropriacao in tituloPagar.ListaApropriacao)
                        {
                            if (!string.IsNullOrEmpty(filtro.Classe.Codigo))
                            {
                                if (filtro.Classe.Codigo != apropriacao.Classe.Codigo)
                                {
                                    continue;
                                }
                            }
                            if (!string.IsNullOrEmpty(filtro.CentroCusto.Codigo))
                            {
                                if (filtro.CentroCusto.Codigo != apropriacao.CentroCusto.Codigo)
                                {
                                    continue;
                                }
                            }

                            if ((filtro.EhTotalizadoPor.HasValue) && (filtro.EhTotalizadoPor.Value == 2))
                            {
                                totalValorApropriado = totalValorApropriado + apropriacao.Valor;
                            }
                            else
                            {
                                relat.ValorApropriado = apropriacao.Valor;
                                relat.CodigoDescricaoClasse = apropriacao.Classe.Codigo + " - " + apropriacao.Classe.Descricao;
                                relat.CodigoDescricaoCentroCusto = apropriacao.CentroCusto.Codigo + " - " + apropriacao.CentroCusto.Descricao;

                                listaRelContasPagarTitulos.Add(relat);
                            }
                        }
                        if ((filtro.EhTotalizadoPor.HasValue) && (filtro.EhTotalizadoPor.Value == 2))
                        {
                            relat.ValorApropriado = totalValorApropriado;
                            listaRelContasPagarTitulos.Add(relat);
                        }

                    }
                    else
                    {
                        relat.ValorApropriado = 0;
                        relat.CodigoDescricaoClasse = "";
                        relat.CodigoDescricaoCentroCusto = "";

                        listaRelContasPagarTitulos.Add(relat);
                    }
                }
                else
                {
                    relat.ValorApropriado = 0;
                    relat.CodigoDescricaoClasse = "";
                    relat.CodigoDescricaoCentroCusto = "";

                    listaRelContasPagarTitulos.Add(relat);
                }

            }

            totalRegistros = listaRelContasPagarTitulos.Count();

            totalizadoValorTitulo = listaRelContasPagarTitulos.Sum(l => l.ValorTitulo);
            totalizadoValorLiquido = listaRelContasPagarTitulos.Sum(l => l.ValorLiquido);
            totalizadoValorApropriado = listaRelContasPagarTitulos.Sum(l => l.ValorApropriado);

            listaRelContasPagarTitulos = OrdenaListaRelContasPagarTitulosDTO(filtro, listaRelContasPagarTitulos);

            listaRelContasPagarTitulos = listaRelContasPagarTitulos.Skip(pageCount * pageIndex).Take(pageCount).To<List<RelContasPagarTitulosDTO>>();

            return listaRelContasPagarTitulos;
        }

        private decimal CalculaValorLiquido(TituloPagar titulo, DateTime dataEmissao)
        {
            decimal valorLiquido = 0;

            if (!ValidaData(titulo.DataVencimento.ToShortDateString()))
            {
                return valorLiquido;
            }

            decimal valorImposto = titulo.ValorImposto.HasValue ? titulo.ValorImposto.Value : 0;

            valorLiquido = titulo.ValorTitulo - valorImposto;

            decimal valorMultaCalculada = 0;
            decimal taxaPermanenciaCalculada = 0;
            decimal multa = titulo.Multa.HasValue ? titulo.Multa.Value : 0;
            decimal taxaPermanencia = titulo.TaxaPermanencia.HasValue ? titulo.TaxaPermanencia.Value : 0;
            int atraso = moduloSigimAppService.ObtemQuantidadeDeDias(titulo.DataVencimento,dataEmissao);
            int fator;
            decimal valorTaxa = 0;
            decimal retencao = titulo.Retencao.HasValue ? titulo.Retencao.Value : 0;
            decimal desconto = titulo.Desconto.HasValue ? titulo.Desconto.Value : 0;

            if (!moduloSigimAppService.OperacaoEmDia(titulo.DataVencimento, dataEmissao))
            {
                if ((titulo.EhMultaPercentual.HasValue) && (titulo.EhMultaPercentual.Value))
                {
                    valorMultaCalculada = ((titulo.ValorTitulo * multa) / 100);
                    valorMultaCalculada = Math.Round(valorMultaCalculada,2);
                }
                else
                {
                    valorMultaCalculada = Math.Round(multa,2);
                }

                if ((titulo.EhTaxaPermanenciaPercentual.HasValue) && (titulo.EhTaxaPermanenciaPercentual.Value))
                {
                    fator = 30;
                    valorTaxa = (taxaPermanencia / fator) * atraso;
                    taxaPermanenciaCalculada = ((titulo.ValorTitulo * valorTaxa) / 100);
                }
                else
                {
                    fator = 1;
                    valorTaxa = (taxaPermanencia / fator) * atraso;
                    taxaPermanenciaCalculada = Math.Round(valorTaxa,2);
                }
                valorLiquido = valorLiquido -  retencao + valorMultaCalculada + taxaPermanenciaCalculada;
                if (titulo.DataLimiteDesconto.HasValue)
                {
                    if (titulo.DataLimiteDesconto.Value.Date >= dataEmissao)
                    {
                        valorLiquido = Math.Round((valorLiquido - desconto),2);
                    }
                }
            }
            else
            {
                if (titulo.DataLimiteDesconto.HasValue)
                {
                    if (titulo.DataLimiteDesconto.Value.Date >= dataEmissao)
                    {
                        valorLiquido = Math.Round((valorLiquido - desconto),2);
                    }
                }
                valorLiquido = Math.Round((valorLiquido - retencao),2);
            }

            return valorLiquido;
        }

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

            specification &= TituloPagarSpecification.EhTipoTituloDiferenteDeTituloPai();

            specification &= TituloPagarSpecification.ValorTituloMaiorOuIgualRelContasPagarTitulos(filtro.ValorTituloInicial);
            specification &= TituloPagarSpecification.ValorTituloMenorOuIgualRelContasPagarTitulos(filtro.ValorTituloFinal);

            specification &= TituloPagarSpecification.MatchingMovimentoBancoId(filtro.BancoId);

            specification &= TituloPagarSpecification.MatchingMovimentoContaCorrenteId(filtro.ContaCorrenteId);

            specification &= TituloPagarSpecification.MatchingMovimentoCaixaId(filtro.CaixaId);

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

            specification &= TituloPagarSpecification.EhTipoTituloDiferenteDeTituloPai();

            specification &= TituloPagarSpecification.ValorTituloMaiorOuIgualRelContasPagarTitulos(filtro.ValorTituloInicial);
            specification &= TituloPagarSpecification.ValorTituloMenorOuIgualRelContasPagarTitulos(filtro.ValorTituloFinal);

            specification &= TituloPagarSpecification.MatchingMovimentoBancoId(filtro.BancoId);

            specification &= TituloPagarSpecification.MatchingMovimentoContaCorrenteId(filtro.ContaCorrenteId);

            specification &= TituloPagarSpecification.MatchingMovimentoCaixaId(filtro.CaixaId);

            return specification;
        }


        #endregion
    }
}
