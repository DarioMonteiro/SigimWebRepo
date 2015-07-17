using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace GIR.Sigim.Application.Enums
{
    public enum TipoPessoaAutoComplete
    {
        [Description("Física")]
        Fisica = 0,

        [Description("Jurídica")]
        Juridica = 1,

        [Description("Todos")]
        Todos = 2

    }
}
