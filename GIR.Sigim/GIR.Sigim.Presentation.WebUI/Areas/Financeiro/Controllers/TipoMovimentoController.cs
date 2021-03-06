﻿using System;
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
using GIR.Sigim.Presentation.WebUI.CustomAttributes;

namespace GIR.Sigim.Presentation.WebUI.Areas.Financeiro.Controllers
{
    public class TipoMovimentoController : BaseController
    {
        private ITipoMovimentoAppService tipoMovimentoAppService;
        private IHistoricoContabilAppService historicoContabilAppService;

        public TipoMovimentoController(ITipoMovimentoAppService tipoMovimentoAppService,
                                       IHistoricoContabilAppService historicoContabilAppService, 
                                       MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.tipoMovimentoAppService = tipoMovimentoAppService;
            this.historicoContabilAppService = historicoContabilAppService;
        }

        [AutorizacaoAcessoAuthorize(GIR.Sigim.Application.Constantes.Modulo.FinanceiroWeb, Roles = Funcionalidade.TipoMovimentoAcessar)]
        public ActionResult Index(int? id)
        {
            var model = Session["Filtro"] as TipoMovimentoViewModel;
            if (model == null)
            {
                model = new TipoMovimentoViewModel();
                model.Filtro.PaginationParameters.PageSize = this.DefaultPageSize;
                model.Filtro.PaginationParameters.UniqueIdentifier = GenerateUniqueIdentifier();
            }
            var tipoMovimento = tipoMovimentoAppService.ObterPeloId(id) ?? new TipoMovimentoDTO();

            if (id.HasValue && !tipoMovimento.Id.HasValue)
                messageQueue.Add(Application.Resource.Sigim.ErrorMessages.NenhumRegistroEncontrado, TypeMessage.Error);

            model.TipoMovimento = tipoMovimento;

            model.TipoMovimento.Tipo = "B";
            model.TipoMovimento.Operacao = "C";

            model.PodeSalvar = tipoMovimentoAppService.EhPermitidoSalvar();
            model.PodeDeletar = tipoMovimentoAppService.EhPermitidoDeletar();
            model.PodeImprimir = tipoMovimentoAppService.EhPermitidoImprimir();


            CarregarCombos(model);

            return View(model);
        }

        public ActionResult CarregarItem(int? id)
        {
            var tipoMovimento = tipoMovimentoAppService.ObterPeloId(id) ?? new TipoMovimentoDTO();
            return Json(tipoMovimento);
        }

        [HttpPost]
        public ActionResult Salvar(TipoMovimentoViewModel model)
        {
            if (ModelState.IsValid)
                tipoMovimentoAppService.Salvar(model.TipoMovimento);

            return PartialView("_NotificationMessagesPartial");
        }

        public ActionResult Lista(TipoMovimentoViewModel model)
        {
            if (ModelState.IsValid)
            {
                Session["Filtro"] = model;
                int totalRegistros;
                var result = tipoMovimentoAppService.ListarNaoAutomatico(model.Filtro, out totalRegistros);
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
            tipoMovimentoAppService.Deletar(id);
            return PartialView("_NotificationMessagesPartial");
        }

        public ActionResult Imprimir(FormatoExportacaoArquivo formato)
        {
            var arquivo = tipoMovimentoAppService.ExportarRelTipoMovimento(formato);
            if (arquivo != null)
            {
                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();
                return File(arquivo.Stream, arquivo.ContentType, arquivo.NomeComExtensao);
            }

            return PartialView("_NotificationMessagesPartial");
        }


        private void CarregarCombos(TipoMovimentoViewModel model)
        {
            model.ListaHistoricoContabil = new SelectList(historicoContabilAppService.ListarPorTipo(HistoricoContabil.TipoMovimento), "Id", "Descricao", model.TipoMovimento.HistoricoContabilId);
        }

    }
}
