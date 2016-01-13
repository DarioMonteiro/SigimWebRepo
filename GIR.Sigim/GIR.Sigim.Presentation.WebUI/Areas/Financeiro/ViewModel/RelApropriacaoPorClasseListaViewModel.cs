using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GIR.Sigim.Application.Filtros.Financeiro;

namespace GIR.Sigim.Presentation.WebUI.Areas.Financeiro.ViewModel
{
    public class RelApropriacaoPorClasseListaViewModel
    {
        public RelApropriacaoPorClasseFiltro Filtro { get; set; }
        public bool PodeImprimir { get; set; }

        public RelApropriacaoPorClasseListaViewModel()
        {
            Filtro = new RelApropriacaoPorClasseFiltro();
        }

    }
}