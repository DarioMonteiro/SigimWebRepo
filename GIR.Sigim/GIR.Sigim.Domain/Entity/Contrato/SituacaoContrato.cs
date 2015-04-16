using System;
using System.Collections.Generic;
using System.ComponentModel; 
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Domain.Entity.Contrato
{
    public enum SituacaoContrato : short
    {

        [Description("Minuta")]
            Minuta = 0,

        [Description("Aguardando Assinatura")]
            AguardandoAssinatura = 1,

        [Description("Assinado")]
            Assinado = 2,

        [Description("Retificacao")]
            Retificacao = 3,

        [Description("Suspenso")]
            Suspenso = 4,

        [Description("Concluido")]
            Concluido = 5,

        [Description("Cancelado")]
            Cancelado = 6

    }
}
