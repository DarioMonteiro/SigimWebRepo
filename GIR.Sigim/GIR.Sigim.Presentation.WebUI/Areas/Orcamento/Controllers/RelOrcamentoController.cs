using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GIR.Sigim.Presentation.WebUI.Controllers;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Presentation.WebUI.CustomAttributes;
using GIR.Sigim.Application.Constantes;
using GIR.Sigim.Presentation.WebUI.Areas.Orcamento.ViewModel;
using Newtonsoft.Json;
using GIR.Sigim.Application.DTO.Financeiro;
using GIR.Sigim.Application.Service.Orcamento;
using GIR.Sigim.Application.DTO.Orcamento;
using GIR.Sigim.Application.Adapter;
using GIR.Sigim.Application.Service.Sigim;

namespace GIR.Sigim.Presentation.WebUI.Areas.Orcamento.Controllers
{
    public class RelOrcamentoController : BaseController
    {
        #region "Declaração"

        private IOrcamentoAppService orcamentoAppService;
        private IEmpresaAppService empresaAppService;
        private IIndiceFinanceiroAppService indiceFinanceiroAppService;

        #endregion

        #region "Construtor"

        public RelOrcamentoController(IOrcamentoAppService orcamentoAppService,
                                      IEmpresaAppService empresaAppService,
                                      IIndiceFinanceiroAppService indiceFinanceiroAppService,
                                      MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.empresaAppService = empresaAppService;
            this.orcamentoAppService = orcamentoAppService;
            this.indiceFinanceiroAppService = indiceFinanceiroAppService;
        }

        #endregion

        #region "Métodos públicos"

        [AutorizacaoAcessoAuthorize(GIR.Sigim.Application.Constantes.Modulo.OrcamentoWeb, Roles = Funcionalidade.RelatorioOrcamentoAcessar)]
        public ActionResult Index()
        {
            var model = new RelOrcamentoListaViewModel();
            model.Filtro.PaginationParameters.PageSize = this.DefaultPageSize;
            model.Filtro.PaginationParameters.UniqueIdentifier = GenerateUniqueIdentifier();

            model.PodeImprimir = orcamentoAppService.EhPermitidoImprimirRelOrcamento();

            CarregarListas(model);

            model.JsonItensClasse = JsonConvert.SerializeObject(new List<ClasseDTO>());

            return View(model);
        }

        public ActionResult CarregaObraPorEmpresa(int empresaId)
        {
            List<ObraDTO> lista = null;
            EmpresaDTO empresa = empresaAppService.ObterEmpresaSemObraPai(empresaId);
            if (empresa != null && empresa.ListaObraSemPai != null)
            {
                lista = empresa.ListaObraSemPai;
            }
            return Json(lista);
        }

        #endregion

        #region "Métodos privados"

        private void CarregarListas(RelOrcamentoListaViewModel model)
        {
            List<ObraDTO> lista = new List<ObraDTO>();
            EmpresaDTO empresa = empresaAppService.ObterEmpresaSemObraPai(model.Filtro.EmpresaId);
            if (empresa != null && empresa.ListaObraSemPai != null)
            {
                lista = empresa.ListaObraSemPai;
            }

            model.ListaEmpresa = new SelectList(empresaAppService.ListarTodos(), "Id", "NumeroNomeEmpresa", model.Filtro.EmpresaId);
            model.ListaObra = new SelectList(lista, "Id", "NumeroDescricao", model.Filtro.ObraId);
            model.ListaIndice = new SelectList(indiceFinanceiroAppService.ListarTodos().OrderBy(l => l.Indice), "Id", "Indice", model.Filtro.IndiceId);
        }

        #endregion

    }
}
