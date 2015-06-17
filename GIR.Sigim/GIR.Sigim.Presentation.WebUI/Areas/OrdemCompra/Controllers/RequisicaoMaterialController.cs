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
    public class RequisicaoMaterialController : BaseController
    {
        private IRequisicaoMaterialAppService requisicaoMaterialAppService;
        private IMaterialAppService materialAppService;
        private IParametrosOrdemCompraAppService parametrosOrdemCompraAppService;
        private IParametrosUsuarioAppService parametrosUsuarioAppService;

        public RequisicaoMaterialController(IRequisicaoMaterialAppService requisicaoMaterialAppService,
            IMaterialAppService materialAppService,
            IParametrosOrdemCompraAppService parametrosOrdemCompraAppService,
            IParametrosUsuarioAppService parametrosUsuarioAppService,
            MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.requisicaoMaterialAppService = requisicaoMaterialAppService;
            this.materialAppService = materialAppService;
            this.parametrosOrdemCompraAppService = parametrosOrdemCompraAppService;
            this.parametrosUsuarioAppService = parametrosUsuarioAppService;
        }

        public ActionResult Index()
        {
            var model = Session["Filtro"] as RequisicaoMaterialListaViewModel;
            if (model == null)
            {
                model = new RequisicaoMaterialListaViewModel();
                model.Filtro.PaginationParameters.PageSize = this.DefaultPageSize;
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Lista(RequisicaoMaterialListaViewModel model)
        {
            if (ModelState.IsValid)
            {
                Session["Filtro"] = model;
                int totalRegistros;

                if (string.IsNullOrEmpty(model.Filtro.PaginationParameters.OrderBy))
                    model.Filtro.PaginationParameters.OrderBy = "id";

                var result = requisicaoMaterialAppService.ListarPeloFiltro(model.Filtro, Usuario.Id, out totalRegistros);
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
            RequisicaoMaterialCadastroViewModel model = new RequisicaoMaterialCadastroViewModel();
            var requisicaoMaterial = requisicaoMaterialAppService.ObterPeloId(id) ?? new RequisicaoMaterialDTO();

            if (id.HasValue && !requisicaoMaterial.Id.HasValue)
                messageQueue.Add(Application.Resource.Sigim.ErrorMessages.NenhumRegistroEncontrado, TypeMessage.Error);

            model.RequisicaoMaterial = requisicaoMaterial;
            model.JsonItens = JsonConvert.SerializeObject(requisicaoMaterial.ListaItens);

            if ((requisicaoMaterial.CentroCusto == null) || (string.IsNullOrEmpty(requisicaoMaterial.CentroCusto.Codigo)))
            {
                var parametrosUsuario = parametrosUsuarioAppService.ObterPeloIdUsuario(Usuario.Id);
                model.RequisicaoMaterial.CentroCusto = parametrosUsuario.CentroCusto;
            }

            var parametros = parametrosOrdemCompraAppService.Obter();
            if (parametros != null)
            {
                model.DataMinima = parametros.DiasDataMinima.HasValue ? DateTime.Now.AddDays(parametros.DiasDataMinima.Value) : DateTime.Now;
                model.DataMaxima = parametros.DiasPrazo.HasValue ? model.DataMinima.Value.AddDays(parametros.DiasPrazo.Value) : DateTime.Now;
                model.Prazo = parametros.DiasPrazo.HasValue ? parametros.DiasPrazo.Value : 0;
            }

            model.PodeSalvar = requisicaoMaterialAppService.EhPermitidoSalvar(requisicaoMaterial);
            model.PodeCancelarRequisicao = requisicaoMaterialAppService.EhPermitidoCancelar(requisicaoMaterial);
            model.PodeImprimir = requisicaoMaterialAppService.EhPermitidoImprimir(requisicaoMaterial);
            model.PodeAdicionarItem = requisicaoMaterialAppService.EhPermitidoAdicionarItem(requisicaoMaterial);
            model.PodeCancelarItem = requisicaoMaterialAppService.EhPermitidoCancelarItem(requisicaoMaterial);
            model.PodeEditarItem = requisicaoMaterialAppService.EhPermitidoEditarItem(requisicaoMaterial);
            model.PodeAprovarRequisicao = requisicaoMaterialAppService.EhPermitidoAprovarRequisicao(requisicaoMaterial);
            model.PodeCancelarAprovacao = requisicaoMaterialAppService.EhPermitidoCancelarAprovacao(requisicaoMaterial);
            model.PodeEditarCentroCusto = requisicaoMaterialAppService.EhPermitidoEditarCentroCusto(requisicaoMaterial);
            
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastro(RequisicaoMaterialCadastroViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.RequisicaoMaterial.ListaItens = Newtonsoft.Json.JsonConvert.DeserializeObject<List<RequisicaoMaterialItemDTO>>(model.JsonItens);
                if (requisicaoMaterialAppService.Salvar(model.RequisicaoMaterial))
                    return PartialView("Redirect", Url.Action("Cadastro", "RequisicaoMaterial", new { id = model.RequisicaoMaterial.Id }));
            }

            return PartialView("_NotificationMessagesPartial");
        }

        [HttpPost]
        public ActionResult Aprovar(int? id)
        {
            if (requisicaoMaterialAppService.Aprovar(id))
                return PartialView("Redirect", Url.Action("Cadastro", "RequisicaoMaterial", new { id = id }));

            return PartialView("_NotificationMessagesPartial");
        }

        [HttpPost]
        public ActionResult CancelarAprovacao(int? id)
        {
            if (requisicaoMaterialAppService.CancelarAprovacao(id))
                return PartialView("Redirect", Url.Action("Cadastro", "RequisicaoMaterial", new { id = id }));

            return PartialView("_NotificationMessagesPartial");
        }

        [HttpPost]
        public ActionResult Cancelar(int? id, string motivo)
        {
            if (requisicaoMaterialAppService.CancelarRequisicao(id, motivo))
                return PartialView("Redirect", Url.Action("Cadastro", "RequisicaoMaterial", new { id = id }));

            return PartialView("_NotificationMessagesPartial");
        }

        public ActionResult Imprimir(int? id, FormatoExportacaoArquivo formato)
        {
            var arquivo = requisicaoMaterialAppService.Exportar(id, formato);
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