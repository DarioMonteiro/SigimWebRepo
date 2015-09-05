using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Financeiro;

namespace GIR.Sigim.Domain.Entity.OrdemCompra
{
    public class EntradaMaterialImposto : BaseEntity
    {
        public int? EntradaMaterialId { get; set; }
        public EntradaMaterial EntradaMaterial { get; set; }
        public int? ImpostoFinanceiroId { get; set; }
        public ImpostoFinanceiro ImpostoFinanceiro { get; set; }
        public Nullable<DateTime> DataVencimento { get; set; }
        public decimal BaseCalculo { get; set; }
        public decimal Valor { get; set; }
        public int? TituloPagarImpostoId { get; set; }
        public TituloPagar TituloPagarImposto { get; set; }
    }
}