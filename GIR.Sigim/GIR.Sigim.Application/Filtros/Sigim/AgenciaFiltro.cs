using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Sigim;

namespace GIR.Sigim.Application.Filtros.Sigim
{
    public class AgenciaFiltro : BaseFiltro
    {
        [Display(Name = "Banco")]
        public int? BancoId { get; set; }
        public BancoDTO Banco { get; set; }
    }
}