using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Domain.Entity.Sigim
{
    public enum TipoFormaRecebimento
   
    {
        [Description("Compensação Imediata")]
        CompensacaoImediata = 0,

        [Description("Compensação Posterior")]
        CompensacaoPosterior = 1,

        [Description("Sem Compensação")]
        SemCompensacao = 2

    }
}