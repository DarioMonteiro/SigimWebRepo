using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GIR.Sigim.Application.DTO.Financeiro;
using GIR.Sigim.Application.Service.Financeiro;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Presentation.WebUI.Controllers;

namespace GIR.Sigim.Presentation.WebUI.Areas.Financeiro.Controllers
{
    public class CentroCustoController : BaseController
    {
        private ICentroCustoAppService centroCustoService;

        public CentroCustoController(ICentroCustoAppService centroCustoService, MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.centroCustoService = centroCustoService;
        }

        [HttpPost]
        public ActionResult ValidaCentroCustoPorUsuario(string codigo, string modulo)
        {
            var centroCusto = centroCustoService.ObterPeloCodigo(codigo);
            if (!string.IsNullOrEmpty(codigo))
            {
                if (!centroCustoService.EhCentroCustoUltimoNivelValido(centroCusto)
                    || !centroCustoService.UsuarioPossuiAcessoCentroCusto(centroCusto, Usuario.Id, modulo))
                {
                    var msg = messageQueue.GetAll()[0].Text;
                    messageQueue.Clear();
                    return Json(new { ehValido = false, errorMessage = msg, descricao = string.Empty });
                }
                return Json(new { ehValido = true, errorMessage = string.Empty, descricao = centroCusto.Descricao });
            }
            return Json(new { ehValido = true, errorMessage = string.Empty, descricao = string.Empty });
        }

        [HttpPost]
        public ActionResult TreeView()
        {
            var model = centroCustoService.ListarRaizesAtivas();
            ViewBag.FirstNode = "Centro de Custo";
            return View(model);
        }
    }
}