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
        private ICentroCustoAppService centroCustoAppService;

        public CentroCustoController(ICentroCustoAppService centroCustoAppService, MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.centroCustoAppService = centroCustoAppService;
        }

        [HttpPost]
        public ActionResult ValidaCentroCustoPorUsuario(string codigo, string modulo, bool somenteNivelFolha)
        {
            var centroCusto = centroCustoAppService.ObterPeloCodigo(codigo);
            if (!string.IsNullOrEmpty(codigo))
            {

                if ((somenteNivelFolha && !centroCustoAppService.EhCentroCustoUltimoNivelValido(centroCusto))
                    || !centroCustoAppService.EhCentroCustoValido(centroCusto)
                    || !centroCustoAppService.UsuarioPossuiAcessoCentroCusto(centroCusto, Usuario.Id, modulo))
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
            var model = centroCustoAppService.ListarRaizesAtivas();
            ViewBag.FirstNode = "Centro de Custo";
            return View(model);
        }
    }
}