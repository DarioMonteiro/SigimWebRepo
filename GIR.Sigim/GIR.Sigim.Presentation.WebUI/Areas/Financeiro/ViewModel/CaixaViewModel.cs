using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using GIR.Sigim.Application.DTO.Financeiro;
using GIR.Sigim.Application.Filtros.Financeiro;

namespace GIR.Sigim.Presentation.WebUI.Areas.Financeiro.ViewModel
{
    public class CaixaViewModel
    {
        public CaixaDTO Caixa { get; set; }
        public CaixaFiltro Filtro { get; set; }
        public bool PodeSalvar { get; set; }
        public bool PodeDeletar { get; set; }
        public bool PodeImprimir { get; set; }

        public CaixaViewModel()
        {
            Filtro = new CaixaFiltro();
        }

    }
}