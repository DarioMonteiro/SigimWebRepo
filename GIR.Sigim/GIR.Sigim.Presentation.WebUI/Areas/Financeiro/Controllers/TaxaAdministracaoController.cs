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
using GIR.Sigim.Application.Filtros;
using Newtonsoft.Json;

namespace GIR.Sigim.Presentation.WebUI.Areas.Financeiro.Controllers
{
    public class TaxaAdministracaoController : BaseController
    {
        private ITaxaAdministracaoAppService taxaAdministracaoAppService;
        private IClienteFornecedorAppService clienteFornecedorAppService;
        private ICentroCustoAppService centroCustoAppService;

        public TaxaAdministracaoController(
            ITaxaAdministracaoAppService taxaAdministracaoAppService,
            IClienteFornecedorAppService clienteFornecedorAppService,
            ICentroCustoAppService centroCustoAppService,
            MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.taxaAdministracaoAppService = taxaAdministracaoAppService;
            this.clienteFornecedorAppService = clienteFornecedorAppService;
            this.centroCustoAppService = centroCustoAppService;
        }

      
        public ActionResult Index()
        {
            var model = new TaxaAdministracaoViewModel();
            //CarregarCombos(model);
            return View(model);
        }

        public ActionResult Cadastro(string CentroCustoId, int? ClienteId)
        {
            TaxaAdministracaoViewModel model = new TaxaAdministracaoViewModel();
            List<TaxaAdministracaoDTO> lista = new List<TaxaAdministracaoDTO>();

            int ClienteIdAux = 0;
            if (ClienteId.HasValue == true) { ClienteIdAux = ClienteId.Value; }

            if ((CentroCustoId != null) && (ClienteIdAux != 0))
            {
                lista = taxaAdministracaoAppService.ListarPeloCentroCustoCliente(CentroCustoId, ClienteId.Value);
                lista = LimpaClasseListaFilhos(lista);
                
                if (lista.Count == 0)
                {
                    messageQueue.Add(Application.Resource.Sigim.ErrorMessages.NenhumRegistroEncontrado, TypeMessage.Error);
                }

                var centroCusto = centroCustoAppService.ObterPeloCodigo(CentroCustoId);
                
                model.CentroCusto = centroCusto;
                model.ClienteId = ClienteId.Value;
            }

            CarregarCombos(model);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastro(TaxaAdministracaoViewModel model)
        {
            List<TaxaAdministracaoDTO> lista;

            if (ModelState.IsValid)
            {
                lista = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TaxaAdministracaoDTO>>(model.JsonItens);
                taxaAdministracaoAppService.Salvar(model.CentroCusto.Codigo, model.ClienteId, lista);
            }
            return PartialView("_NotificationMessagesPartial");
        }


        public ActionResult CarregarItem(string CentroCustoId, int? ClienteId)
        {
            int ClienteIdAux = 0;
            if (ClienteId.HasValue == true) { ClienteIdAux = ClienteId.Value; }
            var lista = taxaAdministracaoAppService.ListarPeloCentroCustoCliente(CentroCustoId, ClienteIdAux);
            lista = LimpaClasseListaFilhos(lista);
            return Json(lista);
        }


        [HttpPost]
        public ActionResult Deletar(string CentroCustoId, int? ClienteId)
        {
            int ClienteIdAux = 0;
            if (ClienteId.HasValue == true) { ClienteIdAux = ClienteId.Value; }

            if ((CentroCustoId == "") || (ClienteIdAux == 0))
            {
                messageQueue.Add(Application.Resource.Sigim.ErrorMessages.NenhumRegistroEncontrado, TypeMessage.Error);
            }
            else 
            {
                taxaAdministracaoAppService.Deletar(CentroCustoId, ClienteIdAux);
            }
            
            return PartialView("_NotificationMessagesPartial");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Lista(TaxaAdministracaoViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = taxaAdministracaoAppService.ListarTodos();
                if (result.Any())
                {
                    var listaViewModel = CreateListaViewModel(result);
                    return PartialView("ListaPartial", listaViewModel);
                }
                return PartialView("_EmptyListPartial");
            }
            return PartialView("_NotificationMessagesPartial");
        }

        private void CarregarCombos(TaxaAdministracaoViewModel model)
        {
            model.ListaCliente = new SelectList(clienteFornecedorAppService.ListarAtivos(), "Id", "Nome", model.ClienteId);
        }

        private List<TaxaAdministracaoDTO> LimpaClasseListaFilhos(List<TaxaAdministracaoDTO> lista)
        {
            foreach (var item in lista)
            {
                item.Classe.ListaFilhos = null;
            }
            return lista;
        }


    }
}
