using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Financeiro;
using GIR.Sigim.Application.Filtros;

namespace GIR.Sigim.Presentation.WebUI.Areas.Financeiro.ViewModel
{
    public class TipoRateioViewModel
    {
        public TipoRateioDTO TipoRateio { get; set; }

        public BaseFiltro Filtro { get; set; }

        public TipoRateioViewModel()
        {
            Filtro = new BaseFiltro();
        }

    }
}