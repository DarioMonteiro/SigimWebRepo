using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Domain.Entity.Contrato
{
    public enum SituacaoLicitacao : short
    {

        [Description("Pendente")]
            Pendente = 0,

        [Description("EmAndamento")]
            EmAndamento = 1,

        [Description("FornecedorEleito")] 
            FornecedorEleito = 2,

        [Description("Fechada")]
            Fechada = 3,

        [Description("ContratoAssinado")] 
            ContratoAssinado = 4,

        [Description("Cancelada")] 
            Cancelada = 5

    }
}
