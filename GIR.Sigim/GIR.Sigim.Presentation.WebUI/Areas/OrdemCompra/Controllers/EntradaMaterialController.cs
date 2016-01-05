using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GIR.Sigim.Application.Constantes;
using GIR.Sigim.Application.DTO.OrdemCompra;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Application.Filtros;
using GIR.Sigim.Application.Service.Financeiro;
using GIR.Sigim.Application.Service.OrdemCompra;
using GIR.Sigim.Application.Service.Sigim;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Presentation.WebUI.Areas.OrdemCompra.ViewModel;
using GIR.Sigim.Presentation.WebUI.Controllers;
using GIR.Sigim.Presentation.WebUI.ViewModel;
using Newtonsoft.Json;

namespace GIR.Sigim.Presentation.WebUI.Areas.OrdemCompra.Controllers
{
    public class EntradaMaterialController : BaseController
    {
        private IEntradaMaterialAppService entradaMaterialAppService;
        private ITipoDocumentoAppService tipoDocumentoAppService;
        private ITipoCompraAppService tipoCompraAppService;
        private ICifFobAppService cifFobAppService;
        private INaturezaOperacaoAppService naturezaOperacaoAppService;
        private ISerieNFAppService serieNFAppService;
        private ICSTAppService CSTAppService;
        private ICodigoContribuicaoAppService codigoContribuicaoAppService;
        private IComplementoNaturezaOperacaoAppService complementoNaturezaOperacaoAppService;
        private IComplementoCSTAppService complementoCSTAppService;
        private INaturezaReceitaAppService naturezaReceitaAppService;
        private IParametrosUsuarioAppService parametrosUsuarioAppService;
        private IImpostoFinanceiroAppService impostoFinanceiroAppService;

        public EntradaMaterialController(
            IEntradaMaterialAppService entradaMaterialAppService,
            ITipoDocumentoAppService tipoDocumentoAppService,
            ITipoCompraAppService tipoCompraAppService,
            ICifFobAppService cifFobAppService,
            INaturezaOperacaoAppService naturezaOperacaoAppService,
            ISerieNFAppService serieNFAppService,
            ICSTAppService CSTAppService,
            ICodigoContribuicaoAppService codigoContribuicaoAppService,
            IComplementoNaturezaOperacaoAppService complementoNaturezaOperacaoAppService,
            IComplementoCSTAppService complementoCSTAppService,
            INaturezaReceitaAppService naturezaReceitaAppService,
            IParametrosUsuarioAppService parametrosUsuarioAppService,
            IImpostoFinanceiroAppService impostoFinanceiroAppService,
            MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.entradaMaterialAppService = entradaMaterialAppService;
            this.tipoDocumentoAppService = tipoDocumentoAppService;
            this.tipoCompraAppService = tipoCompraAppService;
            this.cifFobAppService = cifFobAppService;
            this.naturezaOperacaoAppService = naturezaOperacaoAppService;
            this.serieNFAppService = serieNFAppService;
            this.CSTAppService = CSTAppService;
            this.codigoContribuicaoAppService = codigoContribuicaoAppService;
            this.complementoNaturezaOperacaoAppService = complementoNaturezaOperacaoAppService;
            this.complementoCSTAppService = complementoCSTAppService;
            this.naturezaReceitaAppService = naturezaReceitaAppService;
            this.parametrosUsuarioAppService = parametrosUsuarioAppService;
            this.impostoFinanceiroAppService = impostoFinanceiroAppService;
        }

        [Authorize(Roles = Funcionalidade.EntradaMaterialAcessar)]
        public ActionResult Index()
        {
            var model = Session["Filtro"] as EntradaMaterialListaViewModel;
            if (model == null)
            {
                model = new EntradaMaterialListaViewModel();
                model.Filtro.PaginationParameters.PageSize = this.DefaultPageSize;
                model.Filtro.PaginationParameters.UniqueIdentifier = GenerateUniqueIdentifier();
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Lista(EntradaMaterialListaViewModel model)
        {
            if (ModelState.IsValid)
            {
                Session["Filtro"] = model;
                int totalRegistros;

                if (string.IsNullOrEmpty(model.Filtro.PaginationParameters.OrderBy))
                    model.Filtro.PaginationParameters.OrderBy = "id";

                var result = entradaMaterialAppService.ListarPeloFiltro(model.Filtro, out totalRegistros);
                if (result.Any())
                {
                    if (model.Filtro.PaginationParameters.PageIndex == 0 && result.Count == 1)
                    {
                        Session["Filtro"] = null;
                        return PartialView("Redirect", Url.Action("Cadastro", "EntradaMaterial", new { id = result[0].Id }));
                    }
                    else
                    {
                        var listaViewModel = CreateListaViewModel(model.Filtro.PaginationParameters, totalRegistros, result);
                        return PartialView("ListaPartial", listaViewModel);
                    }
                }
                return PartialView("_EmptyListPartial");
            }
            return PartialView("_NotificationMessagesPartial");
        }

        [Authorize(Roles = Funcionalidade.EntradaMaterialAcessar)]
        public ActionResult Cadastro(int? id)
        {
            EntradaMaterialCadastroViewModel model = new EntradaMaterialCadastroViewModel();
            var entradaMaterial = entradaMaterialAppService.ObterPeloId(id) ?? new EntradaMaterialDTO();

            if (id.HasValue && !entradaMaterial.Id.HasValue)
                messageQueue.Add(Application.Resource.Sigim.ErrorMessages.NenhumRegistroEncontrado, TypeMessage.Error);

            model.EntradaMaterial = entradaMaterial;
            model.JsonItens = JsonConvert.SerializeObject(entradaMaterial.ListaItens);
            model.JsonFormasPagamento = JsonConvert.SerializeObject(entradaMaterial.ListaFormaPagamento);
            model.JsonImpostos = JsonConvert.SerializeObject(entradaMaterial.ListaImposto);

            if ((entradaMaterial.CentroCusto == null) || (string.IsNullOrEmpty(entradaMaterial.CentroCusto.Codigo)))
            {
                var parametrosUsuario = parametrosUsuarioAppService.ObterPeloIdUsuario(Usuario.Id);
                model.EntradaMaterial.CentroCusto = parametrosUsuario.CentroCusto;
            }

            model.PodeSalvar = entradaMaterialAppService.EhPermitidoSalvar(entradaMaterial);
            model.PodeCancelarEntrada = entradaMaterialAppService.EhPermitidoCancelar(entradaMaterial);
            model.ExisteEstoqueParaCentroCusto = entradaMaterial.CentroCusto != null ? entradaMaterialAppService.ExisteEstoqueParaCentroCusto(entradaMaterial.CentroCusto.Codigo) : false;
            model.ExisteMovimentoNoEstoque = entradaMaterialAppService.ExisteMovimentoNoEstoque(entradaMaterial);
            model.PodeImprimir = entradaMaterialAppService.EhPermitidoImprimir(entradaMaterial);
            model.PodeLiberarTitulos = entradaMaterialAppService.EhPermitidoLiberarTitulos(entradaMaterial);
            model.PodeEditarCentroCusto = entradaMaterialAppService.EhPermitidoEditarCentroCusto(entradaMaterial);
            model.PodeEditarFornecedor = entradaMaterialAppService.EhPermitidoEditarFornecedor(entradaMaterial);
            model.PodeAdicionarItem = entradaMaterialAppService.EhPermitidoAdicionarItem(entradaMaterial);
            model.PodeRemoverItem = entradaMaterialAppService.EhPermitidoRemoverItem(entradaMaterial);
            model.PodeEditarItem = entradaMaterialAppService.EhPermitidoEditarItem(entradaMaterial);
            model.PodeAdicionarFormaPagamento = entradaMaterialAppService.EhPermitidoAdicionarFormaPagamento(entradaMaterial);
            model.PodeRemoverFormaPagamento = entradaMaterialAppService.EhPermitidoRemoverFormaPagamento(entradaMaterial);
            model.PodeAdicionarImposto = entradaMaterialAppService.EhPermitidoAdicionarImposto(entradaMaterial);
            model.PodeRemoverImposto = entradaMaterialAppService.EhPermitidoRemoverImposto(entradaMaterial);
            model.PodeEditarImposto = entradaMaterialAppService.EhPermitidoEditarImposto(entradaMaterial);
            CarregarCombos(model);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastro(EntradaMaterialCadastroViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.EntradaMaterial.ListaItens = Newtonsoft.Json.JsonConvert.DeserializeObject<List<EntradaMaterialItemDTO>>(model.JsonItens);
                model.EntradaMaterial.ListaImposto = Newtonsoft.Json.JsonConvert.DeserializeObject<List<EntradaMaterialImpostoDTO>>(model.JsonImpostos);
                if (entradaMaterialAppService.Salvar(model.EntradaMaterial))
                    return PartialView("Redirect", Url.Action("Cadastro", "EntradaMaterial", new { id = model.EntradaMaterial.Id }));
            }
            return PartialView("_NotificationMessagesPartial");
        }

        private void CarregarCombos(EntradaMaterialCadastroViewModel model)
        {
            int? tipoNotaFiscalId = null;
            int? tipoNotaFreteId = null;
            string CodigoTipoCompra = null;
            int? CifFobId = null;
            string codigoNaturezaOperacao = null;
            int? SerieNFId = null;
            string CodigoCST = null;
            string CodigoContribuicaoId = null;

            if (model.EntradaMaterial != null)
            {
                tipoNotaFiscalId = model.EntradaMaterial.TipoNotaFiscalId;
                tipoNotaFreteId = model.EntradaMaterial.TipoNotaFreteId;
                CodigoTipoCompra = model.EntradaMaterial.CodigoTipoCompra;
                CifFobId = model.EntradaMaterial.CifFobId;
                codigoNaturezaOperacao = model.EntradaMaterial.CodigoNaturezaOperacao;
                SerieNFId = model.EntradaMaterial.SerieNFId;
                CodigoCST = model.EntradaMaterial.CodigoCST;
                CodigoContribuicaoId = model.EntradaMaterial.CodigoContribuicaoId;
            }

            var listaTipoDocumento = tipoDocumentoAppService.ListarTodos();
            model.ListaTipoNotaFiscal = new SelectList(listaTipoDocumento, "Id", "Sigla", tipoNotaFiscalId);
            model.ListaTipoNotaFrete = new SelectList(listaTipoDocumento, "Id", "Sigla", tipoNotaFreteId);
            model.ListaTipoCompra = new SelectList(tipoCompraAppService.ListarTodos(), "Codigo", "Descricao", tipoNotaFiscalId);
            model.ListaCifFob = new SelectList(cifFobAppService.ListarTodos(), "Id", "Descricao", tipoNotaFiscalId);
            model.ListaNaturezaOperacao = new SelectList(naturezaOperacaoAppService.ListarTodos(), "Codigo", "CodigoComDescricao", tipoNotaFiscalId);
            model.ListaSerieNF = new SelectList(serieNFAppService.ListarTodos(), "Id", "Descricao", tipoNotaFiscalId);
            model.ListaCST = new SelectList(CSTAppService.ListarTodos(), "Codigo", "Descricao", tipoNotaFiscalId);
            model.ListaCodigoContribuicao = new SelectList(codigoContribuicaoAppService.ListarTodos(), "Codigo", "Descricao", tipoNotaFiscalId);
            model.ListaComplementoNaturezaOperacao = new SelectList(complementoNaturezaOperacaoAppService.ListarPorNaturezaOperacao(codigoNaturezaOperacao), "Codigo", "Descricao");
            model.ListaComplementoCST = new SelectList(complementoCSTAppService.ListarTodos(), "Codigo", "Descricao");
            model.ListaNaturezaReceita = new SelectList(naturezaReceitaAppService.ListarTodos(), "Codigo", "Descricao");
            model.ListaImpostoFinanceiro = new SelectList(impostoFinanceiroAppService.ListarTodos(), "Id", "Descricao");
        }

        [HttpPost]
        public ActionResult ListaComplementoNaturezaOperacao(string codigoNaturezaOperacao)
        {
            var lista = complementoNaturezaOperacaoAppService.ListarPorNaturezaOperacao(codigoNaturezaOperacao);
            return Json(lista);
        }

        [HttpPost]
        public ActionResult HaPossibilidadeCancelamentoEntradaMaterial(int? entradaMaterialId)
        {
            PossibilidadeAcaoViewModel model = new PossibilidadeAcaoViewModel();
            model.HaPossibilidadeAcao = entradaMaterialAppService.HaPossibilidadeCancelamentoEntradaMaterial(entradaMaterialId);
            model.ErrorMessages = messageQueue.GetAll().Where(l => l.Type == TypeMessage.Error).ToList();
            messageQueue.Clear();
            return Json(model);
        }

        [HttpPost]
        public ActionResult HaPossibilidadeLiberacaoTitulos(int? entradaMaterialId)
        {
            PossibilidadeAcaoViewModel model = new PossibilidadeAcaoViewModel();
            model.HaPossibilidadeAcao = entradaMaterialAppService.HaPossibilidadeLiberacaoTitulos(entradaMaterialId);
            model.ErrorMessages = messageQueue.GetAll().Where(l => l.Type == TypeMessage.Error).ToList();
            messageQueue.Clear();
            return Json(model);
        }

        [HttpPost]
        public ActionResult Cancelar(int? id, string motivo)
        {
            if (entradaMaterialAppService.CancelarEntrada(id, motivo))
                return PartialView("Redirect", Url.Action("Cadastro", "EntradaMaterial", new { id = id }));

            return PartialView("_NotificationMessagesPartial");
        }

        public ActionResult Imprimir(int? id, FormatoExportacaoArquivo formato)
        {
            var arquivo = entradaMaterialAppService.Exportar(id, formato);
            if (arquivo != null)
            {
                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();
                return File(arquivo.Stream, arquivo.ContentType, arquivo.NomeComExtensao);
            }

            return PartialView("_NotificationMessagesPartial");
        }

        [HttpPost]
        public ActionResult LiberarTitulos(int? id, string motivo)
        {
            if (entradaMaterialAppService.LiberarTitulos(id))
                return PartialView("Redirect", Url.Action("Cadastro", "EntradaMaterial", new { id = id }));

            return PartialView("_NotificationMessagesPartial");
        }

        [HttpPost]
        public ActionResult ListarItensDeOrdemCompraLiberadaComSaldo(int? entradaMaterialId)
        {
            var jsonItens = JsonConvert.SerializeObject(entradaMaterialAppService.ListarItensDeOrdemCompraLiberadaComSaldo(entradaMaterialId));
            var msg = messageQueue.GetAll().Any() ? messageQueue.GetAll().First().Text : string.Empty;
            messageQueue.Clear();
            return Json(new { errorMessage = msg, itens = jsonItens });
        }

        [HttpPost]
        public ActionResult AdicionarItens(int? entradaMaterialId, int?[] itens)
        {
            string jsonItens = "[]";
            if (entradaMaterialAppService.AdicionarItens(entradaMaterialId, itens))
                jsonItens = JsonConvert.SerializeObject(entradaMaterialAppService.ListarItens(entradaMaterialId));

            var messages = messageQueue.GetAll();
            messageQueue.Clear();
            return Json(new { Messages = messages, Itens = jsonItens });
        }

        [HttpPost]
        public ActionResult RemoverItens(int? entradaMaterialId, int?[] itens)
        {
            string jsonItens = "[]";
            if (entradaMaterialAppService.RemoverItens(entradaMaterialId, itens))
                jsonItens = JsonConvert.SerializeObject(entradaMaterialAppService.ListarItens(entradaMaterialId));

            var messages = messageQueue.GetAll();
            messageQueue.Clear();
            return Json(new { Messages = messages, Itens = jsonItens });
        }

        [HttpPost]
        public ActionResult RemoverFormasPagamento(int? entradaMaterialId, int?[] formasPagamento)
        {
            string jsonFormasPagamento = "[]";
            if (entradaMaterialAppService.RemoverFormasPagamento(entradaMaterialId, formasPagamento))
                jsonFormasPagamento = JsonConvert.SerializeObject(entradaMaterialAppService.ListarFormasPagamento(entradaMaterialId));

            var messages = messageQueue.GetAll();
            messageQueue.Clear();
            return Json(new { Messages = messages, FormasPagamento = jsonFormasPagamento });
        }

        [HttpPost]
        public ActionResult ListarFretePendente(int? entradaMaterialId)
        {
            return Json(JsonConvert.SerializeObject(entradaMaterialAppService.ListarFretePendente(entradaMaterialId)));
        }

        [HttpPost]
        public ActionResult ValidaDataEntradaMaterial(int? id, Nullable<DateTime> data)
        {
            var ehValido = entradaMaterialAppService.EhDataEntradaMaterialValida(id, data);
            var msg = messageQueue.GetAll().Any() ? messageQueue.GetAll()[0].Text : string.Empty;
            messageQueue.Clear();
            return Json(new { ehValido = ehValido, errorMessage = msg });
        }

        [HttpPost]
        public ActionResult ValidaNumeroNotaFiscal(EntradaMaterialDTO entradaMaterial)
        {
            var ehValido = entradaMaterialAppService.EhNumeroNotaFiscalValido(entradaMaterial);
            var msg = messageQueue.GetAll().Any() ? messageQueue.GetAll()[0].Text : string.Empty;
            messageQueue.Clear();
            return Json(new { ehValido = ehValido, errorMessage = msg });
        }

        [HttpPost]
        public ActionResult ValidaDataEmissaoNota(EntradaMaterialDTO entradaMaterial)
        {
            var ehValido = entradaMaterialAppService.EhDataEmissaoNotaValida(entradaMaterial);
            var msg = messageQueue.GetAll().Any() ? messageQueue.GetAll()[0].Text : string.Empty;
            messageQueue.Clear();
            return Json(new { ehValido = ehValido, errorMessage = msg });
        }

        [HttpPost]
        public ActionResult ListarFormasPagamentoOrdemCompraPendentes(int?[] ordemCompraIds)
        {
            return Json(JsonConvert.SerializeObject(entradaMaterialAppService.ListarFormasPagamentoOrdemCompraPendentes(ordemCompraIds)));
        }

        [HttpPost]
        public ActionResult AdicionarFormasPagamento(int? entradaMaterialId, string formasPagamento)
        {
            List<EntradaMaterialFormaPagamentoDTO> listaEntradaMaterialFormaPagamento = Newtonsoft.Json.JsonConvert.DeserializeObject<List<EntradaMaterialFormaPagamentoDTO>>(formasPagamento);
            string jsonFormasPagamento = "[]";
            if (entradaMaterialAppService.AdicionarFormasPagamento(entradaMaterialId, listaEntradaMaterialFormaPagamento))
                jsonFormasPagamento = JsonConvert.SerializeObject(entradaMaterialAppService.ListarFormasPagamento(entradaMaterialId));

            var messages = messageQueue.GetAll();
            messageQueue.Clear();
            return Json(new { Messages = messages, FormasPagamento = jsonFormasPagamento });
        }
    }
}