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

namespace GIR.Sigim.Presentation.WebUI.Areas.Financeiro.Controllers
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
            InformacaoConfiguracaoDTO informacaoConfiguracao = moduloSigimAppService.SetarInformacaoConfiguracao(this.LogGIRCliente, this.EnderecoIP, this.Instancia, this.StringConexao);

            if (!moduloAppService.PossuiModulo(GIR.Sigim.Application.Resource.Sigim.NomeModulo.Financeiro))
            {
                return RedirectToLocal("/");
            }

            if (!acessoAppService.ValidaAcessoAoModulo(GIR.Sigim.Application.Resource.Sigim.NomeModulo.Financeiro, informacaoConfiguracao))
            {
                return RedirectToLocal("/");
            }

            if (!acessoAppService.ValidaAcessoGirCliente(GIR.Sigim.Application.Resource.Sigim.NomeModulo.Financeiro, Usuario.Id.Value, informacaoConfiguracao))
            {
                return RedirectToLocal("/");
            }


            return View();
        }

    }
}
