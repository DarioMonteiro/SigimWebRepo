﻿using System;
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
    public class RateioAutomaticoController : BaseController
    {
        private IRateioAutomaticoAppService rateioAutomaticoAppService;
        private ITipoRateioAppService tipoRateioAppService;

        public RateioAutomaticoController(
            IRateioAutomaticoAppService rateioAutomaticoAppService,
            ITipoRateioAppService tipoRateioAppService,
            MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.rateioAutomaticoAppService = rateioAutomaticoAppService;
            this.tipoRateioAppService = tipoRateioAppService;
        }

      
        public ActionResult Index()
        {
            var model = Session["RateioAutomatico"] as RateioAutomaticoViewModel;

            if (model == null)
            {model = new RateioAutomaticoViewModel();}

            CarregarCombos(model);

            return View(model);
        }

        
        public ActionResult CarregarItem(int tipoRateio)
        {
            var listaRateioAutomaticoDTO = rateioAutomaticoAppService.ListarPeloTipoRateio(tipoRateio);
            return Json(listaRateioAutomaticoDTO);
        }

        [HttpPost]
        public ActionResult Salvar(RateioAutomaticoViewModel model)
        {
            List<RateioAutomaticoDTO> lista;

            if (ModelState.IsValid)
            {
                lista = Newtonsoft.Json.JsonConvert.DeserializeObject<List<RateioAutomaticoDTO>>(model.JsonItens);
                rateioAutomaticoAppService.Salvar(model.TipoRateioId, lista);
            }
            return PartialView("_NotificationMessagesPartial");
        }

        [HttpPost]
        public ActionResult Deletar(int tipoRateio)
        {
            rateioAutomaticoAppService.Deletar(tipoRateio);
            return PartialView("_NotificationMessagesPartial");
        }

        private void CarregarCombos(RateioAutomaticoViewModel model)
        {
            model.ListaTipoRateio = new SelectList(tipoRateioAppService.ListarTodos(), "Id", "Descricao", model.TipoRateioId);
        }

    }
}