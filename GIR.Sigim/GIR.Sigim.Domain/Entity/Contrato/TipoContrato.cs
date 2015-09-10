using System;
using System.Collections.Generic;
using System.ComponentModel; 

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Domain.Entity.Contrato
{
    public enum TipoContrato 
    {
        [Description("Contrato a pagar")]
        ContratoAPagar = 0,

        [Description("Contrato a receber")]
        contratoAReceber = 1
    }
}
