using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Domain.Entity.Estoque
{
    public enum TipoMovimentoEstoque
    {
        [Description("Entrada manual")]
        EntradaManual = 1,

        [Description("Saída manual")]
        SaidaManual = 2,

        [Description("Devolução")]
        Devolucao = 3,

        [Description("Transf. de entrada")]
        TransferenciaEntrada = 4,

        [Description("Transf. de saída")]
        TransferenciaSaida = 5,

        [Description("Entrada EM")]
        EntradaEM = 6,

        [Description("Saída EM")]
        SaidaEM = 7,

        [Description("Reposição temporária")]
        EntradaTemporaria = 8,

        [Description("Retirada temporária")]
        SaidaTemporaria = 9,

        [Description("Entrada contrato")]
        EntradaContrato = 10,

        [Description("Saída contrato")]
        SaidaContrato = 11
    }
}