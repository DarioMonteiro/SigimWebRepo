using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Domain.Entity.Contrato
{
    public enum NaturezaItem
    {
        [Description("Preço Unitário")]
        PrecoUnitario = 0,

        [Description("Preço Global")]
        PrecoGlobal = 1

    }
}
