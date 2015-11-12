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
using GIR.Sigim.Application.Constantes;

namespace GIR.Sigim.Presentation.WebUI.Areas.Financeiro.Controllers
{
    public class CaixaController : BaseController
    {
        private ICaixaAppService caixaAppService;

        public CaixaController(
            ICaixaAppService caixaAppService,
            MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.caixaAppService = caixaAppService;
        }

        [Authorize(Roles = Funcionalidade.CaixaAcessar)]
        public ActionResult Index(int? id)
        {
            var model = Session["Filtro"] as CaixaViewModel;
            if (model == null)
            {
                model = new CaixaViewModel();
                model.Filtro.PaginationParameters.PageSize = this.DefaultPageSize;
                model.Filtro.PaginationParameters.UniqueIdentifier = GenerateUniqueIdentifier();
            }

            model.PodeSalvar = caixaAppService.EhPermitidoSalvar();
            model.PodeDeletar = caixaAppService.EhPermitidoDeletar();
            model.PodeImprimir = caixaAppService.EhPermitidoImprimir();

            var caixa = caixaAppService.ObterPeloId(id) ?? new CaixaDTO();

            if (id.HasValue && !caixa.Id.HasValue)
                messageQueue.Add(Application.Resource.Sigim.ErrorMessages.NenhumRegistroEncontrado, TypeMessage.Error);

            model.Caixa = caixa;

            return View(model);
        }

        public ActionResult CarregarItem(int? id)
        {
            var caixa = caixaAppService.ObterPeloId(id) ?? new CaixaDTO();
            return Json(caixa);
        }        

        public ActionResult Lista(CaixaViewModel model)
        {
            if (ModelState.IsValid)
            {
                Session["Filtro"] = model;
                int totalRegistros;
                var result = caixaAppService.ListarPeloFiltro(model.Filtro, out totalRegistros);
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
        public ActionResult Salvar(CaixaViewModel model)
        {
            if (ModelState.IsValid)
                caixaAppService.Salvar(model.Caixa);

            return PartialView("_NotificationMessagesPartial");
        }

        [HttpPost]
        public ActionResult Deletar(int? id)
        {
            caixaAppService.Deletar(id);
            return PartialView("_NotificationMessagesPartial");
        }

        public ActionResult Imprimir(FormatoExportacaoArquivo formato)
        {
            var arquivo = caixaAppService.ExportarRelCaixa(formato);
            if (arquivo != null)
            {
                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();
                return File(arquivo.Stream, arquivo.ContentType, arquivo.NomeComExtensao);
            }

            return PartialView("_NotificationMessagesPartial");
        }

    }
}
