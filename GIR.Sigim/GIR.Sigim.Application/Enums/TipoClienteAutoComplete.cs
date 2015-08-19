using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace GIR.Sigim.Application.Enums
{
    public enum TipoClienteAutoComplete
    {
        [Description("Prospect")]
        Prospect = 0,

        [Description("Cliente")]
        Cliente = 1,

        [Description("Colaborador")]
        Colaborador = 2,

        [Description("Todos")]
        Todos = 3,

    }
}
