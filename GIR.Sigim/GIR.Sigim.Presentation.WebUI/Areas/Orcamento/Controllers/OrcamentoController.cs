using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GIR.Sigim.Application.Service.Orcamento;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Presentation.WebUI.Controllers;

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
    }
}