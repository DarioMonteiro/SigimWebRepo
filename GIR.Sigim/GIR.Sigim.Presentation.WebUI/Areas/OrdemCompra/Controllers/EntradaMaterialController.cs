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
        }

        [Authorize(Roles = Funcionalidade.EntradaMaterialAcessar)]
        public ActionResult Index()
        {
            var model = Session["Filtro"] as EntradaMaterialListaViewModel;
            if (model == null)
            {
                model = new EntradaMaterialListaViewModel();
                model.Filtro.PaginationParameters.PageSize = this.DefaultPageSize;
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
                    var listaViewModel = CreateListaViewModel(model.Filtro.PaginationParameters, totalRegistros, result);
                    return PartialView("ListaPartial", listaViewModel);
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

            if ((entradaMaterial.CentroCusto == null) || (string.IsNullOrEmpty(entradaMaterial.CentroCusto.Codigo)))
            {
                var parametrosUsuario = parametrosUsuarioAppService.ObterPeloIdUsuario(Usuario.Id);
                model.EntradaMaterial.CentroCusto = parametrosUsuario.CentroCusto;
            }

            //var parametros = parametrosOrdemCompraAppService.Obter();
            //if (parametros != null)
            //{
            //    model.DataMinima = parametros.DiasDataMinima.HasValue ? DateTime.Now.AddDays(parametros.DiasDataMinima.Value) : DateTime.Now;
            //    model.DataMaxima = parametros.DiasPrazo.HasValue ? model.DataMinima.Value.AddDays(parametros.DiasPrazo.Value) : DateTime.Now;
            //    model.Prazo = parametros.DiasPrazo.HasValue ? parametros.DiasPrazo.Value : 0;
            //}

            model.PodeSalvar = entradaMaterialAppService.EhPermitidoSalvar(entradaMaterial);
            model.PodeCancelarEntrada = entradaMaterialAppService.EhPermitidoCancelar(entradaMaterial);
            model.ExisteEstoqueParaCentroCusto = entradaMaterialAppService.ExisteEstoqueParaCentroCusto(entradaMaterial.CentroCusto.Codigo);
            model.ExisteMovimentoNoEstoque = entradaMaterialAppService.ExisteMovimentoNoEstoque(entradaMaterial);
            model.PodeImprimir = entradaMaterialAppService.EhPermitidoImprimir(entradaMaterial);
            model.PodeLiberarTitulos = entradaMaterialAppService.EhPermitidoLiberarTitulos(entradaMaterial);
            model.PodeAdicionarItem = entradaMaterialAppService.EhPermitidoAdicionarItem(entradaMaterial);
            model.PodeRemoverItem = entradaMaterialAppService.EhPermitidoRemoverItem(entradaMaterial);
            model.PodeEditarItem = entradaMaterialAppService.EhPermitidoEditarItem(entradaMaterial);
            //model.PodeAprovarRequisicao = requisicaoMaterialAppService.EhPermitidoAprovarRequisicao(entradaMaterial);
            //model.PodeCancelarAprovacao = requisicaoMaterialAppService.EhPermitidoCancelarAprovacao(entradaMaterial);
            model.PodeEditarCentroCusto = entradaMaterialAppService.EhPermitidoEditarCentroCusto(entradaMaterial);
            model.PodeEditarFornecedor = entradaMaterialAppService.EhPermitidoEditarFornecedor(entradaMaterial);
            CarregarCombos(model);

            return View(model);
        }

        private void CarregarCombos(EntradaMaterialCadastroViewModel model)
        {
            int? tipoNotaFiscalId = null;
            string CodigoTipoCompra = null;
            int? CifFobId = null;
            string codigoNaturezaOperacao = null;
            int? SerieNFId = null;
            string CodigoCST = null;
            string CodigoContribuicaoId = null;

            if (model.EntradaMaterial != null)
            {
                tipoNotaFiscalId = model.EntradaMaterial.TipoNotaFiscalId;
                CodigoTipoCompra = model.EntradaMaterial.CodigoTipoCompra;
                CifFobId = model.EntradaMaterial.CifFobId;
                codigoNaturezaOperacao = model.EntradaMaterial.CodigoNaturezaOperacao;
                SerieNFId = model.EntradaMaterial.SerieNFId;
                CodigoCST = model.EntradaMaterial.CodigoCST;
                CodigoContribuicaoId = model.EntradaMaterial.CodigoContribuicaoId;
            }

            model.ListaTipoNotaFiscal = new SelectList(tipoDocumentoAppService.ListarTodos(), "Id", "Sigla", tipoNotaFiscalId);
            model.ListaTipoCompra = new SelectList(tipoCompraAppService.ListarTodos(), "Codigo", "Descricao", tipoNotaFiscalId);
            model.ListaCifFob = new SelectList(cifFobAppService.ListarTodos(), "Id", "Descricao", tipoNotaFiscalId);
            model.ListaNaturezaOperacao = new SelectList(naturezaOperacaoAppService.ListarTodos(), "Codigo", "CodigoComDescricao", tipoNotaFiscalId);
            model.ListaSerieNF = new SelectList(serieNFAppService.ListarTodos(), "Id", "Descricao", tipoNotaFiscalId);
            model.ListaCST = new SelectList(CSTAppService.ListarTodos(), "Codigo", "Descricao", tipoNotaFiscalId);
            model.ListaCodigoContribuicao = new SelectList(codigoContribuicaoAppService.ListarTodos(), "Codigo", "Descricao", tipoNotaFiscalId);
            model.ListaComplementoNaturezaOperacao = new SelectList(complementoNaturezaOperacaoAppService.ListarPorNaturezaOperacao(codigoNaturezaOperacao), "Codigo", "Descricao");
            model.ListaComplementoCST = new SelectList(complementoCSTAppService.ListarTodos(), "Codigo", "Descricao");
            model.ListaNaturezaReceita = new SelectList(naturezaReceitaAppService.ListarTodos(), "Codigo", "Descricao");
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
            PossibilidadeCancelamentoEntradaMaterialViewModel model = new PossibilidadeCancelamentoEntradaMaterialViewModel();
            model.HaPossibilidadeCancelamentoEntradaMaterial = entradaMaterialAppService.HaPossibilidadeCancelamentoEntradaMaterial(entradaMaterialId);
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
    }
}