using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using GIR.Sigim.Application.DTO.Comercial;
using GIR.Sigim.Application.Filtros.Comercial;

namespace GIR.Sigim.Presentation.WebUI.Areas.Comercial.ViewModel
{
    public class RelStatusVendaViewModel
    {
        public RelStatusVendaDTO RelStatusVenda { get; set; }
        public RelStatusVendaFiltro Filtro { get; set; }

        public SelectList ListaIncorporador { get; set; }
        public SelectList ListaEmpreendimento { get; set; }
        public SelectList ListaBloco { get; set; }
        public SelectList ListaSimNao { get; set; }

        public bool PodeImprimir { get; set; }
        
        public RelStatusVendaViewModel()
        {
            Filtro = new RelStatusVendaFiltro();
        }

    }
}