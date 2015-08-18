using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace GIR.Sigim.Application.Enums
{
    public enum TipoPesquisa
    {
        [Description("Contém")]
        Contem = 1,

        [Description("Intervalo")]
        Intervalo = 2
    }
}