using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GIR.Sigim.Application.DTO.Financeiro;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Application.Service.Financeiro;
using GIR.Sigim.Application.Service.Sigim;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Presentation.WebUI.Areas.Financeiro.ViewModel;
using GIR.Sigim.Presentation.WebUI.Controllers;

namespace GIR.Sigim.Presentation.WebUI.Areas.Financeiro.Controllers
{
    public class TipoCompromissoController : BaseController
    {
        private ITipoCompromissoAppService tipoCompromissoAppService;

        public TipoCompromissoController(
            ITipoCompromissoAppService tipoCompromissoAppService,
            MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.tipoCompromissoAppService = tipoCompromissoAppService;
        }

        /*public ActionResult Index()
        {
            TipoCompromissoViewModel model = new TipoCompromissoViewModel();
            return View(model);
        }*/

        public ActionResult Index(int? id)
        {
            TipoCompromissoViewModel model = new TipoCompromissoViewModel();
            var tipoCompromisso = tipoCompromissoAppService.ObterPeloId(id) ?? new TipoCompromissoDTO();

            if (id.HasValue && !tipoCompromisso.Id.HasValue)
                messageQueue.Add(Application.Resource.Sigim.ErrorMessages.NenhumRegistroEncontrado, TypeMessage.Error);

            model.TipoCompromisso = tipoCompromisso;

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        /*public ActionResult Index(TipoCompromissoViewModel model)
        {
            if (ModelState.IsValid)
            {
               TipoCompromissoAppService.Salvar(model.Parametros);
            }

            return PartialView("_NotificationMessagesPartial");
        }*/
        public ActionResult Lista(TipoCompromissoViewModel model)
        {
            if (ModelState.IsValid)
            {
                int totalRegistros;
                var result = tipoCompromissoAppService.ListarPeloFiltro (model.Filtro, out totalRegistros);
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
        public ActionResult Novo(TipoCompromissoViewModel model)
        {
            //Limpar o campo escondigo do ID
            model.TipoCompromisso.Descricao =string.Empty ;
            model.TipoCompromisso.TipoPagar = false;
            model.TipoCompromisso.TipoReceber = false;

            Lista(model);

            return View(model);
        }

    }
}
