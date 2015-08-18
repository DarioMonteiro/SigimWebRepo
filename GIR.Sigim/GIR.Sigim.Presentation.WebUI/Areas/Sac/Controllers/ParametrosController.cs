using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using GIR.Sigim.Application.DTO.Sac;
using GIR.Sigim.Application.Service.Sac;
using GIR.Sigim.Application.Service.Sigim;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Presentation.WebUI.Areas.Sac.ViewModel;
using GIR.Sigim.Presentation.WebUI.Controllers;
using GIR.Sigim.Application.Constantes;

namespace GIR.Sigim.Presentation.WebUI.Areas.Sac.Controllers
{
    public class ParametrosController : BaseController
    {
        private IParametrosSacAppService parametrosAppService;
        private IClienteFornecedorAppService clienteFornecedorAppService;

        public ParametrosController(IParametrosSacAppService parametrosAppService,
                                    IClienteFornecedorAppService clienteFornecedorAppService,
                                    MessageQueue messageQueue)
                                    : base(messageQueue)
        {
            this.parametrosAppService = parametrosAppService;
            this.clienteFornecedorAppService = clienteFornecedorAppService;
        }

        [Authorize(Roles = Funcionalidade.ParametroSacAcessar)]
        public ActionResult Index()
        {
            ParametrosViewModel model = new ParametrosViewModel();
            var parametrosSac = parametrosAppService.Obter() ?? new ParametrosSacDTO();
            foreach (var parametrosEmail in parametrosSac.ListaParametrosEmailSac)
            {
                parametrosEmail.ParametrosSac = new ParametrosSacDTO();
            }
            model.ParametrosSac = parametrosSac; 
            model.JsonListaEmail = Newtonsoft.Json.JsonConvert.SerializeObject(model.ParametrosSac.ListaParametrosEmailSac);

            CarregarCombos(model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(ParametrosViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.IconeRelatorio != null)
                {
                    using (Stream inputStream = model.IconeRelatorio.InputStream)
                    {
                        MemoryStream memoryStream = inputStream as MemoryStream;
                        if (memoryStream == null)
                        {
                            memoryStream = new MemoryStream();
                            inputStream.CopyTo(memoryStream);
                        }
                        model.ParametrosSac.IconeRelatorio = memoryStream.ToArray();
                    }
                }
                parametrosAppService.Salvar(model.ParametrosSac);
            }

            return PartialView("_NotificationMessagesPartial");
        }
        private void CarregarCombos(ParametrosViewModel model)
        {
            int? clienteId = null;

            if (model.ParametrosSac != null)
            {
                clienteId = model.ParametrosSac.ClienteId;
            }
            model.ListaEmpresa = new SelectList(clienteFornecedorAppService.ListarAtivos(), "Id", "Nome", model.ParametrosSac.ClienteId);
        }
    }   
}
