using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GIR.Sigim.Application.Filtros.Financeiro;
using GIR.Sigim.Application.DTO.Sigim;

namespace GIR.Sigim.Presentation.WebUI.Areas.Financeiro.ViewModel
{
    public class RelContasAPagarTitulosListaViewModel
    {
        public RelContasAPagarTitulosFiltro Filtro { get; set; }

        public bool PodeImprimir { get; set; }

        public SelectList ListaTipoCompromisso { get; set; }
        public SelectList ListaFormaPagamento { get; set; }
        public SelectList ListaBanco { get; set; }
        public SelectList ListaAgenciaConta { get; set; }
        public SelectList ListaCaixa { get; set; }

        public RelContasAPagarTitulosListaViewModel()
        {
            Filtro = new RelContasAPagarTitulosFiltro();
            Filtro.ClienteFornecedor = new ClienteFornecedorDTO();
            Filtro.VisualizarClientePor = 0;
            Filtro.EhTotalizadoPor = 0;
        }
    }
}