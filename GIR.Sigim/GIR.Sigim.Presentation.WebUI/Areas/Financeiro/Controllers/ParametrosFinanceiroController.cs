using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GIR.Sigim.Application.DTO.Financeiro;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Application.Service.Financeiro;
using GIR.Sigim.Application.Service.Sigim;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Presentation.WebUI.Areas.Financeiro.ViewModel;
using GIR.Sigim.Presentation.WebUI.Controllers;

namespace GIR.Sigim.Presentation.WebUI.Areas.Financeiro.Controllers
{
    public class ParametrosFinanceiroController : BaseController
    {
        private IParametrosFinanceiroAppService parametrosAppService;
        private IClienteFornecedorAppService clienteFornecedorAppService;

        public ParametrosFinanceiroController(
            IParametrosFinanceiroAppService parametrosAppService,
            IClienteFornecedorAppService clienteFornecedorAppService,
            MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.parametrosAppService = parametrosAppService;
            this.clienteFornecedorAppService = clienteFornecedorAppService;
        }

        public ActionResult Index()
        {
            ParametrosFinanceiroViewModel model = new ParametrosFinanceiroViewModel();
            model.Parametros = parametrosAppService.Obter();
            CarregarCombos(model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(ParametrosFinanceiroViewModel model)
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
                        model.Parametros.IconeRelatorio = memoryStream.ToArray();
                    }
                }
                parametrosAppService.Salvar(model.Parametros);
            }

            return PartialView("_NotificationMessagesPartial");
        }

        private void CarregarCombos(ParametrosFinanceiroViewModel model)
        {
            model.ListaEmpresa = new SelectList(clienteFornecedorAppService.ListarAtivos(), "Id", "Nome", model.Parametros.ClienteId);
        }
    }
}
