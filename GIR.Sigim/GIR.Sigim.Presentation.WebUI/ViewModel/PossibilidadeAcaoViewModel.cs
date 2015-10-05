using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GIR.Sigim.Infrastructure.Crosscutting.Notification;

namespace GIR.Sigim.Presentation.WebUI.ViewModel
{
    public class PossibilidadeAcaoViewModel
    {
        public bool HaPossibilidadeAcao { get; set; }
        public List<Message> ErrorMessages { get; set; }
    }
}