using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GIR.Sigim.Application.Service.Sigim;
using GIR.Sigim.Application.Service.Contrato;
using GIR.Sigim.Application.Service.Financeiro;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;    
using GIR.Sigim.Presentation.WebUI.Controllers;
using GIR.Sigim.Presentation.WebUI.Areas.Contrato.ViewModel;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Application.DTO.Contrato;

namespace GIR.Sigim.Presentation.WebUI.Areas.Contrato.Controllers
{
    public class MedicaoContratoController : BaseController
    {

        #region Declaration

        private IClienteFornecedorAppService clienteFornecedorAppService;
        private IContratoAppService contratoAppService;
        private IContratoRetificacaoItemAppService contratoRetificacaoItemAppService;
        private ITipoDocumentoAppService tipoDocumentoAppService;
        private ITipoCompraAppService tipoCompraAppService;
        private ICifFobAppService cifFobAppService;
        private INaturezaOperacaoAppService naturezaOperacaoAppService;
        private ISerieNFAppService serieNFAppService;
        private ICSTAppService cstAppService;
        private ICodigoContribuicaoAppService codigoContribuicaoAppService;

        #endregion

        #region Constructor

        public MedicaoContratoController(IClienteFornecedorAppService clienteFornecedorAppService,
                                         IContratoAppService contratoAppService, 
                                         IContratoRetificacaoItemAppService contratoRetificacaoItemAppService,
                                         ITipoDocumentoAppService tipoDocumentoAppService,
                                         ITipoCompraAppService tipoCompraAppService,
                                         ICifFobAppService cifFobAppService,
                                         INaturezaOperacaoAppService naturezaOperacaoAppService,
                                         ISerieNFAppService serieNFAppService,
                                         ICSTAppService cstAppService,
                                         ICodigoContribuicaoAppService codigoContribuicaoAppService,
                                         MessageQueue messageQueue) 
            : base(messageQueue) 
        {
            this.clienteFornecedorAppService = clienteFornecedorAppService;
            this.contratoAppService = contratoAppService;
            this.contratoRetificacaoItemAppService = contratoRetificacaoItemAppService;
            this.tipoDocumentoAppService = tipoDocumentoAppService;
            this.tipoCompraAppService = tipoCompraAppService;
            this.cifFobAppService = cifFobAppService;
            this.naturezaOperacaoAppService = naturezaOperacaoAppService;
            this.serieNFAppService = serieNFAppService;
            this.cstAppService = cstAppService;
            this.codigoContribuicaoAppService = codigoContribuicaoAppService;
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
            var contratoDTO = contratoAppService.ObterPeloId(id, Usuario.Id) ?? new ContratoDTO();

            if (id.HasValue && !contratoDTO.Id.HasValue)
                messageQueue.Add(Application.Resource.Sigim.ErrorMessages.NenhumRegistroEncontrado, TypeMessage.Error);

            if (!contratoDTO.CentroCusto.Ativo)
                messageQueue.Add(Application.Resource.Financeiro.ErrorMessages.CentroCustoInativo, TypeMessage.Error);

            model.Contrato = contratoDTO;

            ICollection<ContratoRetificacaoItemDTO> ListaItensUltimoContratoRetificacao = new HashSet<ContratoRetificacaoItemDTO>(); 

            ContratoRetificacaoDTO contratoRetificacao = contratoDTO.ListaContratoRetificacao.Last();

            if (contratoRetificacao.RetencaoContratual.HasValue)
            {
                model.RetencaoContratual = contratoRetificacao.RetencaoContratual;
            }

            if (!contratoRetificacao.Aprovada)
                messageQueue.Add(Application.Resource.Contrato.ErrorMessages.RetificacaoNaoAprovada, TypeMessage.Error);

            ListaItensUltimoContratoRetificacao = contratoRetificacao.ListaContratoRetificacaoItem;

            model.ListaServicoContratoRetificacaoItem = new SelectList(ListaItensUltimoContratoRetificacao, "Id", "SequencialDescricaoItemComplemento", ListaItensUltimoContratoRetificacao.Select(c => c.Id));

            CarregarCombosMedicao(model); 

            return View(model);
        }


        [HttpPost]
        public ActionResult RecuperaContratoRetificacaoItem(int? codigo)
        {
            if (codigo.HasValue)
            {
                ContratoRetificacaoItemDTO contratoRetificacaoItem = contratoRetificacaoItemAppService.ObterPeloId(codigo.Value);
                if (contratoRetificacaoItem != null)
                {
                    return Json(new
                    {
                        ehRecuperouContratoRetificacaoItem = true,
                        errorMessage = string.Empty,
                        retencaoItem = contratoRetificacaoItem.RetencaoItem,
                        siglaUnidadeMedida = contratoRetificacaoItem.Servico.UnidadeMedida.Sigla,
                        complementoDescricao = contratoRetificacaoItem.ComplementoDescricao,
                        //valorContratadoItem = contratoRetificacaoItem.ValorItem.Value.ToString("###,###,###,##0.00"),
                        valorContratadoItem = contratoRetificacaoItem.ValorItem.Value,
                        descricaoNatureza = contratoRetificacaoItem.DescricaoNaturezaItem
                    });
                }
                return Json(new
                {
                    ehRecuperouContratoRetificacaoItem = false,
                    errorMessage = string.Empty,
                    retencaoItem = string.Empty,
                    siglaUnidadeMedida = string.Empty,
                    complementoDescricao = string.Empty,
                    valorContratadoItem = string.Empty,
                    descricaoNatureza = string.Empty
                });
            }
            return Json(new
            {
                ehRecuperouContratoRetificacaoItem = false,
                errorMessage = string.Empty,
                retencaoItem = string.Empty,
                siglaUnidadeMedida = string.Empty,
                complementoDescricao = string.Empty,
                valorContratadoItem = string.Empty,
                descricaoNatureza = string.Empty
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

            if (model.TipoDocumentoId.HasValue)
            {
                tipoDocumentoId = model.TipoDocumentoId.Value; 
            }

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
