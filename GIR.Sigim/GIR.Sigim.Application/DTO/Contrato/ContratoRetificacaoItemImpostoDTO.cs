using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Financeiro;

namespace GIR.Sigim.Application.DTO.Contrato
{
    public class ContratoRetificacaoItemImpostoDTO : BaseDTO
    {

        public int ContratoId { get; set; }
        public ContratoDTO Contrato { get; set; }
        public int ContratoRetificacaoId { get; set; }
        public ContratoRetificacaoDTO ContratoRetificacao { get; set; }
        public int ContratoRetificacaoItemId { get; set; }
        public ContratoRetificacaoItemDTO ContratoRetificacaoItem { get; set; }
        public int ImpostoFinanceiroId { get; set; }
        public ImpostoFinanceiroDTO ImpostoFinanceiro { get; set; }
        public decimal PercentualBaseCalculo { get; set; }

        public decimal ValorImposto { get; set; }

    }
}
