using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Domain.Entity.OrdemCompra
{
    public enum SituacaoPreRequisicaoMaterial : short
    {
        [Description("Requisitada")]
        Requisitada = 0,

        [Description("Parcialmente aprovada")]
        ParcialmenteAprovada = 1,

        [Description("Fechada")]
        Fechada = 2,

        [Description("Cancelada")]
        Cancelada = 3,

        [Description("Aprovada")]
        Aprovada = 4
    }
}