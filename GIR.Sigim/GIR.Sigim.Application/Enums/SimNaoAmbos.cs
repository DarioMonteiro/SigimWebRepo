using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace GIR.Sigim.Application.Enums
{
    public enum SimNaoAmbos
    {
        [Description("Sim")]
        Sim = 0,

        [Description("Não")]
        Não = 1,

        [Description("Todos")]
        Todos = 2
    }
}
