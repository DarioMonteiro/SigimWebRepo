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
        private IClasseAppService classeService;

        public ClasseController(IClasseAppService ClasseService, MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.classeService = ClasseService;
        }

        [HttpPost]
        public ActionResult ValidaClasse(string codigo)
        {
            var Classe = classeService.ObterPeloCodigo(codigo);
            if (!string.IsNullOrEmpty(codigo))
            {
                if (!classeService.EhClasseUltimoNivelValida(Classe))
                {
                    var msg = messageQueue.GetAll()[0].Text;
                    messageQueue.Clear();
                    return Json(new { ehValido = false, errorMessage = msg, descricao = string.Empty });
                }
                return Json(new { ehValido = true, errorMessage = string.Empty, descricao = Classe.Descricao });
            }
            return Json(new { ehValido = true, errorMessage = string.Empty, descricao = string.Empty });
        }

        [HttpPost]
        public ActionResult TreeView()
        {
            var model = classeService.ListarRaizes();
            ViewBag.FirstNode = "Classe";
            return View(model);
        }
    }
}