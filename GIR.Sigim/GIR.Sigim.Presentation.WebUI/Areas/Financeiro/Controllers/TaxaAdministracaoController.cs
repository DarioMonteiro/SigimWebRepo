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
using GIR.Sigim.Application.Constantes;
using GIR.Sigim.Presentation.WebUI.CustomAttributes;

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

        [AutorizacaoAcessoAuthorize(GIR.Sigim.Application.Constantes.Modulo.FinanceiroWeb, Roles = Funcionalidade.TaxaAdministracaoAcessar)]
        public ActionResult Index()
        {
            var model = new TaxaAdministracaoViewModel();
            return View(model);
        }

        public ActionResult Cadastro(string centroCustoId, int? clienteId)
        {
            TaxaAdministracaoViewModel model = new TaxaAdministracaoViewModel();
            List<TaxaAdministracaoDTO> lista = new List<TaxaAdministracaoDTO>();

            model.CentroCusto = new CentroCustoDTO();
            model.Cliente = new ClienteFornecedorDTO();

            if (!string.IsNullOrEmpty(centroCustoId) && clienteId.HasValue)
            {
                lista = taxaAdministracaoAppService.ListarPeloCentroCustoCliente(centroCustoId, clienteId.Value);

                if (lista.Count == 0)
                {
                    messageQueue.Add(Application.Resource.Sigim.ErrorMessages.NenhumRegistroEncontrado, TypeMessage.Error);
                }
                else
                {
                    model.CentroCusto = lista.First().CentroCusto;
                    model.Cliente = lista.First().Cliente;

                    model.JsonItens = Newtonsoft.Json.JsonConvert.SerializeObject(lista,
                                                                                  Formatting.Indented,
                                                                                  new JsonSerializerSettings()
                                                                                  { 
                                                                                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                                                                                  });
                }
            }

            model.PodeSalvar = taxaAdministracaoAppService.EhPermitidoSalvar();
            model.PodeDeletar = taxaAdministracaoAppService.EhPermitidoDeletar();
            model.PodeImprimir = taxaAdministracaoAppService.EhPermitidoImprimir();

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
                taxaAdministracaoAppService.Salvar(model.CentroCusto.Codigo, model.Cliente.Id.Value, lista);
            }
            return PartialView("_NotificationMessagesPartial");
        }


        public ActionResult CarregarItem(string centroCustoCodigo, int? clienteId)
        {
            string listaString;
            if ((string.IsNullOrEmpty(centroCustoCodigo)) || (!clienteId.HasValue))
            {
                listaString = "[]";
            }
            else
            {
                var lista = taxaAdministracaoAppService.ListarPeloCentroCustoCliente(centroCustoCodigo, clienteId.Value);
                listaString = Newtonsoft.Json.JsonConvert.SerializeObject(lista, Formatting.Indented,
                                                                          new JsonSerializerSettings()
                                                                          {
                                                                              ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                                                                          });
            }
            return Json(new { lista = listaString });
        }


        [HttpPost]
        public ActionResult Deletar(string centroCustoCodigo, int? clienteId)
        {

            if ((string.IsNullOrEmpty(centroCustoCodigo)) || (!clienteId.HasValue))
            {
                messageQueue.Add(Application.Resource.Sigim.ErrorMessages.NenhumRegistroEncontrado, TypeMessage.Error);
            }
            else 
            {
                taxaAdministracaoAppService.Deletar(centroCustoCodigo, clienteId.Value);
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
                    result = result.OrderBy(l => l.CentroCusto.Codigo).ThenByDescending(l => l.Cliente.Nome).ToList();
                    var listaViewModel = CreateListaViewModel(result);
                    return PartialView("ListaPartial", listaViewModel);
                }
                return PartialView("_EmptyListPartial");
            }
            return PartialView("_NotificationMessagesPartial");
        }

        public ActionResult Imprimir(string centroCustoCodigo, int? clienteId, FormatoExportacaoArquivo formato)
        {
            if ((!string.IsNullOrEmpty(centroCustoCodigo)) && (clienteId.HasValue))
            {
                var arquivo = taxaAdministracaoAppService.ExportarRelTaxaAdministracao(centroCustoCodigo, clienteId, formato);
                if (arquivo != null)
                {
                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();
                    return File(arquivo.Stream, arquivo.ContentType, arquivo.NomeComExtensao);
                }
            }
            return PartialView("_NotificationMessagesPartial");
        }

    }
}
