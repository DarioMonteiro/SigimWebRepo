using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Domain.Entity.Financeiro
{
    public enum FatoGeradorImpostoFinanceiro
    {
        [Description("Emissão de documento")]
        EmissaoDocumento = 1,
        
        [Description("Pagamento")]
        Pagamento = 2
    }
}