using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GIR.Sigim.Presentation.WebUI.Controllers;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Application.Service.Admin;
using GIR.Sigim.Application.Service.Sigim;
using GIR.Sigim.Application.DTO.Sigim;

namespace GIR.Sigim.Presentation.WebUI.Areas.Orcamento.Controllers
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

            if (!moduloAppService.PossuiModulo(GIR.Sigim.Application.Constantes.Modulo.OrcamentoWeb))
            {
                return RedirectToAction("Index", "Painel", new { Area = "" });
            }

            if (!acessoAppService.ValidaAcessoAoModulo(GIR.Sigim.Application.Constantes.Modulo.OrcamentoWeb, informacaoConfiguracao))
            {
                return RedirectToAction("Index", "Painel", new { Area = "" });
            }

            if (!acessoAppService.ValidaAcessoGirCliente(GIR.Sigim.Application.Constantes.Modulo.OrcamentoWeb, Usuario.Id.Value, informacaoConfiguracao))
            {
                return RedirectToAction("Index", "Painel", new { Area = "" });
            }

            return View();
        }

    }
}
