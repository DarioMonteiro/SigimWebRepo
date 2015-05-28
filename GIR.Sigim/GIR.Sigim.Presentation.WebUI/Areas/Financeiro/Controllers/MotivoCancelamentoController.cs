using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GIR.Sigim.Application.DTO.Financeiro;
using GIR.Sigim.Application.Service.Financeiro;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Presentation.WebUI.Areas.Financeiro.ViewModel;
using GIR.Sigim.Presentation.WebUI.Controllers;

namespace GIR.Sigim.Presentation.WebUI.Areas.Financeiro.Controllers
{
    public class MotivoCancelamentoController : BaseController
    {

        private IMotivoCancelamentoAppService motivoCancelamentoAppService;

        public MotivoCancelamentoController(
            IMotivoCancelamentoAppService motivoCancelamentoAppService,
            MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.motivoCancelamentoAppService = motivoCancelamentoAppService;
        }
       
        public ActionResult Index(int? id)
        {
            MotivoCancelamentoViewModel model = new MotivoCancelamentoViewModel();
            var motivoCancelamento = motivoCancelamentoAppService.ObterPeloId(id) ?? new MotivoCancelamentoDTO();

            if (id.HasValue && !motivoCancelamento.Id.HasValue)
                messageQueue.Add(Application.Resource.Sigim.ErrorMessages.NenhumRegistroEncontrado, TypeMessage.Error);

            model.MotivoCancelamento = motivoCancelamento;

            return View(model);
        }

        public ActionResult Lista(MotivoCancelamentoViewModel model)
        {
            if (ModelState.IsValid)
            {
                int totalRegistros;
                var result = motivoCancelamentoAppService.ListarPeloFiltro(model.Filtro, out totalRegistros);
                if (result.Any())
                {
                    var listaViewModel = CreateListaViewModel(model.Filtro.PaginationParameters, totalRegistros, result);
                    return PartialView("ListaPartial", listaViewModel);
                }
                return PartialView("_EmptyListPartial");
            }
            return PartialView("_NotificationMessagesPartial");
        }

        [HttpPost]
        public ActionResult Salvar(MotivoCancelamentoViewModel model)
        {
            if (ModelState.IsValid)
            {               
                if (motivoCancelamentoAppService.Salvar(model.MotivoCancelamento))
                    return PartialView("Redirect", Url.Action("", "MotivoCancelamento", new { id = model.MotivoCancelamento.Id }));
            }
            return PartialView("_NotificationMessagesPartial");
        }

                   
    }
}
