using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using GIR.Sigim.Application.DTO.Admin;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Application.Filtros.Admin;
using System.ComponentModel.DataAnnotations;

namespace GIR.Sigim.Presentation.WebUI.Areas.Admin.ViewModel
{
    public class UsuarioFuncionalidadeViewModel
    {

        public bool NovoItem { get; set; }

        public UsuarioFuncionalidadeFiltro Filtro { get; set; }

        public UsuarioDTO Usuario { get; set; }
        [Display(Name = "Usuário")]
        public int UsuarioId { get; set; }

        public ModuloDTO Modulo { get; set; }
        [Display(Name = "Módulo")]
        public int ModuloId { get; set; }

        //public PerfilDTO Perfil { get; set; }
        [Display(Name = "Perfil")]
        public int? PerfilId { get; set; }

        public string JsonFuncionalidadesModulo { get; set; }
        public string JsonFuncionalidadesPerfil { get; set; }
        public string JsonFuncionalidadesAvulsas { get; set; }
        public string[] FuncionalidadeMarcada { get; set; }

        public SelectList ListaModulo { get; set; }
        public SelectList ListaUsuario { get; set; }
        public SelectList ListaPerfil { get; set; }
        public bool PodeSalvar { get; set; }
        public bool PodeDeletar { get; set; }

        public UsuarioFuncionalidadeViewModel()
        {
            Usuario = new UsuarioDTO();
            Modulo = new ModuloDTO();
            //Perfil = new PerfilDTO();
            JsonFuncionalidadesModulo = "[]";
            JsonFuncionalidadesPerfil = "[]";
            JsonFuncionalidadesAvulsas = "[]";
            Filtro = new UsuarioFuncionalidadeFiltro();
        }

    }
}