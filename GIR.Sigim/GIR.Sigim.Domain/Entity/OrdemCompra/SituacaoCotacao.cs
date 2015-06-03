using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Domain.Entity.OrdemCompra
{
    public enum SituacaoCotacao : short
    {
        [Description("Em cotação")]
        EmCotacao = 0,

        [Description("Aguardando fechamento")]
        AguardandoFechamento = 1,

        [Description("Fechada")]
        Fechada = 2,

        [Description("Cancelada")]
        Cancelada = 3
    }
}