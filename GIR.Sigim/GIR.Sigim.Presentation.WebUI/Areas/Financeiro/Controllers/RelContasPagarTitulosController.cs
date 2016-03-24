using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using GIR.Sigim.Presentation.WebUI.Controllers;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Application.Constantes;
using GIR.Sigim.Presentation.WebUI.Areas.Financeiro.ViewModel;
using GIR.Sigim.Application.Service.Financeiro;
using GIR.Sigim.Application.Service.Sigim;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Application.Adapter;
using GIR.Sigim.Application.Enums;


namespace GIR.Sigim.Presentation.WebUI.Areas.Financeiro.Controllers
{
    public class RelContasPagarTitulosController : BaseController
    {
        private ITipoCompromissoAppService tipoCompromissoAppService;
        private IBancoAppService bancoAppService;
        private IContaCorrenteAppService contaCorrenteAppService;
        private ICaixaAppService caixaAppService;
        private ITituloPagarAppService tituloPagarAppService;


        public RelContasPagarTitulosController(ITipoCompromissoAppService tipoCompromissoAppService,
                                                IBancoAppService bancoAppService,
                                                IContaCorrenteAppService contaCorrenteAppService,
                                                ICaixaAppService caixaAppService,
                                                ITituloPagarAppService tituloPagarAppService,
                                                MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.tipoCompromissoAppService = tipoCompromissoAppService;
            this.bancoAppService = bancoAppService;
            this.contaCorrenteAppService = contaCorrenteAppService;
            this.caixaAppService = caixaAppService;
            this.tituloPagarAppService = tituloPagarAppService;
        }


        [Authorize(Roles = Funcionalidade.RelatorioContasAPagarTitulosAcessar)]
        public ActionResult Index()
        {
            var model = Session["Filtro"] as RelContasPagarTitulosListaViewModel;
            if (model == null)
            {
                model = new RelContasPagarTitulosListaViewModel();
                model.Filtro.PaginationParameters.PageSize = this.DefaultPageSize;
                model.Filtro.PaginationParameters.UniqueIdentifier = GenerateUniqueIdentifier();
                model.Filtro.DataInicial = DateTime.Now;
                model.Filtro.DataFinal = DateTime.Now;
            }

            model.PodeImprimir = tituloPagarAppService.EhPermitidoImprimirRelContasPagarTitulo();

            CarregarListas(model);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Lista(RelContasPagarTitulosListaViewModel model)
        {
            if (ModelState.IsValid)
            {
                Session["Filtro"] = model;
                int totalRegistros;

                if (string.IsNullOrEmpty(model.Filtro.PaginationParameters.OrderBy))
                    model.Filtro.PaginationParameters.OrderBy = "tituloId";

                var result = tituloPagarAppService.ListarPeloFiltroRelContasPagarTitulos(model.Filtro, Usuario.Id, out totalRegistros);

                if (result.Any())
                {
                    var listaViewModel = CreateListaViewModel(model.Filtro.PaginationParameters, totalRegistros, result);
                    return PartialView("ListaPartial", listaViewModel);
                }
                return PartialView("_EmptyListPartial");
            }
            return PartialView("_NotificationMessagesPartial");
        }

        private void CarregarListas(RelContasPagarTitulosListaViewModel model)
        {
            model.ListaTipoCompromisso = new SelectList(tipoCompromissoAppService.ListarTipoPagar().OrderBy(l => l.Descricao), "Id", "Descricao", model.Filtro.TipoCompromissoId);
            model.ListaFormaPagamento = new SelectList(typeof(FormaPagamento).ToItemListaDTO(), "Id", "Descricao");
            List<BancoDTO> listaBanco = bancoAppService.ListarTodosComContaCorrenteAtiva().OrderBy(l => l.Nome).ToList();
            model.ListaBanco = new SelectList(listaBanco, "Id", "Nome", model.Filtro.BancoId);
            List<ContaCorrenteDTO> listaContaCorrente = new List<ContaCorrenteDTO>();
            model.ListaAgenciaConta = new SelectList(listaContaCorrente, "Id", "AgenciaContaCorrente", model.Filtro.BancoId);
            model.ListaCaixa = new SelectList(caixaAppService.ListarCaixaAtivo(), "Id", "Descricao", model.Filtro.CaixaId);
        }

        [HttpPost]
        public ActionResult CarregaComboAgenciaContaCorrente(int? bancoId)
        {
            List<ContaCorrenteDTO> listaContaCorrente = null;
            if (bancoId.HasValue)
            {
                listaContaCorrente = contaCorrenteAppService.ListarAtivosPorBanco(bancoId.Value).ToList();
            }
            return Json(new
            {
                listaContaCorrente = JsonConvert.SerializeObject(listaContaCorrente, Formatting.Indented,
                                      new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore})
            });
        }

    }
}
