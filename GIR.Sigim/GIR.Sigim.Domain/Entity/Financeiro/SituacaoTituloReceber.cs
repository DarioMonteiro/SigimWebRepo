using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Domain.Entity.Financeiro
{
    public enum SituacaoTituloReceber : short
    {
        [Description("Provisionado")]
        Provisionado = 0,

        [Description("A faturar")]
        Afaturar = 1,

        [Description("Faturado")]
        Faturado = 2,

        [Description("Pré-datado")]
        Predatado = 3,

        [Description("Recebido")]
        Recebido = 4,

        [Description("Quitado")]
        Quitado = 5,

        [Description("Cancelado")]
        Cancelado = 6
    }
}
