using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GIR.Sigim.Application.DTO.OrdemCompra;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Application.Filtros;
using GIR.Sigim.Application.Service.OrdemCompra;
using GIR.Sigim.Application.Service.Sigim;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Presentation.WebUI.Areas.OrdemCompra.ViewModel;
using GIR.Sigim.Presentation.WebUI.Controllers;
using GIR.Sigim.Presentation.WebUI.ViewModel;
using Newtonsoft.Json;

namespace GIR.Sigim.Presentation.WebUI.Areas.OrdemCompra.Controllers
{
    public class EntradaMaterialController : BaseController
    {
        private IEntradaMaterialAppService entradaMaterialAppService;

        public EntradaMaterialController(
            IEntradaMaterialAppService entradaMaterialAppService,
            MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.entradaMaterialAppService = entradaMaterialAppService;
        }

        public ActionResult Index()
        {
            var model = Session["Filtro"] as EntradaMaterialListaViewModel;
            if (model == null)
            {
                model = new EntradaMaterialListaViewModel();
                model.Filtro.PaginationParameters.PageSize = this.DefaultPageSize;
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Lista(EntradaMaterialListaViewModel model)
        {
            if (ModelState.IsValid)
            {
                Session["Filtro"] = model;
                int totalRegistros;

                if (string.IsNullOrEmpty(model.Filtro.PaginationParameters.OrderBy))
                    model.Filtro.PaginationParameters.OrderBy = "id";

                var result = entradaMaterialAppService.ListarPeloFiltro(model.Filtro, out totalRegistros);
                if (result.Any())
                {
                    var listaViewModel = CreateListaViewModel(model.Filtro.PaginationParameters, totalRegistros, result);
                    return PartialView("ListaPartial", listaViewModel);
                }
                return PartialView("_EmptyListPartial");
            }
            return PartialView("_NotificationMessagesPartial");
        }

        public ActionResult Cadastro(int? id)
        {
            EntradaMaterialCadastroViewModel model = new EntradaMaterialCadastroViewModel();
            var entradaMaterial = entradaMaterialAppService.ObterPeloId(id) ?? new EntradaMaterialDTO();

            if (id.HasValue && !entradaMaterial.Id.HasValue)
                messageQueue.Add(Application.Resource.Sigim.ErrorMessages.NenhumRegistroEncontrado, TypeMessage.Error);

            model.EntradaMaterial = entradaMaterial;
            //model.JsonItens = JsonConvert.SerializeObject(entradaMaterial.ListaItens);

            //if ((entradaMaterial.CentroCusto == null) || (string.IsNullOrEmpty(entradaMaterial.CentroCusto.Codigo)))
            //{
            //    var parametrosUsuario = parametrosUsuarioAppService.ObterPeloIdUsuario(Usuario.Id);
            //    model.RequisicaoMaterial.CentroCusto = parametrosUsuario.CentroCusto;
            //}

            //var parametros = parametrosOrdemCompraAppService.Obter();
            //if (parametros != null)
            //{
            //    model.DataMinima = parametros.DiasDataMinima.HasValue ? DateTime.Now.AddDays(parametros.DiasDataMinima.Value) : DateTime.Now;
            //    model.DataMaxima = parametros.DiasPrazo.HasValue ? model.DataMinima.Value.AddDays(parametros.DiasPrazo.Value) : DateTime.Now;
            //    model.Prazo = parametros.DiasPrazo.HasValue ? parametros.DiasPrazo.Value : 0;
            //}

            model.PodeSalvar = entradaMaterialAppService.EhPermitidoSalvar(entradaMaterial);
            model.PodeCancelarEntrada = entradaMaterialAppService.EhPermitidoCancelar(entradaMaterial);
            model.ExisteMovimentoNoEstoque = entradaMaterialAppService.ExisteMovimentoNoEstoque(entradaMaterial);
            model.PodeImprimir = entradaMaterialAppService.EhPermitidoImprimir(entradaMaterial);
            //model.PodeAdicionarItem = requisicaoMaterialAppService.EhPermitidoAdicionarItem(entradaMaterial);
            //model.PodeCancelarItem = requisicaoMaterialAppService.EhPermitidoCancelarItem(entradaMaterial);
            //model.PodeEditarItem = requisicaoMaterialAppService.EhPermitidoEditarItem(entradaMaterial);
            //model.PodeAprovarRequisicao = requisicaoMaterialAppService.EhPermitidoAprovarRequisicao(entradaMaterial);
            //model.PodeCancelarAprovacao = requisicaoMaterialAppService.EhPermitidoCancelarAprovacao(entradaMaterial);
            //model.PodeEditarCentroCusto = requisicaoMaterialAppService.EhPermitidoEditarCentroCusto(entradaMaterial);

            return View(model);
        }

        [HttpPost]
        public ActionResult HaPossibilidadeCancelamentoEntradaMaterial(int? entradaMaterialId)
        {
            PossibilidadeCancelamentoEntradaMaterialViewModel model = new PossibilidadeCancelamentoEntradaMaterialViewModel();
            model.HaPossibilidadeCancelamentoEntradaMaterial = entradaMaterialAppService.HaPossibilidadeCancelamentoEntradaMaterial(entradaMaterialId);
            model.ErrorMessages = messageQueue.GetAll().Where(l => l.Type == TypeMessage.Error).ToList();
            messageQueue.Clear();
            return Json(model);
        }

        [HttpPost]
        public ActionResult Cancelar(int? id, string motivo)
        {
            if (entradaMaterialAppService.CancelarEntrada(id, motivo))
                return PartialView("Redirect", Url.Action("Cadastro", "EntradaMaterial", new { id = id }));

            return PartialView("_NotificationMessagesPartial");
        }

        public ActionResult Imprimir(int? id, FormatoExportacaoArquivo formato)
        {
            var arquivo = entradaMaterialAppService.Exportar(id, formato);
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