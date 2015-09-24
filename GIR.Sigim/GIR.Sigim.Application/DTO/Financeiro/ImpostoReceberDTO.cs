using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Application.DTO.Financeiro
{
    public class ImpostoReceberDTO : BaseDTO
    {
        public int? TituloReceberId { get; set; }
        public TituloReceberDTO TituloReceber { get; set; }
        public int? ImpostoFinanceiroId { get; set; }
        public ImpostoFinanceiroDTO ImpostoFinanceiro { get; set; }
        public decimal BaseCalculo { get; set; }
        public decimal ValorImposto { get; set; }
        public Nullable<DateTime> DataEmissaoDocumento { get; set; }
        public Nullable<DateTime> DataRecebimento { get; set; }

    }
}
