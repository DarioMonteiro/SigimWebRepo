using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GIR.Sigim.Application.Filtros.Contrato;

namespace GIR.Sigim.Presentation.WebUI.Areas.Contrato.ViewModel
{
    public class RelNotaFiscalLiberadaListaViewModel
    {
        public RelNotaFiscalLiberadaFiltro Filtro { get; set; }
        public SelectList ListaFornecedorCliente { get; set; }

        public bool PodeImprimir { get; set; }

        public RelNotaFiscalLiberadaListaViewModel()
        {
            Filtro = new RelNotaFiscalLiberadaFiltro();
        }
    }
}