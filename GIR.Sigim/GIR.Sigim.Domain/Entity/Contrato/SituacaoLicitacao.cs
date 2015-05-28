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

        [Description("Em Andamento")]
            EmAndamento = 1,

        [Description("Fornecedor Eleito")] 
            FornecedorEleito = 2,

        [Description("Fechada")]
            Fechada = 3,

        [Description("Contrato Assinado")] 
            ContratoAssinado = 4,

        [Description("Cancelada")] 
            Cancelada = 5

    }
}
