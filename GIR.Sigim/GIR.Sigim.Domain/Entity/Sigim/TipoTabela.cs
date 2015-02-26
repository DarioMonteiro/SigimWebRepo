using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Domain.Entity.Sigim
{
    public enum TipoTabela
    {
        [Description("Própria")]
        Propria = 1,

        [Description("TCPO")]
        TCPO = 2,

        [Description("EMOP")]
        EMOP = 3,

        [Description("SINAPI")]
        SINAPI = 4
    }
}