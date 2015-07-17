using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Application.Service.Sigim;

namespace GIR.Sigim.Presentation.WebUI.Controllers
{
    public class MaterialClasseInsumoController : BaseController
    {
        private IMaterialClasseInsumoAppService materialClasseInsumoAppService;

        public MaterialClasseInsumoController(IMaterialClasseInsumoAppService materialClasseInsumoAppService, 
                                              MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.materialClasseInsumoAppService = materialClasseInsumoAppService;
        }

        [HttpPost]
        public ActionResult ValidaClasseInsumo(string codigo, bool somenteNivelFolha)
        {
            var classeInsumo = materialClasseInsumoAppService.ObterPeloCodigo(codigo);
            if (!string.IsNullOrEmpty(codigo))
            {
                if (somenteNivelFolha && !materialClasseInsumoAppService.EhClasseInsumoUltimoNivelValida(classeInsumo)
                    || !materialClasseInsumoAppService.EhClasseInsumoValida(classeInsumo))
                {
                    var msg = messageQueue.GetAll()[0].Text;
                    messageQueue.Clear();
                    return Json(new { ehValido = false, errorMessage = msg, descricao = string.Empty });
                }
                return Json(new { ehValido = true, errorMessage = string.Empty, descricao = classeInsumo.Descricao });
            }
            return Json(new { ehValido = true, errorMessage = string.Empty, descricao = string.Empty });
        }


        [HttpPost]
        public ActionResult TreeView()
        {
            var model = materialClasseInsumoAppService.ListarRaizes();
            ViewBag.FirstNode = "Classe Insumo";
            return View("~/Views/Shared/EditorTemplates/TreeView.cshtml",model);
        }

    }
}
