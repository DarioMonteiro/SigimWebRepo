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
using GIR.Sigim.Application.Service.Financeiro;
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
        private ITipoDocumentoAppService tipoDocumentoAppService;

        #endregion

        #region Constructor

        public LiberacaoContratoController(IClienteFornecedorAppService clienteFornecedorAppService,
                                           IContratoAppService contratoAppService,
                                           IContratoRetificacaoAppService contratoRetificacaoAppService,
                                           IContratoRetificacaoItemMedicaoAppService contratoRetificacaoItemMedicaoAppService,
                                           ITipoDocumentoAppService tipoDocumentoAppService,
                                           MessageQueue messageQueue) 
            : base(messageQueue) 
        {
            this.clienteFornecedorAppService = clienteFornecedorAppService;
            this.contratoAppService = contratoAppService;
            this.contratoRetificacaoAppService = contratoRetificacaoAppService;
            this.contratoRetificacaoItemMedicaoAppService = contratoRetificacaoItemMedicaoAppService;
            this.tipoDocumentoAppService = tipoDocumentoAppService;
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

            model.ContratoRetificacao = contratoRetificacao;

            model.ContratoRetificacaoItemMedicao.ContratoRetificacaoId = contratoRetificacao.Id.Value;

            ListaItensUltimoContratoRetificacao = contratoRetificacao.ListaContratoRetificacaoItem;

            ListaItensUltimoContratoRetificacao.Add(CriaRetificacaoItemFakeTodosOsItens());

            model.ListaServicoContratoRetificacaoItem = new SelectList(ListaItensUltimoContratoRetificacao.OrderBy(l => l.Sequencial), "Id", "SequencialDescricaoItemComplemento", ListaItensUltimoContratoRetificacao.Select(l => l.Id.Value));

            List<ItemLiberacaoDTO> listaItemLiberacao = new List<ItemLiberacaoDTO>();
            contratoAppService.RecuperarMedicoesALiberar(contrato, contratoRetificacaoItem, model.Resumo, out listaItemLiberacao);
            model.PodeConcluirContrato =  contratoAppService.PodeConcluirContrato(contrato);

            model.JsonListaItemLiberacao = JsonConvert.SerializeObject(listaItemLiberacao);

            model.PodeHabilitarBotoes = contratoAppService.EhPermitidoHabilitarBotoes(contrato);

            model.PodeAprovarLiberar = true;
            model.PodeAprovar = true;
            model.PodeLiberar = true;
            model.PodeCancelarLiberacao = true;
            model.PodeAssociarNF = true;
            model.PodeAlterarDataVencimento = true;
            model.PodeImprimirMedicao = true;
            model.DataVencimento = DateTime.Now;

            CarregarCombos(model);

            return View(model);
        }

        [HttpPost]
        public ActionResult RecuperaContratoRetificacaoItem(int? contratoId,int ? contratoRetificacaoId, int? contratoRetificacaoItemId)
        {
            ResumoLiberacaoDTO resumo = new ResumoLiberacaoDTO();
            List<ItemLiberacaoDTO> listaItemLiberacao = new List<ItemLiberacaoDTO>();

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
                        resumo = resumo,
                        listaItemLiberacao = listaItemLiberacao,
                        podeConcluirContrato = false,
                        redirectToUrl = string.Empty
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
                        resumo = resumo,
                        listaItemLiberacao = listaItemLiberacao,
                        podeConcluirContrato = false,
                        redirectToUrl = string.Empty
                    });
                }

                if (!contratoAppService.EhUltimoContratoRetificacao(contrato.Id, contratoRetificacaoId))
                {

                    return Json(new
                            {
                                ehRecuperou = false,
                                errorMessage = string.Empty,
                                resumo = resumo,
                                listaItemLiberacao = listaItemLiberacao,
                                podeConcluirContrato = false,
                                redirectToUrl = Url.Action("Liberacao", "LiberacaoContrato", new { area = "Contrato", id = contrato.Id })
                            });
                }

                ContratoRetificacaoItemDTO contratoRetificacaoItem = new ContratoRetificacaoItemDTO();
                if (contratoRetificacaoItemId.HasValue)
                {
                    if (contratoRetificacaoItemId > 0)
                    {
                        contratoRetificacaoItem = contrato.ListaContratoRetificacaoItem.Where(l => l.Id == contratoRetificacaoItemId).FirstOrDefault() ?? new ContratoRetificacaoItemDTO();
                    }
                    contratoAppService.RecuperarMedicoesALiberar(contrato, contratoRetificacaoItem, resumo, out listaItemLiberacao);
                    bool podeConcluirContrato = contratoAppService.PodeConcluirContrato(contrato);
                    return Json(new
                    {
                        ehRecuperou = true,
                        errorMessage = string.Empty,
                        resumo = resumo,
                        listaItemLiberacao = JsonConvert.SerializeObject(listaItemLiberacao),
                        podeConcluirContrato = podeConcluirContrato,
                        redirectToUrl = string.Empty
                    });
                }
            }

            return Json(new
            {
                ehRecuperou = false,
                errorMessage = string.Empty,
                resumo = resumo,
                listaItemLiberacao = listaItemLiberacao,
                podeConcluirContrato = false,
                redirectToUrl = string.Empty
            });
        }


        [HttpPost]
        public ActionResult ConcluirContrato(int? contratoId, int? contratoRetificacaoId)
        {
            bool ehConcluido = false;
            string msg = "";

            if (contratoId.HasValue && contratoRetificacaoId.HasValue)
            {
                if (!contratoAppService.EhUltimoContratoRetificacao(contratoId, contratoRetificacaoId))
                {
                    ehConcluido = false;
                    msg = "As informações das liberações estão desatualizadas, carregue o contrato novamente";
                }
                else
                {
                    ehConcluido = contratoAppService.AtualizarSituacaoParaConcluido(contratoId);
                    if (messageQueue.GetAll().Count > 0)
                    {
                        msg = messageQueue.GetAll()[0].Text;
                        messageQueue.Clear();
                    }
                }
            }

            return Json(new
            {
                ehConcluido = ehConcluido,
                message = msg
            });
        }

        [HttpPost]
        public ActionResult TratarAprovar(int? contratoId, int? contratoRetificacaoId, string listaItemLiberacao)
        {
            bool ehAprovado = false;
            string msg = "";

            if (contratoId.HasValue && contratoRetificacaoId.HasValue)
            {
                if (!contratoAppService.EhUltimoContratoRetificacao(contratoId, contratoRetificacaoId))
                {
                    ehAprovado = false;
                    msg = "As informações das liberações estão desatualizadas, carregue o contrato novamente";
                }
                else
                {
                    if (!string.IsNullOrEmpty(listaItemLiberacao))
                    {
                        List<ItemLiberacaoDTO> listaItemLiberacaoDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ItemLiberacaoDTO>>(listaItemLiberacao);

                        ehAprovado = contratoAppService.AprovarListaItemLiberacao(contratoId.Value, listaItemLiberacaoDTO);
                        if (messageQueue.GetAll().Count > 0)
                        {
                            msg = messageQueue.GetAll()[0].Text;
                            messageQueue.Clear();
                        }
                    }
                    else
                    {
                        ehAprovado = false;
                        msg = "Nenhum item da lista foi selecionado";
                    }
                }
            }
            return Json(new
            {
                ehAprovado = ehAprovado,
                message = msg,
                redirectToUrl = Url.Action("Liberacao", "LiberacaoContrato", new { area = "Contrato", id = contratoId })
            });
        }

        [HttpPost]
        public ActionResult ValidarImpressaoMedicao(int? contratoId, string listaItemLiberacao)
        {
            string msg = "";
            List<ItemLiberacaoDTO> listaItemLiberacaoDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ItemLiberacaoDTO>>(listaItemLiberacao);
            int? contratoRetificacaoItemMedicaoId;

            bool ehValido = contratoAppService.ValidarImpressaoMedicaoPelaLiberacao(contratoId, listaItemLiberacaoDTO, out contratoRetificacaoItemMedicaoId);
            if (messageQueue.GetAll().Count > 0)
            {
                msg = messageQueue.GetAll()[0].Text;
                messageQueue.Clear();
            }

            return Json(new
            {
                message = msg,
                contratoRetificacaoItemMedicaoId = contratoRetificacaoItemMedicaoId 
            });
        }

        public ActionResult ImprimirMedicao(FormatoExportacaoArquivo formato,int? contratoId,int? contratoRetificacaoItemMedicaoId)
        {
            var arquivo = contratoAppService.ImprimirMedicaoPelaLiberacao(formato, contratoId, contratoRetificacaoItemMedicaoId);

            if (arquivo != null)
            {
                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();
                return File(arquivo.Stream, arquivo.ContentType, arquivo.NomeComExtensao);
            }

            return PartialView("_NotificationMessagesPartial");
        }

        [HttpPost]
        public ActionResult ValidarTrocaDataVencimento(int? contratoId, int? contratoRetificacaoId, string dataVencimento, string listaItemLiberacao)
        {
            bool ehValidouTrocaDataVencimento = false;
            string msg = "";


            if (contratoId.HasValue && contratoRetificacaoId.HasValue && !string.IsNullOrEmpty(dataVencimento))
            {
                if (!contratoAppService.EhUltimoContratoRetificacao(contratoId, contratoRetificacaoId))
                {
                    ehValidouTrocaDataVencimento = false;
                    msg = "As informações das liberações estão desatualizadas, carregue o contrato novamente";
                }
                else
                {
                    if (!string.IsNullOrEmpty(listaItemLiberacao))
                    {
                        List<ItemLiberacaoDTO> listaItemLiberacaoDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ItemLiberacaoDTO>>(listaItemLiberacao);

                        Nullable<DateTime> dtVencimento = DateTime.Parse(dataVencimento);

                        ehValidouTrocaDataVencimento = contratoAppService.ValidarTrocaDataVencimentoListaItemLiberacao(dtVencimento, listaItemLiberacaoDTO);
                        if (messageQueue.GetAll().Count > 0)
                        {
                            msg = messageQueue.GetAll()[0].Text;
                            messageQueue.Clear();
                        }
                    }
                    else
                    {
                        ehValidouTrocaDataVencimento = false;
                        msg = "Nenhum item da lista foi selecionado";
                    }
                }
            }
            return Json(new
            {
                ehValidouTrocaDataVencimento = ehValidouTrocaDataVencimento,
                message = msg
            });
        }

        [HttpPost]
        public ActionResult TratarTrocaDataVencimento(int? contratoId, int? contratoRetificacaoId, string dataVencimento, string listaItemLiberacao)
        {
            bool trocouDataVencimento = false;
            string msg = "";


            if (contratoId.HasValue && contratoRetificacaoId.HasValue && !string.IsNullOrEmpty(dataVencimento) )
            {
                if (!contratoAppService.EhUltimoContratoRetificacao(contratoId, contratoRetificacaoId))
                {
                    trocouDataVencimento = false;
                    msg = "As informações das liberações estão desatualizadas, carregue o contrato novamente";
                }
                else
                {
                    if (!string.IsNullOrEmpty(listaItemLiberacao))
                    {
                        List<ItemLiberacaoDTO> listaItemLiberacaoDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ItemLiberacaoDTO>>(listaItemLiberacao);

                        Nullable<DateTime> dtVencimento = DateTime.Parse(dataVencimento);

                        trocouDataVencimento = contratoAppService.TrocarDataVencimentoListaItemLiberacao(contratoId.Value,dtVencimento, listaItemLiberacaoDTO);
                        if (messageQueue.GetAll().Count > 0)
                        {
                            msg = messageQueue.GetAll()[0].Text;
                            messageQueue.Clear();
                        }
                    }
                    else
                    {
                        trocouDataVencimento = false;
                        msg = "Nenhum item da lista foi selecionado";
                    }
                }
            }
            return Json(new
            {
                trocouDataVencimento = trocouDataVencimento,
                message = msg,
                redirectToUrl = Url.Action("Liberacao", "LiberacaoContrato", new { area = "Contrato", id = contratoId })
            });
        }

        [HttpPost]
        public ActionResult ValidarAssociacaoNotaFiscal(int? contratoId, int? contratoRetificacaoId, string listaItemLiberacao)
        {
            bool ehValidou = false;
            string msg = "";
            ItemLiberacaoDTO itemLiberacao = new ItemLiberacaoDTO();

            if (contratoId.HasValue && contratoRetificacaoId.HasValue)
            {
                if (!contratoAppService.EhUltimoContratoRetificacao(contratoId, contratoRetificacaoId))
                {
                    ehValidou = false;
                    msg = "As informações das liberações estão desatualizadas, carregue o contrato novamente";
                }
                else
                {
                    if (!string.IsNullOrEmpty(listaItemLiberacao))
                    {
                        List<ItemLiberacaoDTO> listaItemLiberacaoDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ItemLiberacaoDTO>>(listaItemLiberacao);

                        ehValidou = contratoAppService.ValidarAssociacaoNotaFiscalListaItemLiberacao(contratoId.Value, listaItemLiberacaoDTO, out itemLiberacao);
                        if (messageQueue.GetAll().Count > 0)
                        {
                            msg = messageQueue.GetAll()[0].Text;
                            messageQueue.Clear();
                        }
                    }
                    else
                    {
                        ehValidou = false;
                        msg = "Nenhum item da lista foi selecionado";
                    }
                }
            }
            return Json(new
            {
                ehValidouAssociacaoNF = ehValidou,
                message = msg,
                itemLiberacao = itemLiberacao,
                redirectToUrl = Url.Action("Liberacao", "LiberacaoContrato", new { area = "Contrato", id = contratoId })
            });
        }

        [HttpPost]
        public ActionResult TratarAssociacaoNotaFiscal(int? contratoId, int? contratoRetificacaoId, int? contratoRetificacaoItemMedicaoId, int? tipoDocumentoId, string numeroDocumento, string dataEmissao, string dataVencimento)
        {
            bool associouNotaFiscal = false;
            string msg = "";


            if (contratoId.HasValue && contratoRetificacaoId.HasValue && !string.IsNullOrEmpty(dataVencimento))
            {
                if (!contratoAppService.EhUltimoContratoRetificacao(contratoId, contratoRetificacaoId))
                {
                    associouNotaFiscal = false;
                    msg = "As informações das liberações estão desatualizadas, carregue o contrato novamente";
                }
                else
                {
                    if ((contratoRetificacaoItemMedicaoId.HasValue) && (tipoDocumentoId.HasValue) && (!string.IsNullOrEmpty(numeroDocumento)) && (!string.IsNullOrEmpty(dataEmissao)) && (!string.IsNullOrEmpty(dataVencimento)) )
                    {
                        Nullable<DateTime> dtEmissao = DateTime.Parse(dataEmissao);
                        Nullable<DateTime> dtVencimento = DateTime.Parse(dataVencimento);

                        associouNotaFiscal = contratoAppService.AssociarNotaFiscalListaItemLiberacao(contratoId, contratoRetificacaoItemMedicaoId, tipoDocumentoId, numeroDocumento, dtEmissao, dtVencimento);
                        if (messageQueue.GetAll().Count > 0)
                        {
                            msg = messageQueue.GetAll()[0].Text;
                            messageQueue.Clear();
                        }
                    }
                    else
                    {
                        associouNotaFiscal = false;
                        msg = "Nenhum item da lista foi selecionado";
                    }
                }
            }
            return Json(new
            {
                associouNotaFiscal = associouNotaFiscal,
                message = msg,
                redirectToUrl = Url.Action("Liberacao", "LiberacaoContrato", new { area = "Contrato", id = contratoId })
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

        private void CarregarCombos(LiberacaoContratoLiberacaoViewModel model)
        {
            int? tipoDocumentoId = null;

            tipoDocumentoId = model.TipoDocumentoNovoId;

            model.ListaTipoDocumentoNovo = new SelectList(tipoDocumentoAppService.ListarTodos(), "Id", "Sigla", tipoDocumentoId);
        }

        #endregion
    }
}
