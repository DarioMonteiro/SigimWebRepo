using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GIR.Sigim.Application.Filtros.Financeiro;
using GIR.Sigim.Application.DTO.Financeiro;

namespace GIR.Sigim.Presentation.WebUI.Areas.Financeiro.ViewModel
{
    public class RelApropriacaoPorClasseListaViewModel
    {
        public ClasseDTO ClasseDespesa { get; set; }
        public string JsonItensClasseDespesa { get; set; }

        public ClasseDTO ClasseReceita { get; set; }
        public string JsonItensClasseReceita { get; set; }

        public RelApropriacaoPorClasseFiltro Filtro { get; set; }
        public bool PodeImprimir { get; set; }
        //public SelectList ListaTipoPesquisa { get; set; }
        public SelectList ListaOpcoesRelatorio { get; set; }

        public RelApropriacaoPorClasseListaViewModel()
        {
            Filtro = new RelApropriacaoPorClasseFiltro();
        }

    }
}