using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Application.Service.Sigim;
using GIR.Sigim.Application.Enums;

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
        public ActionResult ListarClienteFornecedorPorNome(string nome,
                                                           ClienteFornecedorModuloAutoComplete clienteFornecedorModulo,
                                                           SituacaoAutoComplete situacao)
        {
            if (clienteFornecedorModulo == ClienteFornecedorModuloAutoComplete.Contrato){
                if (situacao == SituacaoAutoComplete.Ativo)
                {
                    var model = clienteFornecedorAppService.ListarClienteContratoAtivosPorNome(nome);
                    return Json(model);
                }
            }
            if (clienteFornecedorModulo == ClienteFornecedorModuloAutoComplete.OrdemCompra){
                if (situacao == SituacaoAutoComplete.Ativo)
                {
                    var model = clienteFornecedorAppService.ListarClienteOrdemCompraAtivosPorNome(nome);
                    return Json(model);
                }
            }

            return Json(null);
        }
    }
}
