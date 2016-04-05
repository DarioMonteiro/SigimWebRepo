using System;
using System.Collections.Generic;
using System.ComponentModel; 
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Domain.Entity.GirCliente
{
    public enum AcaoWS
    {
        [Description("Não Acessou")]
        NaoAcessou = 0,

        [Description("Acessou")]
        Acessou = 1,

        [Description("AcessouComErro")]
        AcessouComErro = 2
    }
}
