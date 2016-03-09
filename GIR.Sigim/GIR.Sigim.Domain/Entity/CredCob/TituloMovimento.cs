using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Financeiro;

namespace GIR.Sigim.Domain.Entity.CredCob
{
    public class TituloMovimento : BaseEntity
    {
        public int? TituloCredCobId { get; set; }
        public TituloCredCob TituloCredCob { get; set; }

        public int? MovimentoFinanceiroId { get; set; }
        public MovimentoFinanceiro MovimentoFinanceiro { get; set; }
    }
}
