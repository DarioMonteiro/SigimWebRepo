using System;
using System.Collections.Generic;
using System.ComponentModel; 
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Domain.Entity.Sigim
{
    public enum SituacaoClienteFornecedor : short 
    {

        [Description("Ativo")]
        Ativo = 0,

        [Description("Inativo")]
        Inativo = 1,

        [Description("Todos")]
        Todos = 2
    }
}
