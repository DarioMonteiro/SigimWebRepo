using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Application.Filtros.Sigim;

namespace GIR.Sigim.Presentation.WebUI.ViewModel
{
    public class FormaRecebimentoViewModel
    {
        public FormaRecebimentoDTO FormaRecebimento { get; set; }

        public FormaRecebimentoFiltro Filtro { get; set; }

        public FormaRecebimentoViewModel()
        {
            Filtro = new FormaRecebimentoFiltro();
        }

    }
}