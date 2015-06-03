using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using GIR.Sigim.Application.Filtros.Sigim;
using GIR.Sigim.Presentation.WebUI.ViewModel;

namespace GIR.Sigim.Presentation.WebUI.ViewModel
{
    public class MaterialListaViewModel
    {
        public MaterialFiltro Filtro { get; set; }
        public SelectList ListaUnidadeMedida { get; set; }

        public MaterialListaViewModel()
        {
            Filtro = new MaterialFiltro();
        }
    }
}