using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace GIR.Sigim.Application.Enums
{
    public enum OperacaoLiberarMedicao
    {
        [Description("AprovarLiberar")]
        AprovarLiberar = 0,

        [Description("Liberar")]
        Liberar = 1,

        [Description("Cancelar")]
        Cancelar = 2
    }
}
