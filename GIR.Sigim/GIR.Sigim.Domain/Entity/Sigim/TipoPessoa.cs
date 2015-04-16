using System;
using System.Collections.Generic;
using System.ComponentModel;  
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Domain.Entity.Sigim
{
    public enum TipoPessoa
    {
        [Description("Física")]
        Fisica = 0,

        [Description("Jurídica")]
        Juridica = 1,

        [Description("Todos")]
        Todos = 2
    }
}
