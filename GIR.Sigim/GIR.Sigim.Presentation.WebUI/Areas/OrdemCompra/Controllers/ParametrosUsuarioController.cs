using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GIR.Sigim.Application.Constantes;
using GIR.Sigim.Application.Service.OrdemCompra;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Presentation.WebUI.Areas.OrdemCompra.ViewModel;
using GIR.Sigim.Presentation.WebUI.Controllers;

namespace GIR.Sigim.Presentation.WebUI.Areas.OrdemCompra.Controllers
{
    public class ParametrosUsuarioController : BaseController
    {
        private IParametrosUsuarioAppService parametrosUsuarioAppService;

        public ParametrosUsuarioController(IParametrosUsuarioAppService parametrosUsuarioAppService, MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.parametrosUsuarioAppService = parametrosUsuarioAppService;
        }

        [Authorize(Roles = Funcionalidade.ParametroUsuarioOrdemCompraAcessar)]
        public ActionResult Index()
        {
            ParametrosUsuarioViewModel model = new ParametrosUsuarioViewModel();
            model.ParametrosUsuario = parametrosUsuarioAppService.ObterPeloIdUsuario(Usuario.Id);
            model.PodeSalvar = parametrosUsuarioAppService.EhPermitidoSalvar();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(ParametrosUsuarioViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.ParametrosUsuario.Id = Usuario.Id;
                parametrosUsuarioAppService.Salvar(model.ParametrosUsuario);
            }

            return PartialView("_NotificationMessagesPartial");
        }
    }
}