﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Application.Filtros;
using GIR.Sigim.Application.Service.Sigim;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Presentation.WebUI.ViewModel;
using GIR.Sigim.Presentation.WebUI.Controllers;

namespace GIR.Sigim.Presentation.WebUI.Controllers
{
    public class BancoController : BaseController
    {
        #region Declaration

        private IBancoAppService bancoAppService;

        #endregion

        #region Constructor

        public BancoController(
            IBancoAppService bancoAppService,            
            MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.bancoAppService = bancoAppService;            
        }

        #endregion

        #region Methods
        
        public ActionResult Index(int? id)
        {
            var model = Session["Filtro"] as BancoViewModel;
            if (model == null)
            {
                model = new BancoViewModel();
                model.Filtro.PaginationParameters.PageSize = this.DefaultPageSize;
            }

            var banco = bancoAppService.ObterPeloId(id) ?? new BancoDTO(); ;

            if (id.HasValue && !banco.Id.HasValue)
                messageQueue.Add(Application.Resource.Sigim.ErrorMessages.NenhumRegistroEncontrado, TypeMessage.Error);

            model.Banco = banco;           

            return View(model);
        }

        public ActionResult CarregarItem(int? id)
        {
            var banco = bancoAppService.ObterPeloId(id) ?? new BancoDTO();
            return Json(banco);
        }        

        public ActionResult Lista(BancoViewModel model)
        {
            if (ModelState.IsValid)
            {
                Session["Filtro"] = model;
                int totalRegistros;
                var result = bancoAppService.ListarPeloFiltro(model.Filtro, out totalRegistros);
                if (result.Any())
                {
                    var listaViewModel = CreateListaViewModel(model.Filtro.PaginationParameters, totalRegistros, result);
                    return PartialView("ListaPartial", listaViewModel);
                }
                return PartialView("_EmptyListPartial");
            }
            return PartialView("_NotificationMessagesPartial");
        }

        
        [HttpPost]
        public ActionResult Salvar(BancoViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Banco.BancoCodigo == "999")               
                    messageQueue.Add(Application.Resource.Sigim.ErrorMessages.BancoCarteira, TypeMessage.Info);
                else
                    bancoAppService.Salvar(model.Banco);                              
            }
            return PartialView("_NotificationMessagesPartial");
        }

        [HttpPost]
        public ActionResult Deletar(string id)
        {          
            bancoAppService.Deletar(Int32.Parse(id));
            return PartialView("_NotificationMessagesPartial");
        }

       
        #endregion
    }
}