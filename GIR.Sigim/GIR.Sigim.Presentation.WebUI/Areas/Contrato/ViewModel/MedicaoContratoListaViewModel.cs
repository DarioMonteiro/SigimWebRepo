using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GIR.Sigim.Application.Filtros.Contrato; 

namespace GIR.Sigim.Presentation.WebUI.Areas.Contrato.ViewModel
{
    public class MedicaoContratoListaViewModel 
    {
        public MedicaoContratoFiltro Filtro { get; set; }
        public SelectList ListaContratante { get; set; }
        public SelectList ListaContratado { get; set; }

        public MedicaoContratoListaViewModel()
        {
            Filtro = new MedicaoContratoFiltro();
        }
    }
}
