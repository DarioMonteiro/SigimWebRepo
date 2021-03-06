﻿using System;
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
    public class UnidadeMedidaController : BaseController
    {
        private IUnidadeMedidaAppService unidadeMedidaAppService;               

        public UnidadeMedidaController(
            IUnidadeMedidaAppService unidadeMedidaAppService,            
            MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.unidadeMedidaAppService = unidadeMedidaAppService;            
        }

        [Authorize(Roles = Funcionalidade.OrdemCompraUnidadeMedidaAcessar)]
        public ActionResult Index(string sigla)
        {
            var model = Session["Filtro"] as UnidadeMedidaViewModel;
            if (model == null)
            {
                model = new UnidadeMedidaViewModel();
                model.Filtro.PaginationParameters.PageSize = this.DefaultPageSize;
                model.Filtro.PaginationParameters.UniqueIdentifier = GenerateUniqueIdentifier();
                model.Filtro.PaginationParameters.OrderBy = "sigla";
            }

            model.PodeSalvar = unidadeMedidaAppService.EhPermitidoSalvar();
            model.PodeDeletar = unidadeMedidaAppService.EhPermitidoDeletar();
            model.PodeImprimir = unidadeMedidaAppService.EhPermitidoImprimir();

            var unidadeMedida = unidadeMedidaAppService.ObterPeloCodigo(sigla);

            if (!string.IsNullOrEmpty(sigla))
                messageQueue.Add(Application.Resource.Sigim.ErrorMessages.NenhumRegistroEncontrado, TypeMessage.Error);

            model.UnidadeMedida = unidadeMedida;

            return View(model);
        }

        public ActionResult Lista(UnidadeMedidaViewModel model)
        {
            if (ModelState.IsValid)
            {
                Session["Filtro"] = model;
                int totalRegistros;
                var result = unidadeMedidaAppService.ListarPeloFiltro(model.Filtro, out totalRegistros);
                if (result.Any())
                {
                    var listaViewModel = CreateListaViewModel(model.Filtro.PaginationParameters, totalRegistros, result);
                    return PartialView("ListaPartial", listaViewModel);
                }
                return PartialView("_EmptyListPartial");
            }
            return PartialView("_NotificationMessagesPartial");
        }

        public ActionResult CarregarItem(string sigla)
        {
            var unidadeMedida = unidadeMedidaAppService.ObterPeloCodigo(sigla) ?? new UnidadeMedidaDTO();
            return Json(unidadeMedida);
        }        

        [HttpPost]
        public ActionResult Salvar(UnidadeMedidaViewModel model)
        {
            if (ModelState.IsValid)
                unidadeMedidaAppService.Salvar(model.UnidadeMedida);

            return PartialView("_NotificationMessagesPartial");
        }

        [HttpPost]
        public ActionResult Deletar(string sigla)
        {
            unidadeMedidaAppService.Deletar(sigla);
            return PartialView("_NotificationMessagesPartial");
        }

        public ActionResult Imprimir(FormatoExportacaoArquivo formato)
        {
            var arquivo = unidadeMedidaAppService.ExportarRelUnidadeMedida(formato);
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