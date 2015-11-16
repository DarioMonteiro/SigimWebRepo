using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Application.Service.Sigim;
using GIR.Sigim.Application.Enums;
using GIR.Sigim.Application.Filtros.Sigim;
using GIR.Sigim.Application.DTO.Sigim;

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
            if (clienteFornecedorModulo == ClienteFornecedorModuloAutoComplete.APagar)
            {
                if (situacao == SituacaoAutoComplete.Ativo)
                {
                    var model = clienteFornecedorAppService.ListarClienteAPagarAtivosPorNome(nome);
                    return Json(model);
                }
            }

            if (clienteFornecedorModulo == ClienteFornecedorModuloAutoComplete.Todos)
            {
                if (situacao == SituacaoAutoComplete.Ativo)
                {
                    var model = clienteFornecedorAppService.ListarClienteTodosModulosAtivosPorNome(nome);
                    return Json(model);
                }
            }

            return Json(null);
        }

        [HttpPost]
        public ActionResult PesquisarClienteFornecedor(ClienteFornecedorPesquisaFiltro filtro,
                                                       ClienteFornecedorModuloAutoComplete clienteFornecedorModulo,
                                                       SituacaoAutoComplete situacao)
        {
            int totalRegistros = 0;
            List<ClienteFornecedorDTO> result = null;
            if (clienteFornecedorModulo == ClienteFornecedorModuloAutoComplete.Contrato)
            {
                if (situacao == SituacaoAutoComplete.Ativo)
                {
                    result = clienteFornecedorAppService.PesquisarClientesDeContratoAtivosPeloFiltro(filtro, out totalRegistros);
                }
            }
            if (clienteFornecedorModulo == ClienteFornecedorModuloAutoComplete.OrdemCompra)
            {
                if (situacao == SituacaoAutoComplete.Ativo)
                {
                    result = clienteFornecedorAppService.PesquisarClientesDeOrdemCompraAtivosPeloFiltro(filtro, out totalRegistros);
                }
            }
            if (clienteFornecedorModulo == ClienteFornecedorModuloAutoComplete.APagar)
            {
                if (situacao == SituacaoAutoComplete.Ativo)
                {
                    result = clienteFornecedorAppService.PesquisarClientesAPagarAtivosPeloFiltro(filtro, out totalRegistros);
                }
            }

            if (clienteFornecedorModulo == ClienteFornecedorModuloAutoComplete.Todos)
            {
                if (situacao == SituacaoAutoComplete.Ativo)
                {
                    result = clienteFornecedorAppService.PesquisarClientesDeTodosOsModulosAtivosPeloFiltro(filtro, out totalRegistros);
                }
            }


            if (result.Any())
            {
                var listaViewModel = CreateListaViewModel(filtro, totalRegistros, result);
                return PartialView("ListaPesquisaPartial", listaViewModel);
            }
            return PartialView("_EmptyListPartial");
        }

    }
}
