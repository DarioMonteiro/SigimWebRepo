using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Domain.Entity.OrdemCompra
{
    public enum SituacaoPreRequisicaoMaterialItem : short
    {
        [Description("Requisitado")]
        Requisitado = 0,

        [Description("Aprovado")]
        Aprovado = 1,

        [Description("Cancelado")]
        Cancelado = 2,

        [Description("Atendido pelo estoque")]
        AtendidoPeloEstoque = 3,

        [Description("Entregue pelo estoque")]
        EntreguePeloEstoque = 4,

        [Description("Cancelada pelo estoque")]
        CanceladaPeloEstoque = 5

    }
}