using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GIR.Sigim.Application.Filtros.OrdemCompras;

namespace GIR.Sigim.Presentation.WebUI.Areas.OrdemCompra.ViewModel
{
    public class RelOcItensOrdemCompraListaViewModel
    {
        public RelOcItensOrdemCompraFiltro Filtro { get; set; }
        public SelectList ListaFornecedorCliente { get; set; }

        public bool PodeImprimir { get; set; }

        public RelOcItensOrdemCompraListaViewModel()
        {
            Filtro = new RelOcItensOrdemCompraFiltro();
            Filtro.EhFechada = true;
            Filtro.EhLiberada = true;
            Filtro.EhPendente = true;
        }

    }
}