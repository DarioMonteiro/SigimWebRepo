using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Presentation.WebUI.Controllers;
using GIR.Sigim.Application.Service.Admin;
using GIR.Sigim.Application.Service.Sigim;

namespace GIR.Sigim.Presentation.WebUI.Areas.Financeiro.Controllers
{
    public class HomeController : BaseController
    {
        private IModuloAppService moduloAppService;
        private IAcessoAppService acessoAppService;

        public HomeController(IModuloAppService moduloAppService,
                              IAcessoAppService acessoAppService,
                              MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.moduloAppService = moduloAppService;
            this.acessoAppService = acessoAppService;
        }

        public ActionResult Index()
        {
            if (!moduloAppService.PossuiModulo(GIR.Sigim.Application.Resource.Sigim.NomeModulo.Financeiro))
            {
                return RedirectToLocal("/");
            }

            bool logGirCliente = this.LogGIRCliente;
            if (!acessoAppService.ValidaAcessoAoModulo(GIR.Sigim.Application.Resource.Sigim.NomeModulo.Financeiro, logGirCliente))
            {
                return RedirectToLocal("/");
            }
            return View();
        }

    }
}
