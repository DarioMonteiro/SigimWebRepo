using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Application.Service.Sigim;

namespace GIR.Sigim.Presentation.WebUI.Controllers
{
    public class ClienteFornecedorController : BaseController
    {
        private IClienteFornecedorAppService clienteFornecedorAppService;

        public ClienteFornecedorController(IClienteFornecedorAppService clienteFornecedorAppService,
                                           MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.clienteFornecedorAppService = clienteFornecedorAppService;
        }

        [HttpPost]
        public ActionResult ListarClienteContratoAtivosPorNome(string nome)
        {
            var model = clienteFornecedorAppService.ListarClienteContratoAtivosPorNome(nome);
            return Json(model);
        }

    }
}
