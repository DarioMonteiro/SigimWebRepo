using System;
using System.Collections.Generic;
using System.ComponentModel;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Domain.Entity.Financeiro
{
    public enum TipoMovimentoRelatorioApropriacaoPorClasse
    {
        [Description("Movimento débito")]
        MovimentoDebito = 0,

        [Description("Movimento débito caixa")]
        MovimentoDebitoCaixa = 1,

        [Description("Movimento crédito")]
        MovimentoCredito = 2,

        [Description("Movimento crédito caixa")]
        MovimentoCreditoCaixa = 3,

    }
}
