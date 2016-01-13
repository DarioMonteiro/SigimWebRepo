using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GIR.Sigim.Presentation.WebUI.Controllers;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Application.Constantes;
using GIR.Sigim.Presentation.WebUI.Areas.Financeiro.ViewModel;


namespace GIR.Sigim.Presentation.WebUI.Areas.Financeiro.Controllers
{
    public class RelApropriacaoPorClasseController : BaseController
    {
        public RelApropriacaoPorClasseController(MessageQueue messageQueue)
            : base(messageQueue)
        {
        }

        [Authorize(Roles = Funcionalidade.RelApropriacaoPorClasseAcessar)]
        public ActionResult Index()
        {
            var model = Session["Filtro"] as RelApropriacaoPorClasseListaViewModel;
            if (model == null)
            {
                model = new RelApropriacaoPorClasseListaViewModel();
                model.Filtro.PaginationParameters.PageSize = this.DefaultPageSize;
                model.Filtro.PaginationParameters.UniqueIdentifier = GenerateUniqueIdentifier();
                model.Filtro.DataInicial = DateTime.Now;
                model.Filtro.DataFinal = DateTime.Now;
            }

            model.PodeImprimir = false;
            //model.PodeImprimir = contratoRetificacaoItemMedicaoAppService.EhPermitidoImprimir();

            return View(model);
        }

    }
}
