using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.OrdemCompra;

namespace GIR.Sigim.Presentation.WebUI.Areas.OrdemCompra.ViewModel
{
    public class ParametrosUsuarioViewModel
    {
        public ParametrosUsuarioDTO ParametrosUsuario { get; set; }
        public bool PodeSalvar { get; set; }
    }
}