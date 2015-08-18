using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Application.Filtros.Sigim;

namespace GIR.Sigim.Presentation.WebUI.ViewModel
{
    public class UnidadeMedidaViewModel
    {
        public UnidadeMedidaDTO UnidadeMedida { get; set; }
        public UnidadeMedidaFiltro Filtro { get; set; }
        public bool PodeSalvar { get; set; }
        public bool PodeDeletar { get; set; }
        public bool PodeImprimir { get; set; }

        public UnidadeMedidaViewModel()
        {
            Filtro = new UnidadeMedidaFiltro();
        }

    }
}