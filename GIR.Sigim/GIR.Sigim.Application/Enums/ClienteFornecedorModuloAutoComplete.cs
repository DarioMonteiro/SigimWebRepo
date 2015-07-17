using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace GIR.Sigim.Application.Enums
{
    public enum ClienteFornecedorModuloAutoComplete
    {
        
        [Description("a Pagar")]
        APagar = 0,

        [Description("a Receber")]
        AReceber = 1,

        [Description("Ordem Compra")]
        OrdemCompra = 2,

        [Description("Contrato")]
        Contrato = 3,

        [Description("Todos")]
        Todos = 4,

        [Description("Aluguel")]
        Aluguel = 5,

        [Description("Empreitada")]
        Empreitada = 6,

        [Description("Gestão Serviço")]
        gestaoServico = 7

    }
}
