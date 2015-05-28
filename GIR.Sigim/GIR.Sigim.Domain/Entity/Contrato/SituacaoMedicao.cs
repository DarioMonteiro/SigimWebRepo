using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Domain.Entity.Contrato
{
    public enum SituacaoMedicao
    {

        [Description("Aguardando Aprovação")]
        AguardandoAprovacao = 0,

        [Description("Aguardando Liberação")]
        AguardandoLiberacao = 1,

        [Description("Liberado")]
        Liberado = 2,

        [Description("Retenção")]
        Retencao = 3,

        [Description("Provisionado")]
        Provisionado = 9

    }
}
