using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel; 

namespace GIR.Sigim.Domain.Entity.Sigim
{
    public enum TipoClienteProspect : short
    {
        [Description("Cliente")]
        Cliente = 0,

        [Description("Prospect")]
        Prospect = 1
    }
}
