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
    public class TipoDocumentoController : BaseController
    {
        private ITipoDocumentoAppService tipoDocumentoAppService;

        public TipoDocumentoController(
            ITipoDocumentoAppService tipoDocumentoAppService,
            MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.tipoDocumentoAppService = tipoDocumentoAppService;
        }

        [Authorize(Roles = Funcionalidade.TipoDocumentoAcessar)]
        public ActionResult Index(int? id)
        {
            var model = Session["Filtro"] as TipoDocumentoViewModel;
            if (model == null)
            {
                model = new TipoDocumentoViewModel();
                model.Filtro.PaginationParameters.PageSize = this.DefaultPageSize;
                model.Filtro.PaginationParameters.UniqueIdentifier = GenerateUniqueIdentifier();
            }

            model.PodeSalvar = tipoDocumentoAppService.EhPermitidoSalvar();
            model.PodeDeletar = tipoDocumentoAppService.EhPermitidoDeletar();
            model.PodeImprimir = tipoDocumentoAppService.EhPermitidoImprimir();

            var tipoDocumento = tipoDocumentoAppService.ObterPeloId(id) ?? new TipoDocumentoDTO();

            if (id.HasValue && !tipoDocumento.Id.HasValue)
                messageQueue.Add(Application.Resource.Sigim.ErrorMessages.NenhumRegistroEncontrado, TypeMessage.Error);

            model.TipoDocumento = tipoDocumento;

            return View(model);
        }

        public ActionResult CarregarItem(int? id)
        {
            var tipoCompromisso = tipoDocumentoAppService.ObterPeloId(id) ?? new TipoDocumentoDTO();
            return Json(tipoCompromisso);
        }

        [HttpPost]
        public ActionResult Salvar(TipoDocumentoViewModel model)
        {
            if (ModelState.IsValid)
                tipoDocumentoAppService.Salvar(model.TipoDocumento);

            return PartialView("_NotificationMessagesPartial");
        }

        public ActionResult Lista(TipoDocumentoViewModel model)
        {
            if (ModelState.IsValid)
            {
                Session["Filtro"] = model;

                if (string.IsNullOrEmpty(model.Filtro.PaginationParameters.OrderBy))
                    model.Filtro.PaginationParameters.OrderBy = "sigla";

                int totalRegistros;
                var result = tipoDocumentoAppService.ListarPeloFiltro(model.Filtro, out totalRegistros);
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
        public ActionResult Deletar(int? id)
        {
            tipoDocumentoAppService.Deletar(id);
            return PartialView("_NotificationMessagesPartial");
        }

        public ActionResult Imprimir(FormatoExportacaoArquivo formato)
        {
            var arquivo = tipoDocumentoAppService.ExportarRelTipoDocumento(formato);
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
