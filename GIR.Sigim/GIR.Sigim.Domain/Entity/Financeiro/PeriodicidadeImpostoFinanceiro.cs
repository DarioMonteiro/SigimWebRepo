using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Domain.Entity.Financeiro
{
    public enum PeriodicidadeImpostoFinanceiro
    {
        [Description("Quinzenal")]
        Quinzenal = 1,
        
        [Description("Mensal")]
        Mensal = 2
    }
}