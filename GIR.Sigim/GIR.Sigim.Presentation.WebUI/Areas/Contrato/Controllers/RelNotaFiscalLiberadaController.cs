using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GIR.Sigim.Presentation.WebUI.Controllers;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Presentation.WebUI.Areas.Contrato.ViewModel;
using GIR.Sigim.Application.Service.Sigim;
using GIR.Sigim.Application.Service.Contrato;
using GIR.Sigim.Application.DTO.Sigim;

namespace GIR.Sigim.Presentation.WebUI.Areas.Contrato.Controllers
{
    public class RelNotaFiscalLiberadaController : BaseController
    {
        private IClienteFornecedorAppService clienteFornecedorAppService;
        private IContratoAppService contratoAppService;
        private IContratoRetificacaoItemMedicaoAppService contratoRetificacaoItemMedicaoAppService;

        public RelNotaFiscalLiberadaController( IClienteFornecedorAppService clienteFornecedorAppService,
                                                IContratoAppService contratoAppService,
                                                IContratoRetificacaoItemMedicaoAppService contratoRetificacaoItemMedicaoAppService,
                                                MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.clienteFornecedorAppService = clienteFornecedorAppService;
            this.contratoAppService = contratoAppService;
            this.contratoRetificacaoItemMedicaoAppService = contratoRetificacaoItemMedicaoAppService;
        }

        public ActionResult Index()
        {
            var model = Session["Filtro"] as RelNotaFiscalLiberadaListaViewModel;
            if (model == null)
            {
                model = new RelNotaFiscalLiberadaListaViewModel();
                model.Filtro.PaginationParameters.PageSize = this.DefaultPageSize;
                model.Filtro.DataInicial = DateTime.Now;
                model.Filtro.DataFinal = DateTime.Now;
            }

            model.PodeImprimir = true;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Lista(RelNotaFiscalLiberadaListaViewModel model)
        {
            if (ModelState.IsValid)
            {
                Session["Filtro"] = model;
                int totalRegistros;

                if (string.IsNullOrEmpty(model.Filtro.PaginationParameters.OrderBy))
                    model.Filtro.PaginationParameters.OrderBy = "numeroDocumento";

                var result = contratoRetificacaoItemMedicaoAppService.ListarPeloFiltroRelNotaFiscalLiberada(model.Filtro, Usuario.Id, out totalRegistros);

                if (result.Any())
                {
                    var listaViewModel = CreateListaViewModel(model.Filtro.PaginationParameters, totalRegistros, result);
                    return PartialView("ListaPartial", listaViewModel);
                }
                return PartialView("_EmptyListPartial");
            }
            return PartialView("_NotificationMessagesPartial");
        }

        public ActionResult Imprimir(FormatoExportacaoArquivo formato)
        {
            var model = Session["Filtro"] as RelNotaFiscalLiberadaListaViewModel;
            if (model == null)
            {
                messageQueue.Add(Application.Resource.Sigim.ErrorMessages.NaoExistemRegistros, TypeMessage.Error);
                return PartialView("_NotificationMessagesPartial");
            }

            var arquivo = contratoRetificacaoItemMedicaoAppService.ExportarRelNotaFiscalLiberada(model.Filtro, Usuario.Id,formato);
            if (arquivo != null)
            {
                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();
                return File(arquivo.Stream, arquivo.ContentType, arquivo.NomeComExtensao);
            }

            return PartialView("_NotificationMessagesPartial");
        }

        [HttpPost]
        public ActionResult RecuperaDescricaoContrato(int? contratoId)
        {
            if (contratoId.HasValue)
            {
                GIR.Sigim.Application.DTO.Contrato.ContratoDTO contrato = null;

                contrato = contratoAppService.ObterPeloId(contratoId,Usuario.Id);

                if (contrato != null)
                {
                    return Json(new
                    {
                        errorMessage = string.Empty,
                        descricaoContrato = contrato.ContratoDescricao.Descricao
                    });
                }
            }
            return Json(new
            {
                errorMessage = string.Empty
            });

        }
    }
}
