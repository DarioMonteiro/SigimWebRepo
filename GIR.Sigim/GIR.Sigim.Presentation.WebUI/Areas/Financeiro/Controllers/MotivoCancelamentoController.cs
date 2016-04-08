using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GIR.Sigim.Application.DTO.Financeiro;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Application.Service.Financeiro;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Presentation.WebUI.Areas.Financeiro.ViewModel;
using GIR.Sigim.Presentation.WebUI.Controllers;
using GIR.Sigim.Application.Constantes;
using GIR.Sigim.Presentation.WebUI.CustomAttributes;

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

        [AutorizacaoAcessoAuthorize(GIR.Sigim.Application.Constantes.Modulo.FinanceiroWeb, Roles = Funcionalidade.MotivoCancelamentoAcessar)]
        public ActionResult Index(int? id)
        {
            var model = Session["Filtro"] as MotivoCancelamentoViewModel;
            if (model == null)
            {
                model = new MotivoCancelamentoViewModel();
                model.Filtro.PaginationParameters.PageSize = this.DefaultPageSize;
                model.Filtro.PaginationParameters.UniqueIdentifier = GenerateUniqueIdentifier();
            }

            model.PodeSalvar = motivoCancelamentoAppService.EhPermitidoSalvar();
            model.PodeDeletar = motivoCancelamentoAppService.EhPermitidoDeletar();
            model.PodeImprimir = motivoCancelamentoAppService.EhPermitidoImprimir();

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
                Session["Filtro"] = model;
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

        public ActionResult CarregarItem(int? id)
        {
            var motivoCancelamento = motivoCancelamentoAppService.ObterPeloId(id) ?? new MotivoCancelamentoDTO();
            return Json(motivoCancelamento);
        }

        [HttpPost]
        public ActionResult Salvar(MotivoCancelamentoViewModel model)
        {
            if (ModelState.IsValid)
            {
                motivoCancelamentoAppService.Salvar(model.MotivoCancelamento);
            }

            return PartialView("_NotificationMessagesPartial");
        }

        [HttpPost]
        public ActionResult Deletar(int? id)
        {
            motivoCancelamentoAppService.Deletar(id);
            return PartialView("_NotificationMessagesPartial");
        }

        public ActionResult Imprimir(FormatoExportacaoArquivo formato)
        {
            var arquivo = motivoCancelamentoAppService.ExportarRelMotivoCancelamento(formato);
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
