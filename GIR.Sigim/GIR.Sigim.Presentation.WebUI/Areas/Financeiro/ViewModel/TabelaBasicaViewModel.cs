using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Application.Filtros;

namespace GIR.Sigim.Presentation.WebUI.Areas.Financeiro.ViewModel
{
    public class TabelaBasicaViewModel
    {
        public TabelaBasicaDTO TabelaBasica { get; set; }
        public SelectList ListaTipoTabela { get; set; }
        public int? TipoTabelaId { get; set; }

        public BaseFiltro Filtro { get; set; }

        public TabelaBasicaViewModel()
        {
            Filtro = new BaseFiltro();
        }

    }
}