using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GIR.Sigim.Presentation.WebUI.Controllers;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Presentation.WebUI.Areas.Comercial.ViewModel;
using GIR.Sigim.Application.Service.Sigim;
using GIR.Sigim.Application.Service.Comercial;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Application.Constantes;

namespace GIR.Sigim.Presentation.WebUI.Areas.Comercial.Controllers
{
    public class RelStatusVendaController : BaseController
    {
        private IVendaAppService vendaAppService;
        private IIncorporadorAppService incorporadorAppService;
        private IEmpreendimentoAppService empreendimentoAppService;
        private IBlocoAppService blocoAppService;

        public RelStatusVendaController(IVendaAppService vendaAppService,
                                        IIncorporadorAppService incorporadorAppService,
                                        IEmpreendimentoAppService empreendimentoAppService,
                                        IBlocoAppService blocoAppService,
                                        MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.vendaAppService = vendaAppService;
            this.incorporadorAppService = incorporadorAppService;
            this.empreendimentoAppService = empreendimentoAppService;
            this.blocoAppService = blocoAppService;
        }

        [Authorize(Roles = Funcionalidade.RelStatusVendaAcessar)]
        public ActionResult Index()
        {
            var model = Session["Filtro"] as RelStatusVendaViewModel;
            if (model == null)
            {
                model = new RelStatusVendaViewModel();
                model.Filtro.PaginationParameters.PageSize = this.DefaultPageSize;
                model.Filtro.PaginationParameters.UniqueIdentifier = GenerateUniqueIdentifier();
            }

            model.PodeImprimir = vendaAppService.EhPermitidoImprimirRelStatusVenda();

            CarregarCombos(model);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Lista(RelStatusVendaViewModel model)
        {
            if (ModelState.IsValid)
            {
                Session["Filtro"] = model;
                int totalRegistros;

                if (string.IsNullOrEmpty(model.Filtro.PaginationParameters.OrderBy))
                    model.Filtro.PaginationParameters.OrderBy = "contratoId";

                var result = vendaAppService.ListarPeloFiltroRelStatusVenda(model.Filtro, out totalRegistros);

                if (result.Any())
                {
                    var listaViewModel = CreateListaViewModel(model.Filtro.PaginationParameters, totalRegistros, result);
                    return PartialView("ListaPartial", listaViewModel);
                }
                return PartialView("_EmptyListPartial");
            }
            return PartialView("_NotificationMessagesPartial");
        }

        private void CarregarCombos(RelStatusVendaViewModel model)
        {
            model.ListaIncorporador = new SelectList(incorporadorAppService.ListarTodos().OrderBy(l => l.RazaoSocial), "Id", "RazaoSocial", model.Filtro.IncorporadorId);
            model.ListaEmpreendimento = new SelectList(empreendimentoAppService.ListarPeloIncorporador(model.Filtro.IncorporadorId), "Id", "Nome", model.Filtro.EmpreendimentoId);
            model.ListaBloco = new SelectList(blocoAppService.ListarPeloEmpreendimento(model.Filtro.EmpreendimentoId), "Id", "Nome", model.Filtro.BlocoId );
        }

        public ActionResult CarregaEmpreendimentoPorIncorporador(int IncorporadorId)
        {
            var lista = empreendimentoAppService.ListarPeloIncorporador(IncorporadorId);
            return Json(lista);
        }

        public ActionResult CarregaBlocoPorEmpreendimento(int EmpreendimentoId)
        {
            var lista = blocoAppService.ListarPeloEmpreendimento(EmpreendimentoId);
            return Json(lista);
        }

        public ActionResult Imprimir(FormatoExportacaoArquivo formato)
        {
            var model = Session["Filtro"] as RelStatusVendaViewModel;
            if (model == null)
            {
                messageQueue.Add(Application.Resource.Sigim.ErrorMessages.NaoExistemRegistros, TypeMessage.Error);
                return PartialView("_NotificationMessagesPartial");
            }

            //var arquivo = ordemCompraItemAppService.ExportarRelOCItensOrdemCompra(model.Filtro, Usuario.Id, formato);
            //if (arquivo != null)
            //{
            //    Response.Buffer = false;
            //    Response.ClearContent();
            //    Response.ClearHeaders();
            //    return File(arquivo.Stream, arquivo.ContentType, arquivo.NomeComExtensao);
            //}

            return PartialView("_NotificationMessagesPartial");
        }

    }
}
