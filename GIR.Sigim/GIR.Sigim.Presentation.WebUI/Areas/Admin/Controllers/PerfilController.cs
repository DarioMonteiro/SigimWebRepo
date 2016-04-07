using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GIR.Sigim.Application.DTO.Admin;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Application.Service.Admin;
using GIR.Sigim.Application.Service.Sigim;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;
using GIR.Sigim.Presentation.WebUI.Areas.Admin.ViewModel;
using GIR.Sigim.Presentation.WebUI.Controllers;
using GIR.Sigim.Application.Filtros;
using Newtonsoft.Json;
using GIR.Sigim.Application.Constantes;

namespace GIR.Sigim.Presentation.WebUI.Areas.Admin.Controllers
{
    public class PerfilController : BaseController
    {
        private IPerfilAppService perfilAppService;
        private IModuloAppService moduloAppService;
        private IFuncionalidadeAppService funcionalidadeAppService;

        public PerfilController(
            IPerfilAppService perfilAppService,
            IModuloAppService moduloAppService,
            IFuncionalidadeAppService funcionalidadeAppService,
            MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.perfilAppService = perfilAppService;
            this.moduloAppService = moduloAppService;
            this.funcionalidadeAppService = funcionalidadeAppService;
        }

        [Authorize(Roles = Funcionalidade.PerfilAcessar)]
        public ActionResult Index()
        {
            var model = Session["Filtro"] as PerfilViewModel;
            if (model == null)
            {
                model = new PerfilViewModel();
                model.Filtro.PaginationParameters.PageSize = this.DefaultPageSize;
                model.Filtro.PaginationParameters.UniqueIdentifier = GenerateUniqueIdentifier();
            }

            CarregarCombos(model);

            return View(model);
        }

        [Authorize(Roles = Funcionalidade.PerfilAcessar)]
        public ActionResult Cadastro(int? Id)
        {
            PerfilViewModel model = new PerfilViewModel();
            PerfilDTO perfil = new PerfilDTO();

            int IdAux = 0;
            if (Id.HasValue == true) { IdAux = Id.Value; }
            if (IdAux != 0)
            {
                perfil = perfilAppService.ObterPeloId(Id) ?? new PerfilDTO();
                model.Perfil = perfil;
                var listaFuncionalidadesModulo = funcionalidadeAppService.ListarPeloModulo(perfil.ModuloId);
                model.JsonFuncionalidadesModulo = JsonConvert.SerializeObject(listaFuncionalidadesModulo);
                model.JsonFuncionalidadesPerfil = JsonConvert.SerializeObject(perfil.ListaFuncionalidade);
            }

            model.PodeSalvar = perfilAppService.EhPermitidoSalvar();
            model.PodeDeletar = perfilAppService.EhPermitidoDeletar();
            
            CarregarCombos(model);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastro(PerfilViewModel model)
        {
            if (ModelState.IsValid)
            {
                PerfilFuncionalidadeDTO PerfilFuncionalidade;

                if (model.FuncionalidadeMarcada != null)
                {
                    foreach (var item in model.FuncionalidadeMarcada)
                    {
                        PerfilFuncionalidade = new PerfilFuncionalidadeDTO();
                        PerfilFuncionalidade.Funcionalidade = item;
                        model.Perfil.ListaFuncionalidade.Add(PerfilFuncionalidade);
                    }
                }
                perfilAppService.Salvar(model.Perfil);
            }
            return PartialView("_NotificationMessagesPartial");
        }


        //public ActionResult CarregarItem(int? Id)
        //{
        //    int IdAux = 0;
        //    if (Id.HasValue == true) { IdAux = Id.Value; }
        //    var perfil = perfilAppService.ObterPeloId(Id);
        //    //lista = LimpaClasseListaFilhos(lista);
        //    return Json(perfil.ListaFuncionalidade);
        //}

        public ActionResult CarregarFuncionalidades(int moduloId)
        {
          var lista = funcionalidadeAppService.ListarPeloModulo(moduloId);
          return Json(lista);
        }


        [HttpPost]
        public ActionResult Deletar(int? Id)
        {
            int IdAux = 0;
            if (Id.HasValue == true) { IdAux = Id.Value; }

            if (IdAux == 0)
            {
                messageQueue.Add(Application.Resource.Sigim.ErrorMessages.RegistroNaoRecuperado, TypeMessage.Error);
            }
            else 
            {
                perfilAppService.Deletar(Id);
            }
            
            return PartialView("_NotificationMessagesPartial");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Lista(PerfilViewModel model)
        {
            if (ModelState.IsValid)
            {
                Session["Filtro"] = model;
                int totalRegistros;
                totalRegistros = 0;
                if (string.IsNullOrEmpty(model.Filtro.PaginationParameters.OrderBy))
                {
                    model.Filtro.PaginationParameters.OrderBy = "descricao";
                }

                var result = perfilAppService.ListarPeloFiltro(model.Filtro, out totalRegistros); ;
                if (result.Any())
                {
                    var listaViewModel = CreateListaViewModel(model.Filtro.PaginationParameters, totalRegistros, result);
                    return PartialView("ListaPartial", listaViewModel);
                }
                return PartialView("_EmptyListPartial");
            }
            return PartialView("_NotificationMessagesPartial");
        }

        private void CarregarCombos(PerfilViewModel model)
        {
            model.ListaModulo = new SelectList(moduloAppService.ListarTodosWEB(), "Id", "NomeCompleto", model.Perfil.ModuloId);
        }

        //private List<PerfilDTO> LimpaClasseListaFilhos(List<PerfilDTO> lista)
        //{
        //    foreach (var item in lista)
        //    {
        //        item.Classe.ListaFilhos = null;
        //    }
        //    return lista;
        //}


    }
}
