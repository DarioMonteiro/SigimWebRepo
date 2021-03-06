﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GIR.Sigim.Presentation.WebUI.Controllers;
using GIR.Sigim.Application.Service.Financeiro;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Application.DTO.Financeiro;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Presentation.WebUI.Areas.Financeiro.ViewModel;
using GIR.Sigim.Application.Constantes;
using GIR.Sigim.Presentation.WebUI.CustomAttributes;

namespace GIR.Sigim.Presentation.WebUI.Areas.Financeiro.Controllers
{
    public class TipoRateioController : BaseController
    {

       private ITipoRateioAppService tipoRateioAppService;

       public TipoRateioController(ITipoRateioAppService tipoRateioAppService,
                                   MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.tipoRateioAppService = tipoRateioAppService;
        }

        [AutorizacaoAcessoAuthorize(GIR.Sigim.Application.Constantes.Modulo.FinanceiroWeb, Roles = Funcionalidade.TipoRateioAcessar)]
        public ActionResult Index(int? id)
        {
            var model = Session["Filtro"] as TipoRateioViewModel;
            if (model == null)
            {
                model = new TipoRateioViewModel();
                model.Filtro.PaginationParameters.PageSize = this.DefaultPageSize;
                model.Filtro.PaginationParameters.UniqueIdentifier = GenerateUniqueIdentifier();
                model.Filtro.PaginationParameters.OrderBy = "descricao";
            }

            model.PodeSalvar = tipoRateioAppService.EhPermitidoSalvar();
            model.PodeDeletar = tipoRateioAppService.EhPermitidoDeletar();
            model.PodeImprimir = tipoRateioAppService.EhPermitidoImprimir();

            var tipoRateio = tipoRateioAppService.ObterPeloId(id) ?? new TipoRateioDTO();

            if (id.HasValue && !tipoRateio.Id.HasValue)
                messageQueue.Add(Application.Resource.Sigim.ErrorMessages.NenhumRegistroEncontrado, TypeMessage.Error);

            model.TipoRateio = tipoRateio;

            return View(model);
        }

        public ActionResult CarregarItem(int? id)
        {
            var tipoRateio = tipoRateioAppService.ObterPeloId(id) ?? new TipoRateioDTO();
            return Json(tipoRateio);
        }

        [HttpPost]
        public ActionResult Salvar(TipoRateioViewModel model)
        {
            if (ModelState.IsValid)
                tipoRateioAppService.Salvar(model.TipoRateio);

            return PartialView("_NotificationMessagesPartial");
        }

        public ActionResult Lista(TipoRateioViewModel model)
        {
            if (ModelState.IsValid)
            {
                Session["Filtro"] = model;
                int totalRegistros;
                var result = tipoRateioAppService.ListarPeloFiltro(model.Filtro, out totalRegistros);
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
            tipoRateioAppService.Deletar(id);
            return PartialView("_NotificationMessagesPartial");
        }

        public ActionResult Imprimir(FormatoExportacaoArquivo formato)
        {
            var arquivo = tipoRateioAppService.ExportarRelTipoRateio(formato);
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
