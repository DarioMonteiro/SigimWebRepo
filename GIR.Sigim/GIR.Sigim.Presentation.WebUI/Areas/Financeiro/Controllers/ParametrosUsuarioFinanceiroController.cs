using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GIR.Sigim.Application.Service.Financeiro;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Presentation.WebUI.Areas.Financeiro.ViewModel;
using GIR.Sigim.Presentation.WebUI.Controllers;

namespace GIR.Sigim.Presentation.WebUI.Areas.Financeiro.Controllers
{
    public class ParametrosUsuarioFinanceiroController : BaseController
    {
        #region Properties
            private IParametrosUsuarioFinanceiroAppService parametrosUsuarioFinanceiroAppService;
        #endregion

        #region Constructor

            public ParametrosUsuarioFinanceiroController(IParametrosUsuarioFinanceiroAppService parametrosUsuarioFinanceiroAppService, MessageQueue messageQueue)
                : base(messageQueue)
            {
                this.parametrosUsuarioFinanceiroAppService = parametrosUsuarioFinanceiroAppService;
            }

        #endregion

        #region Methods

            public ActionResult Index()
            {
                ParametrosUsuarioFinanceiroViewModel model = new ParametrosUsuarioFinanceiroViewModel();
                model.ParametrosUsuarioFinanceiro = parametrosUsuarioFinanceiroAppService.ObterPeloIdUsuario(Usuario.Id);
                return View(model);               
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult Index(ParametrosUsuarioFinanceiroViewModel model)
            {
                if (ModelState.IsValid)
                {
                    model.ParametrosUsuarioFinanceiro.Id = Usuario.Id;
                    parametrosUsuarioFinanceiroAppService.Salvar(model.ParametrosUsuarioFinanceiro);
                }

                return PartialView("_NotificationMessagesPartial");
            }


        #endregion

    }
}
