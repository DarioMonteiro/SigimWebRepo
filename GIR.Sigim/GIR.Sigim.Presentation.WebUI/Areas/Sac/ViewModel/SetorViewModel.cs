using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using GIR.Sigim.Application.DTO.Sac;
using GIR.Sigim.Application.Filtros.Sac;

namespace GIR.Sigim.Presentation.WebUI.Areas.Sac.ViewModel
{
    public class SetorViewModel
    {
        public SetorDTO Setor { get; set; }

        public SetorFiltro Filtro { get; set; }

        public SetorViewModel()
        {
            Filtro = new SetorFiltro();
        }

    }
}