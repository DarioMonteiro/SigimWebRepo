﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using GIR.Sigim.Application.Service.Sigim;
using GIR.Sigim.Application.Service.Contrato;
using GIR.Sigim.Application.Service.Financeiro;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;    
using GIR.Sigim.Presentation.WebUI.Controllers;
using GIR.Sigim.Presentation.WebUI.Areas.Contrato.ViewModel;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Application.DTO.Contrato;
using GIR.Sigim.Application.Adapter;


namespace GIR.Sigim.Presentation.WebUI.Areas.Contrato.Controllers
{
    public class MedicaoContratoController : BaseController
    {

        #region Declaration

        private IClienteFornecedorAppService clienteFornecedorAppService;
        private IContratoAppService contratoAppService;
        private IContratoRetificacaoItemAppService contratoRetificacaoItemAppService;
        private IContratoRetificacaoProvisaoAppService contratoRetificacaoProvisaoAppService;
        private ITipoDocumentoAppService tipoDocumentoAppService;
        private ITipoCompraAppService tipoCompraAppService;
        private ICifFobAppService cifFobAppService;
        private INaturezaOperacaoAppService naturezaOperacaoAppService;
        private ISerieNFAppService serieNFAppService;
        private ICSTAppService cstAppService;
        private ICodigoContribuicaoAppService codigoContribuicaoAppService;
        private IContratoRetificacaoAppService contratoRetificacaoAppService;
        private IContratoRetificacaoItemMedicaoAppService contratoRetificacaoItemMedicaoAppService;
        private ITituloPagarAppService tituloPagarAppService;
        private IParametrosContratoAppService parametrosContratoAppService;

        #endregion

        #region Constructor

        public MedicaoContratoController(IClienteFornecedorAppService clienteFornecedorAppService,
                                         IContratoAppService contratoAppService,
                                         IContratoRetificacaoItemAppService contratoRetificacaoItemAppService,
                                         IContratoRetificacaoProvisaoAppService contratoRetificacaoProvisaoAppService,
                                         ITipoDocumentoAppService tipoDocumentoAppService,
                                         ITipoCompraAppService tipoCompraAppService,
                                         ICifFobAppService cifFobAppService,
                                         INaturezaOperacaoAppService naturezaOperacaoAppService,
                                         ISerieNFAppService serieNFAppService,
                                         ICSTAppService cstAppService,
                                         ICodigoContribuicaoAppService codigoContribuicaoAppService,
                                         IContratoRetificacaoAppService contratoRetificacaoAppService,
                                         IContratoRetificacaoItemMedicaoAppService contratoRetificacaoItemMedicaoAppService,
                                         ITituloPagarAppService tituloPagarAppService,
                                         IParametrosContratoAppService parametrosContratoAppService,
                                         MessageQueue messageQueue) 
            : base(messageQueue) 
        {
            this.clienteFornecedorAppService = clienteFornecedorAppService;
            this.contratoAppService = contratoAppService;
            this.contratoRetificacaoItemAppService = contratoRetificacaoItemAppService;
            this.contratoRetificacaoProvisaoAppService = contratoRetificacaoProvisaoAppService;
            this.tipoDocumentoAppService = tipoDocumentoAppService;
            this.tipoCompraAppService = tipoCompraAppService;
            this.cifFobAppService = cifFobAppService;
            this.naturezaOperacaoAppService = naturezaOperacaoAppService;
            this.serieNFAppService = serieNFAppService;
            this.cstAppService = cstAppService;
            this.codigoContribuicaoAppService = codigoContribuicaoAppService;
            this.contratoRetificacaoAppService = contratoRetificacaoAppService;
            this.contratoRetificacaoItemMedicaoAppService = contratoRetificacaoItemMedicaoAppService;
            this.tituloPagarAppService = tituloPagarAppService;
            this.parametrosContratoAppService = parametrosContratoAppService;
        }

        #endregion

        #region Methods

        public ActionResult Index()
        {
            var model = Session["Filtro"] as MedicaoContratoListaViewModel;
            if (model == null)
            {
                model = new MedicaoContratoListaViewModel();
                model.Filtro.PaginationParameters.PageSize = this.DefaultPageSize;  
            }

            CarregarCombosFiltro(model);

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Lista(MedicaoContratoListaViewModel model)
        {
            if (ModelState.IsValid)
            {
                Session["Filtro"] = model;
                int totalRegistros;

                if (string.IsNullOrEmpty(model.Filtro.PaginationParameters.OrderBy))
                    model.Filtro.PaginationParameters.OrderBy = "id";

                var result = contratoAppService.ListarPeloFiltro(model.Filtro,Usuario.Id,out totalRegistros);
                if (result.Any())
                {
                    var listaViewModel = CreateListaViewModel(model.Filtro.PaginationParameters, totalRegistros, result);
                    return PartialView("ListaPartial", listaViewModel);
                }
                return PartialView("_EmptyListPartial");
            }
            return PartialView("_NotificationMessagesPartial");
        }

        public ActionResult Medicao(int? idContrato)
        {
            MedicaoContratoMedicaoViewModel model = new MedicaoContratoMedicaoViewModel();
            ICollection<ContratoRetificacaoItemDTO> ListaItensUltimoContratoRetificacao = new HashSet<ContratoRetificacaoItemDTO>(); 

            var contrato = contratoAppService.ObterPeloId(idContrato, Usuario.Id) ?? new ContratoDTO();
            model.ListaServicoContratoRetificacaoItem = new SelectList(new List<ContratoRetificacaoItemDTO>(), "Id", "SequencialDescricaoItemComplemento");

            model.ContratoRetificacaoItemMedicao.ContratoId = contrato.Id.Value;
            model.ContratoRetificacaoItemMedicao.Contrato = contrato;

            CarregarCombosMedicao(model);
 
            model.PodeSalvar = false;
            model.PodeCancelar = false;
            model.PodeImprimir = false;

            ParametrosContratoDTO parametros = parametrosContratoAppService.Obter();
            if (parametros != null)
            {
                model.DiasMedicaoParametrosContrato = parametros.DiasMedicao.HasValue ? parametros.DiasMedicao.Value : 0;
                model.DiasPagamentoParametrosContrato = parametros.DiasPagamento.HasValue ? parametros.DiasPagamento.Value : 0;
                model.DataLimiteMedicao = Convert.ToDateTime(DateTime.Now.AddDays((model.DiasMedicaoParametrosContrato.Value * -1)).ToShortDateString());
            }

            model.EhSituacaoAguardandoAprovacao = true;
            model.EhSituacaoAguardandoLiberacao = false;
            model.EhSituacaoLiberado = false;

            if (idContrato.HasValue && !contratoAppService.EhContratoExistente(contrato))
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

            if (!contratoAppService.EhContratoAssinado(contrato))
            {
                return View(model);
            }

            model.RetencaoContratual = 0;
            if (contratoRetificacao.RetencaoContratual.HasValue)
            {
                model.RetencaoContratual = contratoRetificacao.RetencaoContratual;
            }

            model.ContratoRetificacaoItemMedicao.ContratoRetificacaoId = contratoRetificacao.Id.Value;

            ListaItensUltimoContratoRetificacao = contratoRetificacao.ListaContratoRetificacaoItem;

            model.ListaServicoContratoRetificacaoItem = new SelectList(ListaItensUltimoContratoRetificacao, "Id", "SequencialDescricaoItemComplemento", ListaItensUltimoContratoRetificacao.Select(c => c.Id));

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Medicao(MedicaoContratoMedicaoViewModel model)
        {
            if (ModelState.IsValid)
            {
                //if (contratoRetificacaoItemMedicaoAppService.Salvar(model.ContratoRetificacaoItemMedicao))
                //    return PartialView("Redirect", Url.Action("Medicao", "MedicaoContrato", new { idContrato = model.ContratoRetificacaoItemMedicao.ContratoId }));
                contratoRetificacaoItemMedicaoAppService.Salvar(model.ContratoRetificacaoItemMedicao);
            }
            return PartialView("_NotificationMessagesPartial");
        }

        [HttpPost]
        public ActionResult RecuperaContratoRetificacaoItem(int? codigo)
        {
            List<ContratoRetificacaoProvisaoDTO> listaContratoRetificacaoProvisao = null;
            ContratoRetificacaoItemDTO contratoRetificacaoItem = null;

            if (codigo.HasValue)
            {
                listaContratoRetificacaoProvisao = contratoRetificacaoProvisaoAppService.ObterListaCronograma(codigo.Value);

                if (!contratoRetificacaoProvisaoAppService.ExisteContratoRetificacaoProvisao(listaContratoRetificacaoProvisao))
                {
                    var msg = messageQueue.GetAll()[0].Text;
                    messageQueue.Clear();
                    return Json(new
                                {
                                    ehRecuperou = false,
                                    errorMessage = msg,
                                    complementoDescricao = "",
                                    ehNaturezaItemPorPrecoGlobal = false,
                                    descricaoNaturezaItem = "",
                                    siglaUnidadeMedida = "",
                                    valorItem = "",
                                    retencaoItem = "",
                                    precoUnitario = "",
                                    baseRetencaoItem = "",
                                    sequencialItem = "",
                                    listaContratoRetificacaoProvisao = listaContratoRetificacaoProvisao
                                });
                }
                else
                {
                    contratoRetificacaoItem = listaContratoRetificacaoProvisao.ElementAt(0).ContratoRetificacaoItem;

                    bool EhNaturezaItemPorPrecoGlobal = contratoRetificacaoItemAppService.EhNaturezaItemPrecoGlobal(contratoRetificacaoItem);
                    bool EhNaturezaItemPorPrecoUnitario = contratoRetificacaoItemAppService.EhNaturezaItemPrecoUnitario(contratoRetificacaoItem);
                    
                    return Json(new
                    {
                        ehRecuperou = true,
                        errorMessage = string.Empty,
                        complementoDescricao = contratoRetificacaoItem.ComplementoDescricao,
                        ehNaturezaItemPorPrecoGlobal = EhNaturezaItemPorPrecoGlobal,
                        descricaoNaturezaItem = contratoRetificacaoItem.DescricaoNaturezaItem,
                        siglaUnidadeMedida = contratoRetificacaoItem.Servico.SiglaUnidadeMedida,
                        valorItem = contratoRetificacaoItem.ValorItem,
                        retencaoItem = contratoRetificacaoItem.RetencaoItem,
                        precoUnitario = contratoRetificacaoItem.PrecoUnitario,
                        baseRetencaoItem = contratoRetificacaoItem.BaseRetencaoItem,
                        sequencialItem = contratoRetificacaoItem.Sequencial,
                        listaContratoRetificacaoProvisao = Newtonsoft.Json.JsonConvert.SerializeObject(listaContratoRetificacaoProvisao)
                    });
                }
            }
            return Json(new 
            {
                ehRecuperou = false, 
                errorMessage = string.Empty, 
                complementoDescricao = "",
                ehNaturezaItemPorPrecoGlobal = false,
                descricaoNaturezaItem = "",
                siglaUnidadeMedida = "",
                valorItem = "",
                retencaoItem = "",
                precoUnitario = "",
                baseRetencaoItem = "",
                sequencialItem = "",
                listaContratoRetificacaoProvisao = listaContratoRetificacaoProvisao
            });
        }

        [HttpPost]
        public ActionResult RecuperaMedicaoPorSequencialItem(int? contratoId, int? sequencialItem)
        {
            List<ContratoRetificacaoItemMedicaoDTO> listaMedicao = null;

            if (contratoId.HasValue && sequencialItem.HasValue)
            {
                listaMedicao = contratoRetificacaoItemMedicaoAppService.ObtemPorSequencialItem(contratoId.Value, sequencialItem.Value);

                if (listaMedicao.Count > 0)
                {
                    return Json(new
                    {
                        ehRecuperou = true,
                        errorMessage = string.Empty,
                        listaContratoRetificacaoItemMedicao = Newtonsoft.Json.JsonConvert.SerializeObject(listaMedicao)
                    });
                }

            }

            return Json(new
            {
                ehRecuperou = false,
                errorMessage = string.Empty,
                listaContratoRetificacaoItemMedicao = Newtonsoft.Json.JsonConvert.SerializeObject(listaMedicao)
            });

        }

        [HttpPost]
        public ActionResult RecuperaContratoRetificacaoItemMedicao(int? contratoRetificacaoItemMedicaoId)
        {
            ContratoRetificacaoItemMedicaoDTO medicao = null;

            if (contratoRetificacaoItemMedicaoId.HasValue)
            {
                medicao = contratoRetificacaoItemMedicaoAppService.ObterPeloId(contratoRetificacaoItemMedicaoId.Value);

                if (!contratoRetificacaoItemMedicaoAppService.EhValidaMedicaoRecuperada(medicao))
                {
                    var msg = messageQueue.GetAll()[0].Text;
                    messageQueue.Clear();
                    return Json(new
                    {
                        ehRecuperou = false,
                        errorMessage = msg
                    });
                }
                else
                {
                    return Json(new
                    {
                        ehRecuperou = true,
                        errorMessage = string.Empty,
                        medicaoRecuperada = Newtonsoft.Json.JsonConvert.SerializeObject(medicao)
                    });
                }
            }
            return Json(new
            {
                ehRecuperou = false,
                errorMessage = string.Empty
            });
        }

        [HttpPost]
        public ActionResult Cancelar(int? id)
        {
            bool cancelou = false;

            cancelou = contratoRetificacaoItemMedicaoAppService.Cancelar(id);
            var msg = messageQueue.GetAll()[0].Text;
            messageQueue.Clear();

            if (!cancelou)
            {
                return Json(new
                {
                    ehCancelou = false,
                    message = msg
                });
            }

            return Json(new
            {
                ehCancelou = true,
                message = msg
            });
        }

        private void CarregarCombosFiltro(MedicaoContratoListaViewModel model) 
        {
            int? contratanteId = null;
            int? contratadoId = null;

            if (model.Filtro != null) 
            {
                contratanteId = model.Filtro.ContratanteId;
                contratadoId = model.Filtro.ContratadoId;
            }

            model.ListaContratante = new SelectList(clienteFornecedorAppService.ListarAtivosDeContrato(), "Id", "Nome", contratanteId);
            model.ListaContratado = new SelectList(clienteFornecedorAppService.ListarAtivosDeContrato(), "Id", "Nome", contratadoId);

            //model.ListaContratante = new SelectList(clienteFornecedorAppService.ListarClienteFornecedor(3,0,2), "Id", "Nome", contratanteId);
            //model.ListaContratado = new SelectList(clienteFornecedorAppService.ListarClienteFornecedor(3, 0, 2), "Id", "Nome", contratadoId); 
        }

        private void CarregarCombosMedicao(MedicaoContratoMedicaoViewModel model)
        {
            int? tipoDocumentoId = null;
            int? multifornecedorId = null;
            string tipoCompraCodigo = null;
            int? cifFobId = null;
            string naturezaOperacaoCodigo = null;
            int? serieNFId = null;
            string cstCodigo = null;
            string codigoContribuicaoCodigo = null;

            tipoDocumentoId = model.ContratoRetificacaoItemMedicao.TipoDocumentoId;

            if (model.ContratoRetificacaoItemMedicao.MultiFornecedorId.HasValue)
            {
                multifornecedorId = model.ContratoRetificacaoItemMedicao.MultiFornecedorId.Value;
            }

            tipoCompraCodigo = model.ContratoRetificacaoItemMedicao.TipoCompraCodigo;

            if (model.ContratoRetificacaoItemMedicao.CifFobId.HasValue)
            {
                cifFobId = model.ContratoRetificacaoItemMedicao.CifFobId.Value;
            }

            if (model.ContratoRetificacaoItemMedicao.SerieNFId.HasValue)
            {
                serieNFId = model.ContratoRetificacaoItemMedicao.SerieNFId.Value;
            }

            naturezaOperacaoCodigo = model.ContratoRetificacaoItemMedicao.NaturezaOperacaoCodigo;
            cstCodigo = model.ContratoRetificacaoItemMedicao.CSTCodigo;
            codigoContribuicaoCodigo = model.ContratoRetificacaoItemMedicao.CodigoContribuicaoCodigo;

            model.ListaTipoDocumento = new SelectList(tipoDocumentoAppService.ListarTodos(), "Id", "Sigla", tipoDocumentoId);
            model.ListaMultiFornecedor = new SelectList(clienteFornecedorAppService.ListarAtivosDeContrato(), "Id", "Nome", multifornecedorId);
            model.ListaTipoCompra = new SelectList(tipoCompraAppService.ListarTodos(), "Codigo", "Descricao", tipoCompraCodigo);
            model.ListaCifFob = new SelectList(cifFobAppService.ListarTodos(), "Id", "Descricao", cifFobId);
            model.ListaNaturezaOperacao = new SelectList(naturezaOperacaoAppService.ListarTodos(), "Codigo", "Descricao", naturezaOperacaoCodigo);
            model.ListaSerieNF = new SelectList(serieNFAppService.ListarTodos(), "Id", "Descricao", serieNFId);
            model.ListaCST = new SelectList(cstAppService.ListarTodos(), "Codigo", "Descricao", cstCodigo);
            model.ListaCodigoContribuicao = new SelectList(codigoContribuicaoAppService.ListarTodos(), "Codigo", "Descricao", codigoContribuicaoCodigo);
        }

        [HttpPost]
        public ActionResult ExisteNumeroDocumento(  Nullable<DateTime> dataEmissao,
                                                    Nullable<DateTime> dataVencimento,
                                                    string numeroDocumento, 
                                                    int? contratadoId)
        {
            bool existeNumeroDocumento = contratoRetificacaoItemMedicaoAppService.ExisteNumeroDocumento(dataEmissao, numeroDocumento, contratadoId);
            if (!existeNumeroDocumento) tituloPagarAppService.ExisteNumeroDocumento(dataEmissao, dataVencimento, numeroDocumento, contratadoId);

            return Json(new
            {
                existe = existeNumeroDocumento
            });
        }

        #endregion

    }
}
