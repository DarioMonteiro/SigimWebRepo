using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Domain.Entity.OrdemCompra
{
    public enum SituacaoRequisicaoMaterialItem : short
    {
        [Description("Requisitado")]
        Requisitado = 0,

        [Description("Em Cotação")]
        EmCotacao = 1,

        [Description("Aguardando Fechamento")]
        AguardandoFechamento = 2,

        [Description("Fechado")]
        Fechado = 3,

        [Description("Cancelado")]
        Cancelado = 4
    }
}