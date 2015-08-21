using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Domain.Entity.Sac
{
    public enum SituacaoSolicitacaoSac
    {
        [Description("Em andamento")]
         EmAndamento = 1,

        [Description("Em avaliação")]
        EmAvaliacao= 2,

        [Description("Em execução")]
        EmExecucao = 3,

        [Description("Concluída")]
        Concluida = 4,

        [Description("Cancelada")]
        Cancelada = 5,

        [Description("Solicitação web")]
        SolicitacaoWeb = 6,
    }
}
