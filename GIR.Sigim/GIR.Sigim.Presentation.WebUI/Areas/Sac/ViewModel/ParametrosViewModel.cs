using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Sac;

namespace GIR.Sigim.Presentation.WebUI.Areas.Sac.ViewModel
{
    public class ParametrosViewModel
    {
    public ParametrosSacDTO Parametros { get; set; }

    public IEnumerable<System.Web.Mvc.SelectListItem> ListaEmpresa { get; set; }
    public HttpPostedFileBase IconeRelatorio { get; set; }
    }
}