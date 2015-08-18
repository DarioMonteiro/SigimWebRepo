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

        public ActionResult Index(int? id)
        {
            var model = Session["Filtro"] as ContaCorrenteListaViewModel;
            if (model == null)
            {
                model = new ContaCorrenteListaViewModel();
                model.Filtro.PaginationParameters.PageSize = this.DefaultPageSize;
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


        public ActionResult Cadastro(int? id)
        {
            ContaCorrenteCadastroViewModel model = new ContaCorrenteCadastroViewModel();
            var contaCorrente = contaCorrenteAppService.ObterPeloId(id) ?? new ContaCorrenteDTO();

            if (id.HasValue && !contaCorrente.Id.HasValue)
                messageQueue.Add(Application.Resource.Sigim.ErrorMessages.NenhumRegistroEncontrado, TypeMessage.Error);

            model.ContaCorrente = contaCorrente;

            CarregarCombos(model);

            return View(model);
        }

        private void CarregarCombos(ContaCorrenteCadastroViewModel model)
        {
            int? bancoId = null;
            int? agenciaId = null;

            if (model.ContaCorrente != null)
            {
                bancoId = model.ContaCorrente.BancoId;
                agenciaId = model.ContaCorrente.AgenciaId;
            }

            model.ListaBanco = new SelectList(bancoAppService.ListarTodos(), "Id", "Nome", bancoId);
            model.ListaAgencia = new SelectList(agenciaAppService.ListarPeloBanco(bancoId), "Id", "AgenciaCodigo", agenciaId);
            model.ListaTipo = new SelectList(contaCorrenteAppService.ListarTipo(), "Id", "Descricao", model.ContaCorrente.Tipo);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastro(ContaCorrenteCadastroViewModel model)
        {
            if (ModelState.IsValid)
            {
                contaCorrenteAppService.Salvar(model.ContaCorrente);
            }
            return PartialView("_NotificationMessagesPartial");

        }

        [HttpPost]
        public ActionResult Deletar(int? id)
        {
            contaCorrenteAppService.Deletar(id);
            return PartialView("_NotificationMessagesPartial");
        }


        [HttpPost]
        public ActionResult CarregaAgencia(int? id)
        {
            return Json(agenciaAppService.ListarPeloBanco(id));
        }

        #endregion
    }
     
}