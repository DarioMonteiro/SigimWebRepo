using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using GIR.Sigim.Application.DTO.Financeiro;
using GIR.Sigim.Application.DTO.Sigim;

namespace GIR.Sigim.Presentation.WebUI.Areas.Financeiro.ViewModel
{
    public class ParametrosFinanceiroViewModel
    {
        public ParametrosFinanceiroDTO Parametros { get; set; }
        //public SelectList ListaEmpresa { get; set; }
        public HttpPostedFileBase IconeRelatorio { get; set; }
        public bool PodeSalvar { get; set; }

        public ParametrosFinanceiroViewModel()
        {
            Parametros = new ParametrosFinanceiroDTO();
            Parametros.Cliente = new ClienteFornecedorDTO();
        }
    }
}