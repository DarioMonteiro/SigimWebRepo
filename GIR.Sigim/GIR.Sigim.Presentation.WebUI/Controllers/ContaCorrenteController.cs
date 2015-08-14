using System;
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
    public class ContaCorrenteController : BaseController
    {
        #region Declaration

        private IBancoAppService bancoAppService;
        private IAgenciaAppService agenciaAppService;
        private IContaCorrenteAppService contaCorrenteAppService;
        
        #endregion

        #region Constructor

        public ContaCorrenteController(
            IBancoAppService bancoAppService,
            IAgenciaAppService agenciaAppService,  
            IContaCorrenteAppService contaCorrenteAppService,
            MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.bancoAppService = bancoAppService;
            this.agenciaAppService = agenciaAppService;
            this.contaCorrenteAppService = contaCorrenteAppService;      
        }

        #endregion

        #region Methods

        [Authorize(Roles = Funcionalidade.ContaCorrenteAcessar)]
        public ActionResult Index(int? id)
        {
            var model = Session["Filtro"] as ContaCorrenteListaViewModel;
            if (model == null)
            {
                model = new ContaCorrenteListaViewModel();
                model.Filtro.PaginationParameters.PageSize = this.DefaultPageSize;
                model.Filtro.PaginationParameters.UniqueIdentifier = GenerateUniqueIdentifier();
            }
            
            if (id.HasValue)
            {
                model.Filtro.BancoId = id.Value;
            }
            CarregarCombosFiltro(model);

            return View(model);
        }

        private void CarregarCombosFiltro(ContaCorrenteListaViewModel model)
        {
            int? bancoId = null;
          
            if (model.Filtro != null)
            {
                bancoId = model.Filtro.BancoId;
            }

            model.ListaBanco = new SelectList(bancoAppService.ListarTodos(), "Id", "Nome", bancoId);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Lista(ContaCorrenteListaViewModel model)
        {
            if (ModelState.IsValid)
            {
                Session["Filtro"] = model;
                int totalRegistros;

                if (string.IsNullOrEmpty(model.Filtro.PaginationParameters.OrderBy))
                    model.Filtro.PaginationParameters.OrderBy = "id";

                var result = contaCorrenteAppService.ListarPeloFiltro(model.Filtro, Usuario.Id, out totalRegistros);
                if (result.Any())
                {
                    var listaViewModel = CreateListaViewModel(model.Filtro.PaginationParameters, totalRegistros, result);
                    return PartialView("ListaPartial", listaViewModel);
                }
                return PartialView("_EmptyListPartial");
            }
            return PartialView("_NotificationMessagesPartial");
        }


        //public ActionResult Cadastro(int? id)
        //{
        //    AgenciaCadastroViewModel model = new AgenciaCadastroViewModel();
        //    var agencia = agenciaAppService.ObterPeloId(id) ?? new AgenciaDTO();

        //    if (id.HasValue && !agencia.Id.HasValue)
        //        messageQueue.Add(Application.Resource.Sigim.ErrorMessages.NenhumRegistroEncontrado, TypeMessage.Error);

        //    model.Agencia = agencia;

        //    CarregarComboBanco(model);

        //    return View(model);
        //}

        //private void CarregarComboBanco(AgenciaCadastroViewModel model)
        //{
        //    int? bancoId = null;

        //    if (model.Agencia != null)
        //    {
        //        bancoId = model.Agencia.BancoId;
        //    }

        //    model.ListaBanco = new SelectList(bancoAppService.ListarTodos(), "Id", "Nome", bancoId);
        //}


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Cadastro(AgenciaCadastroViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {                
        //        agenciaAppService.Salvar(model.Agencia);
        //    }
        //    return PartialView("_NotificationMessagesPartial");            
            
        //}

        //[HttpPost]
        //public ActionResult Deletar(int? id)
        //{
        //    agenciaAppService.Deletar(id);
        //    return PartialView("_NotificationMessagesPartial");
        //}

        #endregion
    }
     
}