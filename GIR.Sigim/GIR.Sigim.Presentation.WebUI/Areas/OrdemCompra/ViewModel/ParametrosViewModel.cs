using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using GIR.Sigim.Application.DTO.OrdemCompra;
using GIR.Sigim.Application.DTO.Sigim;

namespace GIR.Sigim.Presentation.WebUI.Areas.OrdemCompra.ViewModel
{
    public class ParametrosViewModel
    {
        public ParametrosOrdemCompraDTO Parametros { get; set; }
        public SelectList ListaAssuntoContatoEmail { get; set; }
        public SelectList ListaTipoCompromissoFrete { get; set; }
        public SelectList ListaLayoutSPED { get; set; }
        public SelectList ListaModeloInterface { get; set; }
        public HttpPostedFileBase IconeRelatorio { get; set; }
        public bool PodeSalvar { get; set; }

        public ParametrosViewModel()
        {
            Parametros = new ParametrosOrdemCompraDTO();
            Parametros.Cliente = new ClienteFornecedorDTO();
        }
    }
}