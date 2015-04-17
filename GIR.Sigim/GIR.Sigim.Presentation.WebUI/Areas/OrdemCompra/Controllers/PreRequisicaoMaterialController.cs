using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using GIR.Sigim.Application.DTO.OrdemCompra;
using GIR.Sigim.Application.Service.OrdemCompra;
using GIR.Sigim.Application.Service.Sigim;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Presentation.WebUI.Areas.OrdemCompra.ViewModel;
using GIR.Sigim.Presentation.WebUI.Controllers;
using GIR.Sigim.Presentation.WebUI.ViewModel;

namespace GIR.Sigim.Presentation.WebUI.Areas.OrdemCompra.Controllers
{
    public class PreRequisicaoMaterialController : BaseController
    {
        private IPreRequisicaoMaterialAppService preRequisicaoMaterialAppService;
        private IMaterialAppService materialAppService;
        private IParametrosOrdemCompraAppService parametrosOrdemCompraAppService;

        public PreRequisicaoMaterialController(IPreRequisicaoMaterialAppService preRequisicaoMaterialAppService,
            IMaterialAppService materialAppService,
            IParametrosOrdemCompraAppService parametrosOrdemCompraAppService,
            MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.preRequisicaoMaterialAppService = preRequisicaoMaterialAppService;
            this.materialAppService = materialAppService;
            this.parametrosOrdemCompraAppService = parametrosOrdemCompraAppService;
        }

        public ActionResult Index()
        {
            var model = Session["Filtro"] as PreRequisicaoMaterialListaViewModel;
            if (model == null)
            {
                model = new PreRequisicaoMaterialListaViewModel();
                model.Filtro.PaginationParameters.PageSize = this.DefaultPageSize;
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Lista(PreRequisicaoMaterialListaViewModel model)
        {
            if (ModelState.IsValid)
            {
                Session["Filtro"] = model;
                int totalRegistros;

                if (string.IsNullOrEmpty(model.Filtro.PaginationParameters.OrderBy))
                    model.Filtro.PaginationParameters.OrderBy = "id";

                var result = preRequisicaoMaterialAppService.ListarPeloFiltro(model.Filtro, Usuario.Id, out totalRegistros);
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
            PreRequisicaoMaterialCadastroViewModel model = new PreRequisicaoMaterialCadastroViewModel();
            var preRequisicaoMaterial = preRequisicaoMaterialAppService.ObterPeloId(id, Usuario.Id) ?? new PreRequisicaoMaterialDTO();

            if (id.HasValue && !preRequisicaoMaterial.Id.HasValue)
                messageQueue.Add(Application.Resource.Sigim.ErrorMessages.NenhumRegistroEncontrado, TypeMessage.Error);

            model.PreRequisicaoMaterial = preRequisicaoMaterial;
            model.JsonItens = Newtonsoft.Json.JsonConvert.SerializeObject(preRequisicaoMaterial.ListaItens);

            var parametros = parametrosOrdemCompraAppService.Obter();
            if (parametros != null)
            {
                model.DataMinima = parametros.DiasDataMinima.HasValue ? DateTime.Now.AddDays(parametros.DiasDataMinima.Value) : DateTime.Now;
                model.DataMaxima = parametros.DiasPrazo.HasValue ? model.DataMinima.Value.AddDays(parametros.DiasPrazo.Value) : DateTime.Now;
                model.Prazo = parametros.DiasPrazo.HasValue ? parametros.DiasPrazo.Value : 0;
            }

            model.PodeSalvar = preRequisicaoMaterialAppService.EhPermitidoSalvar(preRequisicaoMaterial);
            model.PodeCancelar = preRequisicaoMaterialAppService.EhPermitidoCancelar(preRequisicaoMaterial);
            model.PodeAdicionarItem = preRequisicaoMaterialAppService.EhPermitidoAdicionarItem(preRequisicaoMaterial);
            model.PodeCancelarItem = preRequisicaoMaterialAppService.EhPermitidoCancelarItem(preRequisicaoMaterial);
            model.PodeEditarItem = preRequisicaoMaterialAppService.EhPermitidoEditarItem(preRequisicaoMaterial);
            model.PodeAprovarItem = preRequisicaoMaterialAppService.EhPermitidoAprovarItem(preRequisicaoMaterial);
            
            return View(model);
        }

        [HttpPost]
        public ActionResult ListaMaterial(string descricao)
        {
            var model = materialAppService.ListarAtivosPeloTipoTabelaPropria(descricao).Take(15);
            return Json(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastro(PreRequisicaoMaterialCadastroViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.PreRequisicaoMaterial.ListaItens = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PreRequisicaoMaterialItemDTO>>(model.JsonItens);
                if(preRequisicaoMaterialAppService.Salvar(model.PreRequisicaoMaterial))
                    return PartialView("Redirect", Url.Action("Cadastro", "PreRequisicaoMaterial", new { id = model.PreRequisicaoMaterial.Id }));
            }
            return PartialView("_NotificationMessagesPartial");
        }

        [HttpPost]
        public ActionResult Aprovar(int? id, int[] itens)
        {
            if (preRequisicaoMaterialAppService.Aprovar(id, itens))
                return PartialView("Redirect", Url.Action("Cadastro", "PreRequisicaoMaterial", new { id = id }));

            return PartialView("_NotificationMessagesPartial");
        }
    }
}