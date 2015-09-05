using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Domain.Entity.Financeiro
{
    public class ImpostoPagar : BaseEntity
    {
        public int? TituloPagarId { get; set; }
        public TituloPagar TituloPagar { get; set; }
        public int? ImpostoFinanceiroId { get; set; }
        public ImpostoFinanceiro ImpostoFinanceiro { get; set; }
        public decimal BaseCalculo { get; set; }
        public decimal ValorImposto { get; set; }
        public int? TituloPagarImpostoId { get; set; }
        public TituloPagar TituloPagarImposto { get; set; }
        public Nullable<DateTime> DataEmissaoPagamentoTituloOrigem { get; set; }
    }
}