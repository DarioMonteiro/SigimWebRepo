using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Domain.Entity.Financeiro
{
    public enum TipoPesquisaRelatorioApropriacaoPorClasse
    {
        [Description("Por competência")]
        PorCompetencia = 1,

        [Description("Por emissão documento")]
        PorEmissaoDocumento = 2,

        [Description("Nenhum")]
        PorNenhum = 3

    }

}
