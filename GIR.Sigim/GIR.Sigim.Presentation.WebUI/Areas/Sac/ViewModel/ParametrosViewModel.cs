using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Sac;

namespace GIR.Sigim.Presentation.WebUI.Areas.Sac.ViewModel
{
    public class ParametrosViewModel
    {
    public ParametrosSacDTO ParametrosSac { get; set; }
    public IEnumerable<System.Web.Mvc.SelectListItem> ListaEmpresa { get; set; }
    public HttpPostedFileBase IconeRelatorio { get; set; }
    public SelectList ListaSituacaoSolicitacaoSac { get; set; }
    public IEnumerable<System.Web.Mvc.SelectListItem> ListaSetor { get; set; }

    public string Email { get; set; }
    public int? SetorId { get; set; }
    public int? Tipo { get; set; }
    public bool Anexo { get; set; }
    public string DescricaoAnexo
    {
        get { return Anexo == true ? "Sim" : "Não"; }
    }
    public int IndexSelecionado { get; set; }
    public string JsonListaEmail { get; set; }

    public ParametrosViewModel()
    {
        //this.Setor = new SetorDTO();
        this.ParametrosSac = new ParametrosSacDTO();
        JsonListaEmail = "[]";
    }
    }

}