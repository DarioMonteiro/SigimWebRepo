using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using GIR.Sigim.Application.DTO.Financeiro;
using GIR.Sigim.Application.Filtros;

namespace GIR.Sigim.Presentation.WebUI.Areas.Financeiro.ViewModel
{
    public class ImpostoFinanceiroViewModel
    {
        public ImpostoFinanceiroDTO ImpostoFinanceiro { get; set; }
        public BaseFiltro Filtro { get; set; }
        //public SelectList ListaCorrentista { get; set; }
        public SelectList ListaTipoCompromisso { get; set; }
        public SelectList ListaOpcoesPeriodicidade { get; set; }
        public SelectList ListarOpcoesFimDeSemana { get; set; }
        public SelectList ListarOpcoesFatoGerador { get; set; }
        public bool PodeSalvar { get; set; }
        public bool PodeDeletar { get; set; }
        public bool PodeImprimir { get; set; }

        public ImpostoFinanceiroViewModel()
        {
            Filtro = new BaseFiltro();
        }

    }
}