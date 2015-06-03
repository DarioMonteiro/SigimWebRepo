using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Application.Filtros.Sigim;

namespace GIR.Sigim.Presentation.WebUI.Areas.Sigim.ViewModel
{
    public class UnidadeMedidaViewModel
    {
        public UnidadeMedidaDTO UnidadeMedida { get; set; }

        public UnidadeMedidaFiltro Filtro { get; set; }

        public UnidadeMedidaViewModel()
        {
            Filtro = new UnidadeMedidaFiltro();
        }

    }
}