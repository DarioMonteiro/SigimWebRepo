using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Application.Filtros.Sigim;
using GIR.Sigim.Presentation.WebUI.ViewModel;

namespace GIR.Sigim.Presentation.WebUI.ViewModel
{
    public class AgenciaCadastroViewModel
    {
        public AgenciaDTO Agencia { get; set; }
        
        public SelectList ListaBanco { get; set; }

        public AgenciaCadastroViewModel()
        {
               
        }
        
        
    }
}