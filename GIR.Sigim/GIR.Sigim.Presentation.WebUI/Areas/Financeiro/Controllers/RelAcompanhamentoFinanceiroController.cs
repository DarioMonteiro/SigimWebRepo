using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Presentation.WebUI.Controllers;
using GIR.Sigim.Presentation.WebUI.CustomAttributes;
using GIR.Sigim.Application.Constantes;
using GIR.Sigim.Presentation.WebUI.Areas.Financeiro.ViewModel;
using Newtonsoft.Json;
using GIR.Sigim.Application.DTO.Financeiro;
using GIR.Sigim.Application.Service.Sigim;
using GIR.Sigim.Application.Service.Financeiro;
using GIR.Sigim.Application.DTO.Sigim;

namespace GIR.Sigim.Presentation.WebUI.Areas.Financeiro.Controllers
{
    public class RelAcompanhamentoFinanceiroController : BaseController
    {
        public IIndiceFinanceiroAppService indiceFinanceiroAppService;
        public IApropriacaoAppService apropriacaoAppService;

        public RelAcompanhamentoFinanceiroController(IIndiceFinanceiroAppService indiceFinanceiroAppService,
                                                     IApropriacaoAppService apropriacaoAppService,
                                                     MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.indiceFinanceiroAppService = indiceFinanceiroAppService;
            this.apropriacaoAppService = apropriacaoAppService;
        }

        #region "Métodos Públicos"

        [AutorizacaoAcessoAuthorize(GIR.Sigim.Application.Constantes.Modulo.FinanceiroWeb, Roles = Funcionalidade.RelatorioAcompanhamentoFinanceiroAcessar)]
        public ActionResult Index()
        {
            Session["Filtro"] = null;

            var model = new RelAcompanhamentoFinanceiroListaViewModel();
            model.Filtro.PaginationParameters.PageSize = this.DefaultPageSize;
            model.Filtro.PaginationParameters.UniqueIdentifier = GenerateUniqueIdentifier();
            model.Filtro.DataInicial = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            model.Filtro.DataFinal = DateTime.Now;

            model.PodeImprimir = apropriacaoAppService.EhPermitidoImprimirRelAcompanhamentoFinanceiro();

            CarregarListas(model);

            model.JsonItensClasse = JsonConvert.SerializeObject(new List<ClasseDTO>(), Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Lista(RelAcompanhamentoFinanceiroListaViewModel model)
        {
            if (ModelState.IsValid)
            {
                Session["Filtro"] = model;

                model.Filtro.ListaClasse = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ClasseDTO>>(model.JsonItensClasse);

                int totalRegistros;
                if (string.IsNullOrEmpty(model.Filtro.PaginationParameters.OrderBy))
                {
                    model.Filtro.PaginationParameters.OrderBy = "classe";
                }

                List<RelAcompanhamentoFinanceiroDTO> listaRelAcompanhamentoFinanceiroDTO;

                listaRelAcompanhamentoFinanceiroDTO = apropriacaoAppService.ListarPeloFiltroRelAcompanhamentoFinanceiro(model.Filtro);

                TempData["listaAcompanhamentoFinanceiroDTO"] = listaRelAcompanhamentoFinanceiroDTO;

                var result = apropriacaoAppService.PaginarPeloFiltroRelAcompanhamentoFinanceiro(model.Filtro, listaRelAcompanhamentoFinanceiroDTO, out totalRegistros);

                if (result.Any())
                {
                    var listaViewModel = CreateListaViewModel(model.Filtro.PaginationParameters, totalRegistros, result);
                    if (model.Filtro.BaseadoPor == 1)
                    {
                        return PartialView("ListaPartialPercentualExecutado", listaViewModel);
                    }
                    return PartialView("ListaPartial", listaViewModel);
                }
                return PartialView("_EmptyListPartial");
            }

            return PartialView("_NotificationMessagesPartial");
        }

        public ActionResult Imprimir(FormatoExportacaoArquivo formato)
        {
            var model = Session["Filtro"] as RelAcompanhamentoFinanceiroListaViewModel;
            if (model == null)
            {
                messageQueue.Add(Application.Resource.Sigim.ErrorMessages.NaoExistemRegistros, TypeMessage.Error);
                return PartialView("_NotificationMessagesPartial");
            }

            List<RelAcompanhamentoFinanceiroDTO> listaRelAcompanhamentoFinanceiroDTO = TempData["listaAcompanhamentoFinanceiroDTO"] as List<RelAcompanhamentoFinanceiroDTO>;
            if (listaRelAcompanhamentoFinanceiroDTO == null)
            {
                listaRelAcompanhamentoFinanceiroDTO = apropriacaoAppService.ListarPeloFiltroRelAcompanhamentoFinanceiro(model.Filtro);
                TempData["listaAcompanhamentoFinanceiroDTO"] = listaRelAcompanhamentoFinanceiroDTO;
            }

            var arquivo = apropriacaoAppService.ExportarRelAcompanhamentoFinanceiro(model.Filtro, listaRelAcompanhamentoFinanceiroDTO, formato);
            if (arquivo != null)
            {
                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();
                return File(arquivo.Stream, arquivo.ContentType, arquivo.NomeComExtensao);
            }

            return PartialView("_NotificationMessagesPartial");

        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Lista(RelAcompanhamentoFinanceiroListaViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        Session["Filtro"] = model;

        //        model.Filtro.ListaClasse = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ClasseDTO>>(model.JsonItensClasse);

        //        int totalRegistros;
        //        if (string.IsNullOrEmpty(model.Filtro.PaginationParameters.OrderBy))
        //        {
        //            model.Filtro.PaginationParameters.OrderBy = "classe";
        //        }

        //        List<RelAcompanhamentoFinanceiroDTO> result;

        //        result = apropriacaoAppService.ListarPeloFiltroRelAcompanhamentoFinanceiro(model.Filtro,
        //                                                                                   out totalRegistros);
        //        if (result.Any())
        //        {
        //            var listaViewModel = CreateListaViewModel(model.Filtro.PaginationParameters, totalRegistros, result);
        //            if (model.Filtro.BaseadoPor == 1) 
        //            {
        //                return PartialView("ListaPartialPercentualExecutado", listaViewModel);
        //            }
        //            return PartialView("ListaPartial", listaViewModel);
        //        }
        //        return PartialView("_EmptyListPartial");
        //    }

        //    return PartialView("_NotificationMessagesPartial");
        //}

        //public ActionResult Imprimir(FormatoExportacaoArquivo formato)
        //{
        //    var model = Session["Filtro"] as RelAcompanhamentoFinanceiroListaViewModel;
        //    if (model == null)
        //    {
        //        messageQueue.Add(Application.Resource.Sigim.ErrorMessages.NaoExistemRegistros, TypeMessage.Error);
        //        return PartialView("_NotificationMessagesPartial");
        //    }

        //    var arquivo = apropriacaoAppService.ExportarRelAcompanhamentoFinanceiro(model.Filtro, formato);
        //    if (arquivo != null)
        //    {
        //        Response.Buffer = false;
        //        Response.ClearContent();
        //        Response.ClearHeaders();
        //        return File(arquivo.Stream, arquivo.ContentType, arquivo.NomeComExtensao);
        //    }

        //    return PartialView("_NotificationMessagesPartial");

        //}

        #endregion


        #region "Métodos Privados"

        private void CarregarListas(RelAcompanhamentoFinanceiroListaViewModel model)
        {
            model.ListaIndice = new SelectList(indiceFinanceiroAppService.ListarTodos().OrderBy(l => l.Indice), "Id", "Indice", model.Filtro.IndiceId);
        }

        #endregion

    }
}
