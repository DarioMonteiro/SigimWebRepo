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
    public class TipoMovimentoViewModel
    {
        public TipoMovimentoDTO TipoMovimento { get; set; }
        public SelectList ListaHistoricoContabil { get; set; }

        public BaseFiltro Filtro { get; set; }
        public bool PodeSalvar { get; set; }
        public bool PodeDeletar { get; set; }

        public TipoMovimentoViewModel()
        {
            Filtro = new BaseFiltro();
        }

    }
}