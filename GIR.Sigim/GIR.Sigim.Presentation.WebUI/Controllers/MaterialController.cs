using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GIR.Sigim.Application.Filtros.Sigim;
using GIR.Sigim.Application.Service.Sigim;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Presentation.WebUI.ViewModel;

namespace GIR.Sigim.Presentation.WebUI.Controllers
{
    public class MaterialController : BaseController
    {
        private IMaterialAppService materialAppService;
        private IUnidadeMedidaAppService unidadeMedidaAppService;

        public MaterialController(
            IMaterialAppService materialAppService,
            IUnidadeMedidaAppService unidadeMedidaAppService,
            MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.materialAppService = materialAppService;
            this.unidadeMedidaAppService = unidadeMedidaAppService;
        }

        public ActionResult Index()
        {
            var model = Session["Filtro"] as MaterialListaViewModel;
            if (model == null)
            {
                model = new MaterialListaViewModel();
                model.Filtro.PaginationParameters.PageSize = this.DefaultPageSize;
            }
            CarregarCombos(model);
            return View(model);
        }

        //[HttpPost]
        //public ActionResult ListaPelaDescricao(string descricao)
        //{
        //    var model = materialAppService.ListarAtivosPeloTipoTabelaPropria(descricao);
        //    return Json(model);
        //}

        [HttpPost]
        public ActionResult ListarAtivosPeloCentroCustoEDescricao(string codigoCentroCusto, string descricao)
        {
            var model = materialAppService.ListarAtivosPeloCentroCustoEDescricao(codigoCentroCusto, descricao);
            return Json(model);
        }

        [HttpPost]
        public ActionResult PesquisarMaterial(MaterialPesquisaFiltro filtro)
        {
            //var model = materialAppService.PesquisarMaterial(filtro);
            int totalRegistros;
            var result = materialAppService.PesquisarAtivosPeloFiltro(filtro, out totalRegistros);
            if (result.Any())
            {
                var listaViewModel = CreateListaViewModel(filtro, totalRegistros, result);
                return PartialView("ListaPesquisaPartial", listaViewModel);
            }
            return PartialView("_EmptyListPartial");
        }

        [HttpPost]
        public ActionResult ObterInterfaceOrcamento(int? materialId, string codigoCentroCusto, string codigoClasse)
        {
            InterfaceOrcamentoViewModel model = new InterfaceOrcamentoViewModel();
            bool possuiInterfaceOrcamento;
            model.ListaItens = materialAppService.ListarOrcamentoComposicaoItem(materialId, codigoCentroCusto, codigoClasse, out possuiInterfaceOrcamento);
            model.ExibirTelaInterfaceOrcamento = possuiInterfaceOrcamento;
            model.ErrorMessages = messageQueue.GetAll().Where(l => l.Type == TypeMessage.Error).ToList();
            messageQueue.Clear();
            return Json(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Lista(MaterialListaViewModel model)
        {
            if (ModelState.IsValid)
            {
                Session["Filtro"] = model;
                int totalRegistros;

                if (string.IsNullOrEmpty(model.Filtro.PaginationParameters.OrderBy))
                    model.Filtro.PaginationParameters.OrderBy = "id";

                var result = materialAppService.ListarPeloFiltro(model.Filtro, out totalRegistros);
                if (result.Any())
                {
                    var listaViewModel = CreateListaViewModel(model.Filtro.PaginationParameters, totalRegistros, result);
                    return PartialView("ListaPartial", listaViewModel);
                }
                return PartialView("_EmptyListPartial");
            }
            return PartialView("_NotificationMessagesPartial");
        }

        private void CarregarCombos(MaterialListaViewModel model)
        {
            string sigla = string.Empty;

            if (model.Filtro != null)
            {
                sigla = model.Filtro.Sigla;
            }

            model.ListaUnidadeMedida = new SelectList(unidadeMedidaAppService.ListarTodos(), "Sigla", "Descricao", sigla);
        }
    }
}