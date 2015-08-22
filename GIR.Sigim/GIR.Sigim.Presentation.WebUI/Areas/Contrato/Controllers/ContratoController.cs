using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Presentation.WebUI.Controllers;
using GIR.Sigim.Application.Filtros.Contrato;
using GIR.Sigim.Application.Service.Contrato;
using GIR.Sigim.Application.DTO.Contrato;

namespace GIR.Sigim.Presentation.WebUI.Areas.Contrato.Controllers
{
    public class ContratoController : BaseController
    {
        #region Declaração

        private IContratoAppService contratoAppService;

        #endregion

        #region Constructor

        public ContratoController(IContratoAppService contratoAppService,MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.contratoAppService = contratoAppService;
        }

        #endregion

        #region Métodos públicos

        [HttpPost]
        public ActionResult PesquisarContratos(ContratoPesquisaFiltro filtro)
        {
            int totalRegistros = 0;
            List<ContratoDTO> result = contratoAppService.PesquisarContratosPeloFiltro(filtro, out totalRegistros);

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
