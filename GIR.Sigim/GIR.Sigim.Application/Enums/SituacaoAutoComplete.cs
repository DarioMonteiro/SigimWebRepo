using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace GIR.Sigim.Application.Enums
{
    public enum SituacaoAutoComplete
    {
        [Description("Ativo")]
        Ativo = 0,

        [Description("Inativo")]
        Inativo = 1,

        [Description("Todos")]
        Todos = 2
    }
}
