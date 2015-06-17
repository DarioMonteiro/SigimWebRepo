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
        [Description("Compensação imediata")]
        CompensacaoImediata = 0,

        [Description("Compensação posterior")]
        CompensacaoPosterior = 1,

        [Description("Sem compensação")]
        SemCompensacao = 2

    }
}