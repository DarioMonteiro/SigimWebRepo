using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Domain.Entity.OrdemCompra
{
    public enum SituacaoEntradaMaterial : short
    {
        [Description("Pendente")]
        Pendente = 0,

        [Description("Fechada")]
        Fechada = 1,

        [Description("Cancelada")]
        Cancelada = 2
    }
}