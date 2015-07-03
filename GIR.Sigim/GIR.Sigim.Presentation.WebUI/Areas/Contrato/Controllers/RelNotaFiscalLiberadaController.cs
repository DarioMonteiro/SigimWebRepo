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

            CarregarCombosFiltro(model);

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

        public ActionResult Imprimir(string dataInicial,
                                     string dataFinal,
                                     int? contratoId,
                                     int? fornecedorClienteId,
                                     string documento,
                                     string codigoCentroCusto,
                                     FormatoExportacaoArquivo formato)
        {

            DateTime dtInicial;
            DateTime dtFinal;
            
            dtInicial =  DateTime.Parse(dataInicial);
            dtFinal = DateTime.Parse(dataFinal);

            var arquivo = contratoRetificacaoItemMedicaoAppService.ExportarRelNotaFiscalLiberada(dtInicial, 
                                                                                                 dtFinal, 
                                                                                                 contratoId, 
                                                                                                 fornecedorClienteId, 
                                                                                                 documento,
                                                                                                 codigoCentroCusto, 
                                                                                                 Usuario.Id, 
                                                                                                 formato);
            if (arquivo != null)
            {
                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();
                return File(arquivo.Stream, arquivo.ContentType, arquivo.NomeComExtensao);
            }

            return PartialView("_NotificationMessagesPartial");
        }


        private void CarregarCombosFiltro(RelNotaFiscalLiberadaListaViewModel model)
        {
            int? fornecedorClienteId = null;

            if (model.Filtro != null)
            {
                fornecedorClienteId = model.Filtro.FornecedorClienteId;
            }

            model.ListaFornecedorCliente = new SelectList(clienteFornecedorAppService.ListarAtivosDeContrato(), "Id", "Nome", fornecedorClienteId);

        }

    }
}
