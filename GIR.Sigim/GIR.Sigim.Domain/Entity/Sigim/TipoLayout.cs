using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Domain.Entity.Sigim
{
    public enum TipoLayout
    {
        [Description("Cobrança")]
        Cobranca = 1,

        [Description("Pagamento")]
        Pagamento = 2,

        [Description("Interface cont. Financeiro")]
        InterfaceContabilFinanceiro = 3,

        [Description("Interface cont. Ref")]
        InterfaceContabilRef = 4,

        [Description("Interface cotação")]
        InterfaceCotacao = 5,

        [Description("Sped Fiscal")]
        InterfaceSpedFiscal = 6
    }
}