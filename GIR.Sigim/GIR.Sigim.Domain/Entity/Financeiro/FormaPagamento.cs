using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Domain.Entity.Financeiro
{
    public enum FormaPagamento : short
    {
        [Description("Automático")]
        Automatico = 1,

        [Description("Borderô")]
        Bordero = 2,

        [Description("Borderô eletrônico")]
        BorderoEletrônico = 3,

        [Description("Cheque")]
        Cheque = 4,

        [Description("Dinheiro")]
        Dinheiro = 5,

        [Description("Operação bancária")]
        OperacaoBancaria = 6
    }
}
