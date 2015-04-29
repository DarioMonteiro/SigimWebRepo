using System;
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
        private IContratoRetificacaoItemMedicaoAppService contratoRetificacaoItemMedicaoAppService;
        private IContratoRetificacaoAppService contratoRetificacaoAppService;

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
                                         IContratoRetificacaoItemMedicaoAppService contratoRetificacaoItemMedicaoAppService,
                                         IContratoRetificacaoAppService contratoRetificacaoAppService,
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
            this.contratoRetificacaoItemMedicaoAppService = contratoRetificacaoItemMedicaoAppService;
            this.contratoRetificacaoAppService = contratoRetificacaoAppService;
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

        public ActionResult Medicao(int? id)
        {
            MedicaoContratoMedicaoViewModel model = new MedicaoContratoMedicaoViewModel();
            ICollection<ContratoRetificacaoItemDTO> ListaItensUltimoContratoRetificacao = new HashSet<ContratoRetificacaoItemDTO>(); 

            var contrato = contratoAppService.ObterPeloId(id, Usuario.Id) ?? new ContratoDTO();
            model.ListaServicoContratoRetificacaoItem = new SelectList(new List<ContratoRetificacaoItemDTO>(), "Id", "SequencialDescricaoItemComplemento");

            CarregarCombosMedicao(model); 

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

            if (!contratoAppService.EhContratoAssinado(contrato))
            {
                return View(model);
            }

            if (contratoRetificacao.RetencaoContratual.HasValue)
            {
                model.RetencaoContratual = contratoRetificacao.RetencaoContratual;
            }

            ListaItensUltimoContratoRetificacao = contratoRetificacao.ListaContratoRetificacaoItem;

            model.ListaServicoContratoRetificacaoItem = new SelectList(ListaItensUltimoContratoRetificacao, "Id", "SequencialDescricaoItemComplemento", ListaItensUltimoContratoRetificacao.Select(c => c.Id));

            return View(model);
        }


        [HttpPost]
        public ActionResult RecuperaContratoRetificacaoItem(int? codigo)
        {
            List<ContratoRetificacaoProvisaoDTO> listaContratoRetificacaoProvisao = null;
            ContratoRetificacaoItemDTO contratoRetificacaoItem = null;

            if (codigo.HasValue)
            {

                //decimal qtd1=0;
                //decimal val1=0;
                //decimal qtd2 = 0;
                //decimal val2 = 0;

                //contratoRetificacaoItemMedicaoAppService.ObterQuantidadesEhValoresMedicao(codigo.Value,178,ref qtd1,ref val1,ref qtd2,ref val2);

                listaContratoRetificacaoProvisao = contratoRetificacaoProvisaoAppService.ObterListaCronograma(codigo.Value);

                if (!contratoRetificacaoProvisaoAppService.ExisteContratoRetificacaoProvisao(listaContratoRetificacaoProvisao))
                {
                    var msg = messageQueue.GetAll()[0].Text;
                    messageQueue.Clear();
                    return Json(new
                                {
                                    ehRecuperou = false,
                                    errorMessage = msg,
                                    contratoRetificacaoItem = contratoRetificacaoItem,
                                    listaContratoRetificacaoProvisao = listaContratoRetificacaoProvisao
                                });
                }
                else
                {
                    contratoRetificacaoItem = listaContratoRetificacaoProvisao.ElementAt(0).ContratoRetificacaoItem;

                    return Json(new
                    {
                        ehRecuperou = true,
                        errorMessage = string.Empty,
                        contratoRetificacaoItem = contratoRetificacaoItem,
                        listaContratoRetificacaoProvisao = Newtonsoft.Json.JsonConvert.SerializeObject(listaContratoRetificacaoProvisao)
                    });
                }
            }
            return Json(new 
            {
                ehRecuperou = true, 
                errorMessage = string.Empty, 
                contratoRetificacaoItem = contratoRetificacaoItem,
                listaContratoRetificacaoProvisao = listaContratoRetificacaoProvisao
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

            tipoDocumentoId = model.TipoDocumentoId;

            if (model.MultiFornecedorId.HasValue)
            {
                multifornecedorId = model.MultiFornecedorId.Value;
            }

            tipoCompraCodigo = model.TipoCompraCodigo;

            if (model.CifFobId.HasValue)
            {
                cifFobId = model.CifFobId.Value;
            }

            if (model.SerieNFId.HasValue)
            {
                serieNFId = model.SerieNFId.Value;
            }

            naturezaOperacaoCodigo = model.NaturezaOperacaoCodigo;
            cstCodigo = model.CSTCodigo;
            codigoContribuicaoCodigo = model.CodigoContribuicaoCodigo;

            model.ListaTipoDocumento = new SelectList(tipoDocumentoAppService.ListarTodos(), "Id", "Sigla", tipoDocumentoId);
            model.ListaMultiFornecedor = new SelectList(clienteFornecedorAppService.ListarAtivosDeContrato(), "Id", "Nome", multifornecedorId);
            model.ListaTipoCompra = new SelectList(tipoCompraAppService.ListarTodos(), "Codigo", "Descricao", tipoCompraCodigo);
            model.ListaCifFob = new SelectList(cifFobAppService.ListarTodos(), "Id", "Descricao", cifFobId);
            model.ListaNaturezaOperacao = new SelectList(naturezaOperacaoAppService.ListarTodos(), "Codigo", "Descricao", naturezaOperacaoCodigo);
            model.ListaSerieNF = new SelectList(serieNFAppService.ListarTodos(), "Id", "Descricao", serieNFId);
            model.ListaCST = new SelectList(cstAppService.ListarTodos(), "Codigo", "Descricao", cstCodigo);
            model.ListaCodigoContribuicao = new SelectList(codigoContribuicaoAppService.ListarTodos(), "Codigo", "Descricao", codigoContribuicaoCodigo);
        }

        #endregion

    }
}
