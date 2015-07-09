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
    public class PerfilViewModel
    {
        public PerfilFiltro Filtro { get; set; }

        public PerfilDTO Perfil { get; set; }

        //[Display(Name = "Módulo")]
        //public int ModuloId { get; set; }

        public string JsonFuncionalidadesModulo { get; set; }
        public string JsonFuncionalidadesPerfil { get; set; }
        public string[] FuncionalidadeMarcada { get; set; }

        public SelectList ListaModulo { get; set; }

        public PerfilViewModel()
        {
            Perfil = new PerfilDTO();
            JsonFuncionalidadesModulo = "[]";
            JsonFuncionalidadesPerfil = "[]";
            Filtro = new PerfilFiltro();
        }

    }
}