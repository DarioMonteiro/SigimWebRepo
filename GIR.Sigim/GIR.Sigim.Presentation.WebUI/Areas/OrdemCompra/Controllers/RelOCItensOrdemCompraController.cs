using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GIR.Sigim.Presentation.WebUI.Controllers;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Presentation.WebUI.Areas.OrdemCompra.ViewModel;
using GIR.Sigim.Application.Service.Sigim;


namespace GIR.Sigim.Presentation.WebUI.Areas.OrdemCompra.Controllers
{

    public class RelOCItensOrdemCompraController : BaseController
    {
        private IClienteFornecedorAppService clienteFornecedorAppService;

        public RelOCItensOrdemCompraController(IClienteFornecedorAppService clienteFornecedorAppService,
                                               MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.clienteFornecedorAppService = clienteFornecedorAppService;
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


    }
}
