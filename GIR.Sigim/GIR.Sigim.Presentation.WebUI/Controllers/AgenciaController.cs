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
using GIR.Sigim.Application.Constantes;

namespace GIR.Sigim.Presentation.WebUI.Controllers
{
    public class AgenciaController : BaseController
    {
        #region Declaration

        private IBancoAppService bancoAppService;
        private IAgenciaAppService agenciaAppService;
        private IUnidadeFederacaoAppService unidadeFederacaoAppService;
        
        #endregion

        #region Constructor

        public AgenciaController(IBancoAppService bancoAppService,
                                 IAgenciaAppService agenciaAppService,  
                                 IUnidadeFederacaoAppService unidadeFederacaoAppService,
                                 MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.bancoAppService = bancoAppService;
            this.agenciaAppService = agenciaAppService;
            this.unidadeFederacaoAppService = unidadeFederacaoAppService;
        }

        #endregion

        #region Methods

        [Authorize(Roles = Funcionalidade.AgenciaAcessar)]
        public ActionResult Index(int? id)
        {
            var model = Session["Filtro"] as AgenciaListaViewModel;
            if (model == null)
            {
                model = new AgenciaListaViewModel();
                model.Filtro.PaginationParameters.PageSize = this.DefaultPageSize;
                model.Filtro.PaginationParameters.UniqueIdentifier = GenerateUniqueIdentifier();
            }
            model.EhValidoImprimir = false;

            if (id.HasValue)
            {
                model.Filtro.BancoId = id.Value;
            }
            CarregarCombosFiltro(model);

            return View(model);
        }

        private void CarregarCombosFiltro(AgenciaListaViewModel model)
        {
            int? bancoId = null;
          
            if (model.Filtro != null)
            {
                bancoId = model.Filtro.BancoId;
            }

            model.ListaBanco = new SelectList(bancoAppService.ListarTodos().OrderBy(l => l.Nome).ToList(), "Id", "Nome", bancoId);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Lista(AgenciaListaViewModel model)
        {
            if (ModelState.IsValid)
            {
                Session["Filtro"] = model;
                int totalRegistros;

                if (string.IsNullOrEmpty(model.Filtro.PaginationParameters.OrderBy))
                    model.Filtro.PaginationParameters.OrderBy = "id";

                var result = agenciaAppService.ListarPeloFiltro(model.Filtro, Usuario.Id, out totalRegistros);

                if (result.Any())
                {
                    model.EhValidoImprimir = true;
                    var listaViewModel = CreateListaViewModel(model.Filtro.PaginationParameters, totalRegistros, result);
                    return PartialView("ListaPartial", listaViewModel);
                }
                return PartialView("_EmptyListPartial");
            }
            return PartialView("_NotificationMessagesPartial");
        }


        public ActionResult Cadastro(int? id)
        {
            AgenciaCadastroViewModel model = new AgenciaCadastroViewModel();
            model.BancoIdPesquisado = null;

            var modelLista = Session["Filtro"] as AgenciaListaViewModel;
            if (modelLista != null && modelLista.Filtro != null)
            {
                model.BancoIdPesquisado = modelLista.Filtro.BancoId;
                model.EhValidoImprimir = modelLista.EhValidoImprimir;
            }

            model.PodeSalvar = agenciaAppService.EhPermitidoSalvar();
            model.PodeDeletar = agenciaAppService.EhPermitidoDeletar();
            model.PodeImprimir = agenciaAppService.EhPermitidoImprimir();
            model.PodeAcessarContaCorrente = agenciaAppService.EhPermitidoAcessarContaCorrente();

            var agencia = agenciaAppService.ObterPeloId(id) ?? new AgenciaDTO();

            if (id.HasValue && !agencia.Id.HasValue)
                messageQueue.Add(Application.Resource.Sigim.ErrorMessages.NenhumRegistroEncontrado, TypeMessage.Error);

            model.Agencia = agencia;

            CarregarCombosCadastro(model);

            return View(model);
        }

        private void CarregarCombosCadastro(AgenciaCadastroViewModel model)
        {
            int? bancoId = null;

            if (model.Agencia != null)
            {
                bancoId = model.Agencia.BancoId;
            }

            model.ListaBanco = new SelectList(bancoAppService.ListarTodos().OrderBy(l => l.Nome).ToList() , "Id", "Nome", bancoId);

            model.ListaUnidadeFederacao = new SelectList(unidadeFederacaoAppService.ListarTodos(), "Sigla", "Sigla", "");
        }

        public ActionResult CadastroAgencia(int? idBanco)
        {

            return PartialView("Redirect", Url.Action("Index", "Agencia", new { id = idBanco }));           
                      
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastro(AgenciaCadastroViewModel model)
        {
            if (ModelState.IsValid)
            {                
                agenciaAppService.Salvar(model.Agencia);
            }
            return PartialView("_NotificationMessagesPartial");            
            
        }

        [HttpPost]
        public ActionResult Deletar(int? id)
        {
            agenciaAppService.Deletar(id);
            return PartialView("_NotificationMessagesPartial");
        }

        public ActionResult Imprimir(int? bancoId, FormatoExportacaoArquivo formato)
        {
            var arquivo = agenciaAppService.ExportarRelAgencia(bancoId, formato);
            if (arquivo != null)
            {
                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();
                return File(arquivo.Stream, arquivo.ContentType, arquivo.NomeComExtensao);
            }

            return PartialView("_NotificationMessagesPartial");
        }

        #endregion
    }
     
}