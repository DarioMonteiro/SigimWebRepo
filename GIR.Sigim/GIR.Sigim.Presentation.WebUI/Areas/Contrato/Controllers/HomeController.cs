using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Presentation.WebUI.Controllers;
using GIR.Sigim.Application.Service.Admin;


namespace GIR.Sigim.Presentation.WebUI.Areas.Contrato.Controllers
{
    public class HomeController : BaseController
    {
        private IModuloAppService moduloAppService;

        public HomeController(IModuloAppService moduloAppService, 
                              MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.moduloAppService = moduloAppService;        
        }

        public ActionResult Index()
        {
            if (moduloAppService.PossuiModulo(GIR.Sigim.Application.Resource.Sigim.NomeModulo.Contrato))
            {
                return View();
            }
            return RedirectToLocal("/");
        }

    }
}
