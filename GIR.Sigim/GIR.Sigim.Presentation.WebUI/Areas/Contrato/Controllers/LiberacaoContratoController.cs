using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GIR.Sigim.Presentation.WebUI.Controllers;
using GIR.Sigim.Application.Service.Sigim;
using GIR.Sigim.Application.Service.Contrato;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Application.Constantes;
using GIR.Sigim.Presentation.WebUI.Areas.Contrato.ViewModel;
using GIR.Sigim.Application.DTO.Contrato;
using GIR.Sigim.Application.DTO.Sigim;
using Newtonsoft.Json;

namespace GIR.Sigim.Presentation.WebUI.Areas.Contrato.Controllers
{
    public class LiberacaoContratoController : BaseController
    {

        #region Declaration

        private IClienteFornecedorAppService clienteFornecedorAppService;
        private IContratoAppService contratoAppService;
        private IContratoRetificacaoAppService contratoRetificacaoAppService;
        private IContratoRetificacaoItemMedicaoAppService contratoRetificacaoItemMedicaoAppService;

        #endregion

        #region Constructor

        public LiberacaoContratoController(IClienteFornecedorAppService clienteFornecedorAppService,
                                           IContratoAppService contratoAppService,
                                           IContratoRetificacaoAppService contratoRetificacaoAppService,
                                           IContratoRetificacaoItemMedicaoAppService contratoRetificacaoItemMedicaoAppService,
                                           MessageQueue messageQueue) 
            : base(messageQueue) 
        {
            this.clienteFornecedorAppService = clienteFornecedorAppService;
            this.contratoAppService = contratoAppService;
            this.contratoRetificacaoAppService = contratoRetificacaoAppService;
            this.contratoRetificacaoItemMedicaoAppService = contratoRetificacaoItemMedicaoAppService;
        }

        #endregion

        #region Methods

        [Authorize(Roles = Funcionalidade.LiberacaoAcessar)]
        public ActionResult Index()
        {
            var model = Session["Filtro"] as LiberacaoContratoListaViewModel;
            if (model == null)
            {
                model = new LiberacaoContratoListaViewModel();
                model.Filtro.PaginationParameters.PageSize = this.DefaultPageSize;
                model.Filtro.PaginationParameters.UniqueIdentifier = GenerateUniqueIdentifier();
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Lista(LiberacaoContratoListaViewModel model)
        {
            if (ModelState.IsValid)
            {
                Session["Filtro"] = model;
                int totalRegistros;

                if (string.IsNullOrEmpty(model.Filtro.PaginationParameters.OrderBy))
                    model.Filtro.PaginationParameters.OrderBy = "id";

                var result = contratoAppService.ListarPeloFiltro(model.Filtro, Usuario.Id, out totalRegistros);
                if (result.Any())
                {
                    if (model.Filtro.PaginationParameters.PageIndex == 0 && result.Count == 1)
                    {
                        Session["Filtro"] = null;

                        return PartialView("Redirect", Url.Action("Liberacao", "LiberacaoContrato", new { id = result[0].Id }));
                    }
                    else
                    {
                        var listaViewModel = CreateListaViewModel(model.Filtro.PaginationParameters, totalRegistros, result);
                        return PartialView("ListaPartial", listaViewModel);
                    }
                }
                return PartialView("_EmptyListPartial");
            }
            return PartialView("_NotificationMessagesPartial");
        }

        [Authorize(Roles = Funcionalidade.LiberacaoAcessar)]
        public ActionResult Liberacao(int? id)
        {
            LiberacaoContratoLiberacaoViewModel model = new LiberacaoContratoLiberacaoViewModel();
            ICollection<ContratoRetificacaoItemDTO> ListaItensUltimoContratoRetificacao = new HashSet<ContratoRetificacaoItemDTO>();

            ContratoDTO contrato = contratoAppService.ObterPeloId(id, Usuario.Id) ?? new ContratoDTO();
            ContratoRetificacaoItemDTO contratoRetificacaoItem = new ContratoRetificacaoItemDTO();

            model.ListaServicoContratoRetificacaoItem = new SelectList(new List<ContratoRetificacaoItemDTO>(), "Id", "SequencialDescricaoItemComplemento");

            model.ContratoRetificacaoItemMedicao.ContratoId = contrato.Id.Value;
            model.Contrato = contrato;

            if (id.HasValue && !contratoAppService.EhContratoExistente(contrato))
            {
                return View(model);
            }

            if (!contratoAppService.EhContratoComCentroCustoAtivo(contrato))
            {
                return View(model);
            }

            ContratoRetificacaoDTO contratoRetificacao = contrato.ListaContratoRetificacao.Last();

            if (!contratoRetificacaoAppService.EhRetificacaoExistente(contratoRetificacao))
            {
                return View(model);
            }

            if (!contratoRetificacaoAppService.EhRetificacaoAprovada(contratoRetificacao))
            {
                return View(model);
            }

            model.ContratoRetificacaoItemMedicao.ContratoRetificacaoId = contratoRetificacao.Id.Value;

            ListaItensUltimoContratoRetificacao = contratoRetificacao.ListaContratoRetificacaoItem;

            ListaItensUltimoContratoRetificacao.Add(CriaRetificacaoItemFakeTodosOsItens());

            model.ListaServicoContratoRetificacaoItem = new SelectList(ListaItensUltimoContratoRetificacao.OrderBy(l => l.Sequencial), "Id", "SequencialDescricaoItemComplemento", ListaItensUltimoContratoRetificacao.Select(l => l.Id.Value));

            List<ItemListaLiberacaoDTO> listaItemListaLiberacao = new List<ItemListaLiberacaoDTO>();
            contratoAppService.PreencherResumo(contrato, contratoRetificacaoItem, model.Resumo, listaItemListaLiberacao);

            model.JsonListaItemListaLiberacao = JsonConvert.SerializeObject(listaItemListaLiberacao);

            return View(model);
        }

        [HttpPost]
        public ActionResult RecuperaContratoRetificacaoItem(int? contratoId, int? contratoRetificacaoItemId)
        {
            ResumoLiberacaoDTO resumo = new ResumoLiberacaoDTO();
            List<ItemListaLiberacaoDTO> listaItemListaLiberacao = new List<ItemListaLiberacaoDTO>();

            if (contratoId.HasValue && contratoRetificacaoItemId.HasValue)
            {
                ContratoDTO contrato = contratoAppService.ObterPeloId(contratoId, Usuario.Id) ?? new ContratoDTO();

                if (contratoId.HasValue && !contratoAppService.EhContratoExistente(contrato))
                {
                    var msg = messageQueue.GetAll()[0].Text;
                    messageQueue.Clear();

                    return Json(new
                    {
                        ehRecuperou = false,
                        errorMessage = msg,
                        resumo = resumo
                    });
                }

                if (!contratoAppService.EhContratoComCentroCustoAtivo(contrato))
                {
                    var msg = messageQueue.GetAll()[0].Text;
                    messageQueue.Clear();

                    return Json(new
                    {
                        ehRecuperou = false,
                        errorMessage = msg,
                        resumo = resumo
                    });
                }

                ContratoRetificacaoItemDTO contratoRetificacaoItem = new ContratoRetificacaoItemDTO();
                if (contratoRetificacaoItemId.HasValue)
                {
                    if (contratoRetificacaoItemId > 0)
                    {
                        contratoRetificacaoItem = contrato.ListaContratoRetificacaoItem.Where(l => l.Id == contratoRetificacaoItemId).FirstOrDefault() ?? new ContratoRetificacaoItemDTO();
                    }
                    contratoAppService.PreencherResumo(contrato, contratoRetificacaoItem, resumo, listaItemListaLiberacao);
                    return Json(new
                    {
                        ehRecuperou = true,
                        errorMessage = string.Empty,
                        resumo = resumo,
                        listaItemListaLiberacao = JsonConvert.SerializeObject(listaItemListaLiberacao)

                        //listaItemListaLiberacao = listaItemListaLiberacao

                    });
                }
            }

            return Json(new
            {
                ehRecuperou = false,
                errorMessage = string.Empty,
                resumo = resumo
            });

        }


        #endregion

        #region Métodos Privados"

        private ContratoRetificacaoItemDTO CriaRetificacaoItemFakeTodosOsItens()
        {
            ContratoRetificacaoItemDTO contratoRetificacaoItemTodos = new ContratoRetificacaoItemDTO();
            contratoRetificacaoItemTodos.Servico = new ServicoDTO();
            contratoRetificacaoItemTodos.Id = 0;
            contratoRetificacaoItemTodos.Sequencial = 0;
            contratoRetificacaoItemTodos.Servico.Id = 0;
            contratoRetificacaoItemTodos.Servico.Descricao = "Todos os ítens do contrato";

            return contratoRetificacaoItemTodos;
        }

        #endregion
    }
}
