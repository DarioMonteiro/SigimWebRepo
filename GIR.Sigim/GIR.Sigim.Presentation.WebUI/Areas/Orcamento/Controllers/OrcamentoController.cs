using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GIR.Sigim.Application.Service.Orcamento;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Presentation.WebUI.Controllers;
using GIR.Sigim.Application.DTO.Orcamento;
using GIR.Sigim.Application.Filtros.Orcamento;

namespace GIR.Sigim.Presentation.WebUI.Areas.Orcamento.Controllers
{
    public class OrcamentoController : BaseController
    {
        IOrcamentoAppService orcamentoAppService;

        public OrcamentoController(
            IOrcamentoAppService orcamentoAppService,
            MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.orcamentoAppService = orcamentoAppService;
        }

        public ActionResult ObterUltimoOrcamentoPeloCentroCusto(string codigoCentroCusto)
        {
            var orcamento = orcamentoAppService.ObterUltimoOrcamentoPeloCentroCusto(codigoCentroCusto);
            return Json(orcamento != null ? orcamento.Id.Value : 0);
        }

        public ActionResult ObterUltimoOrcamentoPeloCentroCustoClasseOrcamento(string codigoCentroCusto)
        {
            var orcamento = orcamentoAppService.ObterUltimoOrcamentoPeloCentroCustoClasseOrcamento(codigoCentroCusto);
            return Json(orcamento != null ? orcamento.Id.Value : 0);
        }

        [HttpPost]
        public ActionResult PesquisarOrcamentos(OrcamentoPesquisaFiltro filtro)
        {
            int totalRegistros = 0;

            if (string.IsNullOrEmpty(filtro.OrderBy))
                filtro.OrderBy = "";

            List<OrcamentoDTO> result = orcamentoAppService.PesquisarOrcamentosPeloFiltro(filtro, out totalRegistros);

            if (result.Any())
            {
                var listaViewModel = CreateListaViewModel(filtro, totalRegistros, result);
                return PartialView("ListaPesquisaPartial", listaViewModel);
            }
            return PartialView("_EmptyListPartial");
        }

    }
}