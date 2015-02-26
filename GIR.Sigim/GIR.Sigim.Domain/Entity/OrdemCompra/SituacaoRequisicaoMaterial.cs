using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Domain.Entity.OrdemCompra
{
    public enum SituacaoRequisicaoMaterial : short
    {
        [Description("Requisitada")]
        Requisitada = 0,

        [Description("Aprovada")]
        Aprovada = 1,

        [Description("Fechada")]
        Fechada = 2,

        [Description("Cancelada")]
        Cancelada = 3
    }
}