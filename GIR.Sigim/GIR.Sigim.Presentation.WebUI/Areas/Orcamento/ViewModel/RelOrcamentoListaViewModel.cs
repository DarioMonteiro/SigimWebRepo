using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GIR.Sigim.Application.DTO.Financeiro;
using GIR.Sigim.Application.Filtros.Orcamento;
using GIR.Sigim.Application.DTO.Orcamento;

namespace GIR.Sigim.Presentation.WebUI.Areas.Orcamento.ViewModel
{
    public class RelOrcamentoListaViewModel
    {
        public ClasseDTO Classe { get; set; }
        public string JsonItensClasse { get; set; }

        public RelOrcamentoFiltro Filtro { get; set; }
        public bool PodeImprimir { get; set; }

        public SelectList ListaEmpresa { get; set; }
        public SelectList ListaObra { get; set; }
        public SelectList ListaOrcamento { get; set; }
        public SelectList ListaIndice { get; set; }

        public RelOrcamentoListaViewModel()
        {
            Classe = new ClasseDTO();
            Filtro = new RelOrcamentoFiltro();
            Filtro.Orcamento = new OrcamentoDTO();
        }

    }
}