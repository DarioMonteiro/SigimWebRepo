using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Domain.Entity.Financeiro
{
    public enum SituacaoTituloPagar : short
    {
        [Description("Provisionado")]
        Provisionado = 0,

        [Description("Aguardando liberação")]
        AguardandoLiberacao = 1,

        [Description("Liberado")]
        Liberado = 2,

        [Description("Emitido")]
        Emitido = 3,

        [Description("Pago")]
        Pago = 4,

        [Description("Baixado")]
        Baixado = 5,

        [Description("Cancelado")]
        Cancelado = 6


    }
}
