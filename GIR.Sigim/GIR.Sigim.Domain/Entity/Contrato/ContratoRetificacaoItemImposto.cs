using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Financeiro;

namespace GIR.Sigim.Domain.Entity.Contrato
{
    public class ContratoRetificacaoItemImposto : BaseEntity
    {
        public int ContratoId { get; set; }
        public Contrato Contrato { get; set; }
        public int ContratoRetificacaoId { get; set; }
        public ContratoRetificacao ContratoRetificacao { get; set; }
        public int ContratoRetificacaoItemId { get; set; }
        public ContratoRetificacaoItem ContratoRetificacaoItem { get; set; }
        public int ImpostoFinanceiroId { get; set; }
        public ImpostoFinanceiro ImpostoFinanceiro { get; set; }
        public decimal PercentualBaseCalculo { get; set; }
    }
}
