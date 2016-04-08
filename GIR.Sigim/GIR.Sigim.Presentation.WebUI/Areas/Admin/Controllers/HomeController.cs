using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Presentation.WebUI.Controllers;
using GIR.Sigim.Application.Service.Admin;
using GIR.Sigim.Application.Service.Sigim;
using GIR.Sigim.Application.DTO.Sigim;

namespace GIR.Sigim.Presentation.WebUI.Areas.Admin.Controllers
{
    public class HomeController : BaseController
    {
        private IModuloAppService moduloAppService;
        private IAcessoAppService acessoAppService;
        private IModuloSigimAppService moduloSigimAppService;

        public HomeController(IModuloAppService moduloAppService,
                              IAcessoAppService acessoAppService,
                              IModuloSigimAppService moduloSigimAppService,
                              MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.moduloAppService = moduloAppService;
            this.acessoAppService = acessoAppService;
            this.moduloSigimAppService = moduloSigimAppService;      
        }

        public ActionResult Index()
        {
            InformacaoConfiguracaoDTO informacaoConfiguracao = moduloSigimAppService.SetarInformacaoConfiguracao(this.LogGIRCliente, Request.UserHostName);

            if (!moduloAppService.PossuiModulo(GIR.Sigim.Application.Constantes.Modulo.AdminWeb))
            {
                return RedirectToLocal("/");
            }

            if (!acessoAppService.ValidaAcessoAoModulo(GIR.Sigim.Application.Constantes.Modulo.AdminWeb, informacaoConfiguracao))
            {
                return RedirectToLocal("/");
            }

            if (!acessoAppService.ValidaAcessoGirCliente(GIR.Sigim.Application.Constantes.Modulo.AdminWeb, Usuario.Id.Value, informacaoConfiguracao))
            {
                return RedirectToLocal("/");
            }

            return View();
        }

    }
}
