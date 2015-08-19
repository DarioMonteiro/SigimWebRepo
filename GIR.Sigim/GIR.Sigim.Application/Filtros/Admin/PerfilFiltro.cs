using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Admin;

namespace GIR.Sigim.Application.Filtros.Admin
{
    public class PerfilFiltro : BaseFiltro
    {
        [Display(Name = "Módulo")]
        public int? ModuloId { get; set; }
        public ModuloDTO Modulo { get; set; }
    }
}