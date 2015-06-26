using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GIR.Sigim.Application.DTO.Sac;
using GIR.Sigim.Application.Service.Sac;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Presentation.WebUI.Areas.Sac.ViewModel;
using GIR.Sigim.Presentation.WebUI.Controllers;

namespace GIR.Sigim.Presentation.WebUI.Areas.Sac.Controllers
{
    public class SetorController : BaseController
    {
        private ISetorAppService SetorAppService;

        public SetorController(
            ISetorAppService SetorAppService,
            MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.SetorAppService = SetorAppService;
        }
       
        public ActionResult Index(int? id)
        {
            var model = Session["Filtro"] as SetorViewModel;
            if (model == null)
            {
                model = new SetorViewModel();
                model.Filtro.PaginationParameters.PageSize = this.DefaultPageSize;
            }
            var Setor = SetorAppService.ObterPeloId(id) ?? new SetorDTO();

            if (id.HasValue && !Setor.Id.HasValue)
                messageQueue.Add(Application.Resource.Sigim.ErrorMessages.NenhumRegistroEncontrado, TypeMessage.Error);

            model.Setor = Setor;

            return View(model);
        }

        public ActionResult Lista(SetorViewModel model)
        {
            if (ModelState.IsValid)
            {
                Session["Filtro"] = model;
                int totalRegistros;
                var result = SetorAppService.ListarPeloFiltro(model.Filtro, out totalRegistros);
                if (result.Any())
                {
                    var listaViewModel = CreateListaViewModel(model.Filtro.PaginationParameters, totalRegistros, result);
                    return PartialView("ListaPartial", listaViewModel);
                }
                return PartialView("_EmptyListPartial");
            }
            return PartialView("_NotificationMessagesPartial");
        }

        public ActionResult CarregarItem(int? id)
        {
            var Setor = SetorAppService.ObterPeloId(id) ?? new SetorDTO();
            return Json(Setor);
        }

        [HttpPost]
        public ActionResult Salvar(SetorViewModel model)
        {
            if (ModelState.IsValid)
                SetorAppService.Salvar(model.Setor);

            return PartialView("_NotificationMessagesPartial");
        }

        [HttpPost]
        public ActionResult Deletar(int? id)
        {
            SetorAppService.Deletar(id);
            return PartialView("_NotificationMessagesPartial");
        }      

    }
}
