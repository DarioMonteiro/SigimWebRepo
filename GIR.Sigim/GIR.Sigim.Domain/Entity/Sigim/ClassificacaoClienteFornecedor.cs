using System;
using System.Collections.Generic;
using System.ComponentModel; 
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Domain.Entity.Sigim
{
    public enum ClassificacaoClienteFornecedor : short
    {

        [Description("APagar")]
        APagar = 0,

        [Description("AReceber")]
        aReceber = 1,

        [Description("OrdemCompra")]
        OrdemCompra = 2,

        [Description("Contrato")]
        Contrato = 3,

        [Description("Todos")]
        Todos = 4,

        [Description("Aluguel")]
        Aluguel = 5,

        [Description("Empreitada")]
        Empreitada = 6

    }
}
