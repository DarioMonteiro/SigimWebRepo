using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Application.Filtros.Sigim
{
    public class MaterialFiltro : BaseFiltro
    {
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        [Display(Name = "UN")]
        public string Sigla { get; set; }
    }
}