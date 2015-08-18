using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Domain.Entity.Sigim
{
    public enum TipoContaCorrente
   
    {        
        [Description("Simples")]
        CompensacaoPosterior = 1,

        [Description("Registrada")]
        SemCompensacao = 2

    }
}