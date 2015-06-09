using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Domain.Entity.Financeiro
{
    public enum FimDeSemanaImpostoFinanceiro
    {
        [Description("Antecipa")]
        Antecipa = 1,
        
        [Description("Posterga")]
        Posterga = 2
    }
}