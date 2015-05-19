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
        [Description("Genérico por preço unitário")]
        PrecoUnitario = 0,

        [Description("Genérico por preço global")]
        PrecoGlobal = 1

    }
}
