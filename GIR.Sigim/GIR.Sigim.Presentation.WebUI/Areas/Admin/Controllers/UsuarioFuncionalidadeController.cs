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

namespace GIR.Sigim.Presentation.WebUI.Areas.Admin.Controllers
{
    public class UsuarioFuncionalidadeController : BaseController
    {
        private IUsuarioAppService usuarioAppService;
        private IModuloAppService moduloAppService;
        private IPerfilAppService perfilAppService;
        
        private IFuncionalidadeAppService funcionalidadeAppService;

        public UsuarioFuncionalidadeController(
            IUsuarioAppService usuarioAppService,
            IModuloAppService moduloAppService,
            IPerfilAppService perfilAppService,
            IFuncionalidadeAppService funcionalidadeAppService,
            MessageQueue messageQueue)
            : base(messageQueue)
        {
            this.usuarioAppService = usuarioAppService;
            this.moduloAppService = moduloAppService;
            this.perfilAppService = perfilAppService;
            this.funcionalidadeAppService = funcionalidadeAppService;
        }

      
        public ActionResult Index()
        {
            var model = Session["Filtro"] as UsuarioFuncionalidadeViewModel;
            if (model == null)
            {
                model = new UsuarioFuncionalidadeViewModel();
                model.Filtro.PaginationParameters.PageSize = this.DefaultPageSize;
            }

            CarregarCombos(model);

            return View(model);
        }

        public ActionResult RecuperaRegistro(int UsuarioId, int ModuloId)
        {
            PerfilDTO perfil = new PerfilDTO();
            List<PerfilFuncionalidadeDTO> listaUsuarioPerfil = new List<PerfilFuncionalidadeDTO>();
            List<UsuarioFuncionalidadeDTO> listaFuncionalidadesAvulsas = new List<UsuarioFuncionalidadeDTO>();
            int PerfilId = 0;
            bool NovoItem;

            if ((UsuarioId != 0) || (ModuloId != 0))
            {
                var usuario = usuarioAppService.ObterUsuarioPorId(UsuarioId) ?? new UsuarioDTO();

                if (usuario.ListaUsuarioPerfil.Any(l => l.ModuloId == ModuloId))
                {
                    var usuarioPerfil = usuario.ListaUsuarioPerfil.Where(l => l.ModuloId == ModuloId).ToList<UsuarioPerfilDTO>()[0];
                    if (usuarioPerfil.Perfil.Id.HasValue)
                    {
                        perfil = perfilAppService.ObterPeloId(usuarioPerfil.PerfilId);
                        listaUsuarioPerfil = perfil.ListaFuncionalidade;
                        PerfilId = usuarioPerfil.PerfilId;
                    }
                }

                listaFuncionalidadesAvulsas = usuario.ListaUsuarioFuncionalidade;
                listaFuncionalidadesAvulsas = usuario.ListaUsuarioFuncionalidade.Where(l => l.ModuloId == ModuloId).ToList<UsuarioFuncionalidadeDTO>();
                listaFuncionalidadesAvulsas = LimpaFuncionalidadeUsuario(listaFuncionalidadesAvulsas);
            }

            NovoItem = true;
            if ((PerfilId != 0) || (listaFuncionalidadesAvulsas.Count > 0))
            {
                NovoItem = false;
            }

            return Json(new
                            {
                                NovoItem,
                                PerfilId,
                                listaUsuarioPerfil,
                                listaFuncionalidadesAvulsas,
                            }
                       );
        }

        public ActionResult Cadastro(int? UsuarioId, int? ModuloId)
        {
            UsuarioFuncionalidadeViewModel model = new UsuarioFuncionalidadeViewModel();
            PerfilDTO perfil = new PerfilDTO();
            int PerfilId = 0;

            if ((UsuarioId != null) && (ModuloId != null))
            {
                var usuario = usuarioAppService.ObterUsuarioPorId(UsuarioId.Value) ?? new UsuarioDTO();
                model.Usuario = usuario;
                model.UsuarioId = UsuarioId.Value;

                var modulo = moduloAppService.ObterPeloId(ModuloId.Value) ?? new ModuloDTO();
                model.Modulo = modulo;
                model.ModuloId = ModuloId.Value;

                if (usuario.ListaUsuarioPerfil.Any(l => l.ModuloId == ModuloId.Value))
                {
                    var usuarioPerfil = usuario.ListaUsuarioPerfil.Where(l => l.ModuloId == ModuloId.Value).ToList<UsuarioPerfilDTO>()[0];
                    //if (usuarioPerfil.PerfilId != null)
                    //{
                        perfil = perfilAppService.ObterPeloId(usuarioPerfil.PerfilId);
                        PerfilId = usuarioPerfil.PerfilId;
                        model.PerfilId = PerfilId;
                    //}
                }

                var listaFuncionalidadesAvulsas = usuario.ListaUsuarioFuncionalidade.Where(l => l.ModuloId == ModuloId).ToList<UsuarioFuncionalidadeDTO>();
                listaFuncionalidadesAvulsas = LimpaFuncionalidadeUsuario(listaFuncionalidadesAvulsas);

                model.NovoItem = true;
                if ((PerfilId !=0) || (listaFuncionalidadesAvulsas.Count > 0))
                {
                    model.NovoItem = false;
                }

                var listaFuncionalidadesModulo = funcionalidadeAppService.ListarPeloModulo(ModuloId.Value);
                model.JsonFuncionalidadesModulo = JsonConvert.SerializeObject(listaFuncionalidadesModulo);
                model.JsonFuncionalidadesPerfil = JsonConvert.SerializeObject(perfil.ListaFuncionalidade);
                model.JsonFuncionalidadesAvulsas = JsonConvert.SerializeObject(listaFuncionalidadesAvulsas);
            }
            
            CarregarCombos(model);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastro(UsuarioFuncionalidadeViewModel model)
        {
            if (ModelState.IsValid)
            {
                UsuarioFuncionalidadeDTO usuarioFuncionalidade;
                List<UsuarioFuncionalidadeDTO> listaUsuarioFuncionalidade = new List<UsuarioFuncionalidadeDTO>();
                List<PerfilFuncionalidadeDTO> listaPerfilFuncionalidade = new List<PerfilFuncionalidadeDTO>();

                if (model.FuncionalidadeMarcada != null)
                {
                   
                   if (model.PerfilId.HasValue)
                   {
                       var perfil = perfilAppService.ObterPeloId(model.PerfilId.Value);
                       listaPerfilFuncionalidade = perfil.ListaFuncionalidade;
                   }

                    foreach (var item in model.FuncionalidadeMarcada)
                    {
                        if (!listaPerfilFuncionalidade.Any(l => l.Funcionalidade == item.ToString()))
                        {
                            usuarioFuncionalidade = new UsuarioFuncionalidadeDTO();
                            usuarioFuncionalidade.Funcionalidade = item;
                            listaUsuarioFuncionalidade.Add(usuarioFuncionalidade);
                        }
                    }
                }
                usuarioAppService.SalvarPermissoes(model.Usuario.Id.Value, model.Modulo.Id.Value, model.PerfilId, listaUsuarioFuncionalidade);
            }
            return PartialView("_NotificationMessagesPartial");
        }


        public ActionResult CarregarFuncionalidades(int ModuloId)
        {
          var lista = funcionalidadeAppService.ListarPeloModulo(ModuloId);
          return Json(lista);
        }

        public ActionResult CarregarPerfil(int PerfilId)
        {
            var objPerfil = perfilAppService.ObterPeloId(PerfilId);
            return Json(objPerfil.ListaFuncionalidade);
        }

        public ActionResult CarregaPerfilPorModulo(int ModuloId)
        {
            var listaPerfil = perfilAppService.ListarPeloModulo(ModuloId);
            return Json(listaPerfil);
        }


        [HttpPost]
        public ActionResult Deletar(int UsuarioId, int ModuloId)
        {
            if ((UsuarioId == 0) || (ModuloId == 0))
            {
                messageQueue.Add(Application.Resource.Sigim.ErrorMessages.RegistroNaoRecuperado, TypeMessage.Error);
            }
            else 
            {
                usuarioAppService.DeletarPermissoes(UsuarioId, ModuloId);
            }
            
            return PartialView("_NotificationMessagesPartial");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Lista(UsuarioFuncionalidadeViewModel model)
        {
            if (ModelState.IsValid)
            {
                Session["Filtro"] = model;
                int totalRegistros;
                totalRegistros = 0;
                var result = usuarioAppService.ListarPeloUsuarioModulo(model.Filtro, out totalRegistros);
                if (result.Any())
                {
                    var listaViewModel = CreateListaViewModel(model.Filtro.PaginationParameters, totalRegistros, result);
                    return PartialView("ListaPartial", listaViewModel);
                }
                return PartialView("_EmptyListPartial");
            }
            return PartialView("_NotificationMessagesPartial");
        }

        private void CarregarCombos(UsuarioFuncionalidadeViewModel model)
        {
            model.ListaUsuario = new SelectList(usuarioAppService.ListarTodos(), "Id", "Login", model.UsuarioId);
            model.ListaModulo = new SelectList(moduloAppService.ListarTodos(), "Id", "NomeCompleto", model.ModuloId);
            model.ListaPerfil = new SelectList(perfilAppService.ListarPeloModulo(model.ModuloId), "Id", "Descricao", model.PerfilId);
        }

        private List<UsuarioFuncionalidadeDTO> LimpaFuncionalidadeUsuario(List<UsuarioFuncionalidadeDTO> lista)
        {
            foreach (UsuarioFuncionalidadeDTO item in lista)
            {
                item.Usuario = null;
            }
            return lista;
        }


    }
}
