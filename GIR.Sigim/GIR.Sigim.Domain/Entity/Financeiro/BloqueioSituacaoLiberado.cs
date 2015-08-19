using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Domain.Entity.Financeiro
{
    public enum BloqueioSituacaoLiberado
    {
        [Description("Correntista")]
        Correntista = 0,

        [Description("Identificação")]
        Identificacao = 1,

        [Description("Valor do título")]
        ValorTitulo = 2,

        [Description("Data emissão")]
        DataEmissao = 3,

        [Description("Data vencimento")]
        DataVencimento = 4,

        [Description("Imposto")]
        Imposto = 5,

        [Description("Apropriação")]
        Apropriacao = 6
    }
}
