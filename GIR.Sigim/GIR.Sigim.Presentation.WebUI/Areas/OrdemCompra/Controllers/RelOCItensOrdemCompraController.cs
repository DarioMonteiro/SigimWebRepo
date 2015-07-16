using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GIR.Sigim.Presentation.WebUI.Controllers;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Presentation.WebUI.Areas.OrdemCompra.ViewModel;
using GIR.Sigim.Application.Service.Sigim;
using GIR.Sigim.Application.Service.OrdemCompra;


namespace GIR.Sigim.Presentation.WebUI.Areas.OrdemCompra.Controllers
{

    public class RelOCItensOrdemCompraController : BaseController
    {
        private IOrdemCompraItemAppService ordemCompraItemAppService;

        public RelOCItensOrdemCompraController(IOrdemCompraItemAppService ordemCompraItemAppService,
                                               MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.ordemCompraItemAppService = ordemCompraItemAppService;
        }

        public ActionResult Index()
        {
            var model = Session["Filtro"] as RelOcItensOrdemCompraListaViewModel;
            if (model == null)
            {
                model = new RelOcItensOrdemCompraListaViewModel();
                model.Filtro.PaginationParameters.PageSize = this.DefaultPageSize;
                model.Filtro.DataInicial = DateTime.Now;
                model.Filtro.DataFinal = DateTime.Now;
            }

            model.PodeImprimir = true;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Lista(RelOcItensOrdemCompraListaViewModel model)
        {
            if (ModelState.IsValid)
            {
                Session["Filtro"] = model;
                int totalRegistros;

                if (string.IsNullOrEmpty(model.Filtro.PaginationParameters.OrderBy))
                    model.Filtro.PaginationParameters.OrderBy = "ordemCompraId";

                var result = ordemCompraItemAppService.ListarPeloFiltroRelOCItensOrdemCompra(model.Filtro, Usuario.Id, out totalRegistros);

                if (result.Any())
                {
                    var listaViewModel = CreateListaViewModel(model.Filtro.PaginationParameters, totalRegistros, result);
                    return PartialView("ListaPartial", listaViewModel);
                }
                return PartialView("_EmptyListPartial");
            }
            return PartialView("_NotificationMessagesPartial");
        }

    }
}
