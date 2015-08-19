using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Application.Filtros;
using GIR.Sigim.Application.Service.Sigim;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Presentation.WebUI.ViewModel;
using GIR.Sigim.Presentation.WebUI.Controllers;
using GIR.Sigim.Application.Constantes;

namespace GIR.Sigim.Presentation.WebUI.Controllers
{
    public class FormaRecebimentoController : BaseController
    {
        private IFormaRecebimentoAppService formaRecebimentoAppService;               

        public FormaRecebimentoController(
            IFormaRecebimentoAppService formaRecebimentoAppService,            
            MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.formaRecebimentoAppService = formaRecebimentoAppService;            
        }

        [Authorize(Roles = Funcionalidade.FormaRecebimentoAcessar)]
        public ActionResult Index(int? id)
        {
           
            var model = Session["Filtro"] as FormaRecebimentoViewModel;
            if (model == null)
            {
                model = new FormaRecebimentoViewModel();
                model.Filtro.PaginationParameters.PageSize = this.DefaultPageSize;
                model.Filtro.PaginationParameters.UniqueIdentifier = GenerateUniqueIdentifier();
            }

            model.PodeSalvar = formaRecebimentoAppService.EhPermitidoSalvar();
            model.PodeDeletar = formaRecebimentoAppService.EhPermitidoDeletar();
            //model.PodeImprimir = formaRecebimentoAppService.EhPermitidoImprimir();

            model.PodeHabilitarNumeroDias = false;

            var formaRecebimento = formaRecebimentoAppService.ObterPeloId(id) ?? new FormaRecebimentoDTO();
            
            if (id.HasValue && !formaRecebimento.Id.HasValue)
                messageQueue.Add(Application.Resource.Sigim.ErrorMessages.NenhumRegistroEncontrado, TypeMessage.Error);

            model.FormaRecebimento = formaRecebimento;

            CarregarCombos(model);

            return View(model);
        }

        public ActionResult Lista(FormaRecebimentoViewModel model)
        {
            if (ModelState.IsValid)
            {
                Session["Filtro"] = model;
                int totalRegistros;
                var result = formaRecebimentoAppService.ListarPeloFiltro(model.Filtro, out totalRegistros);
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
            var formaRecebimento = formaRecebimentoAppService.ObterPeloId(id) ?? new FormaRecebimentoDTO();
            return Json(formaRecebimento);
        }

        private void CarregarCombos(FormaRecebimentoViewModel model)
        {
            model.ListaTipoRecebimento = new SelectList(formaRecebimentoAppService.ListarTipoRecebimento(),"Id", "Descricao", model.FormaRecebimento.TipoRecebimento);
        }

        [HttpPost]
        public ActionResult Salvar(FormaRecebimentoViewModel model)
        {
            if (ModelState.IsValid)
               
             formaRecebimentoAppService.Salvar(model.FormaRecebimento);
               
            return PartialView("_NotificationMessagesPartial");
        }

        [HttpPost]
        public ActionResult Deletar(int? id)
        {
            formaRecebimentoAppService.Deletar(id);
            return PartialView("_NotificationMessagesPartial");
        }

    }
}