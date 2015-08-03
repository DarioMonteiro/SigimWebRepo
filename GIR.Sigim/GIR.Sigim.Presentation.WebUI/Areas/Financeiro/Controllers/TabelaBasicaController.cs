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
using GIR.Sigim.Application.Adapter;
using GIR.Sigim.Application.Enums;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Presentation.WebUI.Areas.Financeiro.ViewModel;
using GIR.Sigim.Presentation.WebUI.Controllers;
using GIR.Sigim.Application.Filtros;
using GIR.Sigim.Application.Constantes;

namespace GIR.Sigim.Presentation.WebUI.Areas.Financeiro.Controllers
{
    public class TabelaBasicaController : BaseController
    {
        private ITabelaBasicaAppService tabelaBasicaAppService;

        public TabelaBasicaController(
            ITabelaBasicaAppService tabelaBasicaAppService,
            MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.tabelaBasicaAppService = tabelaBasicaAppService;
        }

        [Authorize(Roles = Funcionalidade.TabelaBasicaFinanceiroAcessar)]
        public ActionResult Index(int? id)
        {
            TabelaBasicaDTO tabelaBasica = new TabelaBasicaDTO();
            int tipoTabela = 0;

            var model = Session["Filtro"] as TabelaBasicaViewModel;

            if (model == null)
            {
                tipoTabela = 0;
                model = new TabelaBasicaViewModel();
                model.Filtro.PaginationParameters.PageSize = this.DefaultPageSize;
            }

            if (model.TipoTabelaId != null) { tipoTabela = (int)model.TipoTabelaId; }
                                    
            tabelaBasica = tabelaBasicaAppService.ObterPeloId(id, tipoTabela) ?? new TabelaBasicaDTO();

            if (id.HasValue && !tabelaBasica.Id.HasValue)
                messageQueue.Add(Application.Resource.Sigim.ErrorMessages.NenhumRegistroEncontrado, TypeMessage.Error);

            model.TabelaBasica = tabelaBasica;

            CarregarCombos(model);
            model.TabelaBasica.TipoTabela = tipoTabela;

            return View(model);
        }

        public ActionResult CarregarItem(int? id, int tipoTabela)
        {
            var tabelaBasica = tabelaBasicaAppService.ObterPeloId(id, tipoTabela) ?? new TabelaBasicaDTO();
            return Json(tabelaBasica);
        }

        [HttpPost]
        public ActionResult Salvar(TabelaBasicaViewModel model)
        {
            if (ModelState.IsValid)
                tabelaBasicaAppService.Salvar(model.TabelaBasica);

            return PartialView("_NotificationMessagesPartial");
        }

        public ActionResult Lista(TabelaBasicaViewModel model)
        {
            if (ModelState.IsValid)
            {
                Session["Filtro"] = model;

                int tipoTabela = 0;

                if (model == null)
                {
                    model = new TabelaBasicaViewModel();
                    model.Filtro.PaginationParameters.PageSize = this.DefaultPageSize;
                }

                if (model.TipoTabelaId != null) { tipoTabela = (int)model.TipoTabelaId; }

                int totalRegistros;
                var result = tabelaBasicaAppService.ListarPeloFiltro(model.Filtro, out totalRegistros, (int)tipoTabela);
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
        public ActionResult Deletar(int? id, short tipoTabela)
        {
            tabelaBasicaAppService.Deletar(id, tipoTabela);
            return PartialView("_NotificationMessagesPartial");
        }

        private void CarregarCombos(TabelaBasicaViewModel model)
        {
            model.ListaTipoTabela = new SelectList(typeof(TabelaBasicaFinanceiro).ToItemListaDTO(), "Id", "Descricao");
        }

    }
}
