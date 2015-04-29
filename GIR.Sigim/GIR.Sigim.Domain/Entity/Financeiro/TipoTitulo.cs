using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Domain.Entity.Financeiro
{
    public enum TipoTitulo : short
    {
        [Description("Vinculado")]
        Vinculado = 0,

        [Description("Não Vinculado")]
        NaoVinculado = 1,

        [Description("Todos")]
        Todos = 2

    }
}
