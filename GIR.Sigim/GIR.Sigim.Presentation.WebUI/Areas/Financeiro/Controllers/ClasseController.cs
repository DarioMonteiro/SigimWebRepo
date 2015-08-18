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
    public class ClasseController : BaseController
    {
        private IClasseAppService classeAppService;

        public ClasseController(IClasseAppService classeAppService, MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.classeAppService = classeAppService;
        }

        [HttpPost]
        public ActionResult ValidaClasse(string codigo, int orcamentoId, bool somenteNivelFolha)
        {
            var classe = classeAppService.ObterPeloCodigoEOrcamento(codigo, orcamentoId);
            if (!string.IsNullOrEmpty(codigo))
            {
                if (somenteNivelFolha && !classeAppService.EhClasseUltimoNivelValida(classe, orcamentoId)
                    || !classeAppService.EhClasseValida(classe, orcamentoId))
                {
                    var msg = messageQueue.GetAll()[0].Text;
                    messageQueue.Clear();
                    return Json(new { ehValido = false, errorMessage = msg, descricao = string.Empty });
                }
                return Json(new { ehValido = true, errorMessage = string.Empty, descricao = classe.Descricao });
            }
            return Json(new { ehValido = true, errorMessage = string.Empty, descricao = string.Empty });
        }

        [HttpPost]
        public ActionResult TreeView(int? orcamentoId)
        {
            var model = classeAppService.ListarPeloOrcamento(orcamentoId);
            ViewBag.FirstNode = "Classe";
            return View("~/Views/Shared/EditorTemplates/TreeView.cshtml",model);
        }
    }
}