using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Presentation.WebUI.Controllers;
using GIR.Sigim.Application.Filtros.OrdemCompras;
using GIR.Sigim.Application.Service.OrdemCompra;
using GIR.Sigim.Application.DTO.OrdemCompra;

namespace GIR.Sigim.Presentation.WebUI.Areas.OrdemCompra.Controllers
{
    public class OrdemCompraController : BaseController
    {
        #region Declaração

        private IOrdemCompraAppService ordemCompraAppService;

        #endregion

        #region Constructor

        public OrdemCompraController(IOrdemCompraAppService ordemCompraAppService, MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.ordemCompraAppService = ordemCompraAppService;
        }

        #endregion

        #region Métodos públicos

        [HttpPost]
        public ActionResult PesquisarOrdensCompra(OrdemCompraPesquisaFiltro filtro)
        {
            int totalRegistros = 0;
            List<OrdemCompraDTO> result = ordemCompraAppService.PesquisarOrdensCompraPeloFiltro(filtro, out totalRegistros);

            if (result.Any())
            {
                var listaViewModel = CreateListaViewModel(filtro, totalRegistros, result);
                return PartialView("ListaPesquisaPartial", listaViewModel);
            }
            return PartialView("_EmptyListPartial");
        }

        #endregion

    }
}
