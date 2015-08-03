using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Domain.Entity.Financeiro
{
    public enum TipoTitulo : short
    {
        [Description("Normal")]
        Normal = 0,

        [Description("Pai")]
        Pai = 1,

        [Description("Parcela")]
        Parcela = 2

    }
}
