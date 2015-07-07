using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using GIR.Sigim.Application.DTO.OrdemCompra;
using GIR.Sigim.Application.Filtros.OrdemCompras;
using GIR.Sigim.Presentation.WebUI.ViewModel;

namespace GIR.Sigim.Presentation.WebUI.Areas.OrdemCompra.ViewModel
{
    public class EntradaMaterialListaViewModel
    {
        public EntradaMaterialFiltro Filtro { get; set; }

        public EntradaMaterialListaViewModel()
        {
            Filtro = new EntradaMaterialFiltro();
        }
    }
}