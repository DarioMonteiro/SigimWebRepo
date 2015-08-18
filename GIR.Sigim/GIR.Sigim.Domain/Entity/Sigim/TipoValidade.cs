using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Domain.Entity.Sigim
{
    public enum TipoValidade
    {
        [Description("Dias")]
        Dias = 1,

        [Description("Meses")]
        Meses = 2,

        [Description("Anos")]
        Anos = 3
    }
}