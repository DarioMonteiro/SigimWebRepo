using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Domain.Entity.Financeiro
{
    public class ImpostoReceber : BaseEntity
    {
        public int TituloReceberId { get; set; }
        public TituloReceber TituloReceber { get; set; }
        public int ImpostoFinanceiroId { get; set; }
        public ImpostoFinanceiro ImpostoFinanceiro { get; set; }
        public decimal BaseCalculo { get; set; }
        public decimal ValorImposto { get; set; }
        public Nullable<DateTime> DataEmissaoDocumento { get; set; }
        public Nullable<DateTime> DataRecebimento { get; set; }
    }
}
