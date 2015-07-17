using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Admin;

namespace GIR.Sigim.Application.Filtros.Admin
{
    public class UsuarioFuncionalidadeFiltro : BaseFiltro
    {
        [Display(Name = "Usuário")]
        public int? UsuarioId { get; set; }
        public UsuarioDTO Usuario { get; set; }

        [Display(Name = "Módulo")]
        public int? ModuloId { get; set; }
        public ModuloDTO Modulo { get; set; }
    }
}