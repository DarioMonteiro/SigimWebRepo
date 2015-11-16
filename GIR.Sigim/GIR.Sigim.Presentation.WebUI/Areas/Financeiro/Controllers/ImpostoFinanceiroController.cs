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
    public class ImpostoFinanceiroController : BaseController
    {
        private IImpostoFinanceiroAppService impostoFinanceiroAppService;
        private IClienteFornecedorAppService clienteFornecedorAppService;
        private ITipoCompromissoAppService tipoCompromissoAppService;

        public ImpostoFinanceiroController(
            IImpostoFinanceiroAppService impostoFinanceiroAppService,
            IClienteFornecedorAppService clienteFornecedorAppService,
            ITipoCompromissoAppService tipoCompromissoAppService,
            MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.impostoFinanceiroAppService = impostoFinanceiroAppService;
            this.clienteFornecedorAppService = clienteFornecedorAppService;
            this.tipoCompromissoAppService = tipoCompromissoAppService;
        }

        [Authorize(Roles = Funcionalidade.ImpostoFinanceiroAcessar)]
        public ActionResult Index(int? id)
        {
            var model = Session["Filtro"] as ImpostoFinanceiroViewModel;
            if (model == null)
            {
                model = new ImpostoFinanceiroViewModel();
                model.Filtro.PaginationParameters.PageSize = this.DefaultPageSize;
                model.Filtro.PaginationParameters.UniqueIdentifier = GenerateUniqueIdentifier();
            }

            model.PodeSalvar = impostoFinanceiroAppService.EhPermitidoSalvar();
            model.PodeDeletar = impostoFinanceiroAppService.EhPermitidoDeletar();
            model.PodeImprimir = impostoFinanceiroAppService.EhPermitidoImprimir();

            var impostoFinanceiro = impostoFinanceiroAppService.ObterPeloId(id) ?? new ImpostoFinanceiroDTO();

            if (id.HasValue && !impostoFinanceiro.Id.HasValue)
                messageQueue.Add(Application.Resource.Sigim.ErrorMessages.NenhumRegistroEncontrado, TypeMessage.Error);

            model.ImpostoFinanceiro = impostoFinanceiro;
            CarregarListas(model);
            return View(model);
        }

        public ActionResult CarregarItem(int? id)
        {
            var impostoFinanceiro = impostoFinanceiroAppService.ObterPeloId(id) ?? new ImpostoFinanceiroDTO();
            return Json(impostoFinanceiro);
        }

        [HttpPost]
        public ActionResult Salvar(ImpostoFinanceiroViewModel model)
        {
            if (ModelState.IsValid)
                impostoFinanceiroAppService.Salvar(model.ImpostoFinanceiro);

            return PartialView("_NotificationMessagesPartial");
        }

        public ActionResult Lista(ImpostoFinanceiroViewModel model)
        {
            if (ModelState.IsValid)
            {
                Session["Filtro"] = model;
                int totalRegistros;
                var result = impostoFinanceiroAppService.ListarPeloFiltro(model.Filtro, out totalRegistros);
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
            impostoFinanceiroAppService.Deletar(id);
            return PartialView("_NotificationMessagesPartial");
        }

        private void CarregarListas(ImpostoFinanceiroViewModel model)
        {
            model.ListaCorrentista = new SelectList(clienteFornecedorAppService.ListarAtivos(), "Id", "Nome", model.ImpostoFinanceiro.ClienteId);
            model.ListaTipoCompromisso = new SelectList(tipoCompromissoAppService.ListarTipoPagar(), "Id", "Descricao", model.ImpostoFinanceiro.TipoCompromissoId );
            model.ListaOpcoesPeriodicidade = new SelectList(impostoFinanceiroAppService.ListarOpcoesPeriodicidade(), "Id", "Descricao", model.ImpostoFinanceiro.Periodicidade);
            model.ListarOpcoesFimDeSemana = new SelectList(impostoFinanceiroAppService.ListarOpcoesFimDeSemana(), "Id", "Descricao", model.ImpostoFinanceiro.FimDeSemana);
            model.ListarOpcoesFatoGerador = new SelectList(impostoFinanceiroAppService.ListarOpcoesFatoGerador(), "Id", "Descricao", model.ImpostoFinanceiro.FatoGerador);
        }

        public ActionResult Imprimir(FormatoExportacaoArquivo formato)
        {
            var arquivo = impostoFinanceiroAppService.ExportarRelImpostoFinanceiro(formato);
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
