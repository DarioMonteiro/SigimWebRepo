using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GIR.Sigim.Application.DTO.Financeiro;
using GIR.Sigim.Application.Filtros.Financeiro;

namespace GIR.Sigim.Presentation.WebUI.Areas.Financeiro.ViewModel
{
    public class RelAcompanhamentoFinanceiroListaViewModel
    {
        public ClasseDTO Classe { get; set; }
        public string JsonItensClasse { get; set; }

        public RelAcompanhamentoFinanceiroFiltro Filtro { get; set; }
        public bool PodeImprimir { get; set; }

        public SelectList ListaIndice { get; set; }

        public RelAcompanhamentoFinanceiroListaViewModel()
        {
            Filtro = new RelAcompanhamentoFinanceiroFiltro();
        }

    }
}