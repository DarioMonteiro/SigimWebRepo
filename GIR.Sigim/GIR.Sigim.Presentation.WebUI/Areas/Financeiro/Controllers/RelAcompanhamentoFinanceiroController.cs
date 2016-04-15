using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Presentation.WebUI.Controllers;
using GIR.Sigim.Presentation.WebUI.CustomAttributes;
using GIR.Sigim.Application.Constantes;
using GIR.Sigim.Presentation.WebUI.Areas.Financeiro.ViewModel;
using Newtonsoft.Json;
using GIR.Sigim.Application.DTO.Financeiro;
using GIR.Sigim.Application.Service.Sigim;

namespace GIR.Sigim.Presentation.WebUI.Areas.Financeiro.Controllers
{
    public class RelAcompanhamentoFinanceiroController : BaseController
    {
        public IIndiceFinanceiroAppService indiceFinanceiroAppService;

        public RelAcompanhamentoFinanceiroController(IIndiceFinanceiroAppService indiceFinanceiroAppService,
                                                     MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.indiceFinanceiroAppService = indiceFinanceiroAppService;
        }

        #region "Métodos Públicos"

        [AutorizacaoAcessoAuthorize(GIR.Sigim.Application.Constantes.Modulo.FinanceiroWeb, Roles = Funcionalidade.RelatorioAcompanhamentoFinanceiroAcessar)]
        public ActionResult Index()
        {
            var model = new RelAcompanhamentoFinanceiroListaViewModel();
            model.Filtro.PaginationParameters.PageSize = this.DefaultPageSize;
            model.Filtro.PaginationParameters.UniqueIdentifier = GenerateUniqueIdentifier();
            model.Filtro.DataInicial = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            model.Filtro.DataFinal = DateTime.Now;

            //model.PodeImprimir = apropriacaoAppService.EhPermitidoImprimirRelApropriacaoPorClasse();
            model.PodeImprimir = false;

            CarregarListas(model);

            model.JsonItensClasse = JsonConvert.SerializeObject(new List<ClasseDTO>());

            return View(model);
        }

        #endregion

        #region "Métodos Privados"

        private void CarregarListas(RelAcompanhamentoFinanceiroListaViewModel model)
        {
            model.ListaIndice = new SelectList(indiceFinanceiroAppService.ListarTodos().OrderBy(l => l.Indice), "Id", "Indice", model.Filtro.IndiceId);
        }

        #endregion

    }
}
