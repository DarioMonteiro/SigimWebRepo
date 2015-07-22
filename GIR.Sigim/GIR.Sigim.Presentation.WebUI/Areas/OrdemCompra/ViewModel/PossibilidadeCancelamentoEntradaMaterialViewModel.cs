using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;

namespace GIR.Sigim.Presentation.WebUI.Areas.OrdemCompra.ViewModel
{
    public class PossibilidadeCancelamentoEntradaMaterialViewModel
    {
        public bool HaPossibilidadeCancelamentoEntradaMaterial { get; set; }
        public List<Message> ErrorMessages { get; set; }
    }
}