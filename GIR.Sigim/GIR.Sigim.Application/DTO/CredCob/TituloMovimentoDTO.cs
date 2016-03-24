using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Financeiro;

namespace GIR.Sigim.Application.DTO.CredCob
{
    public class TituloMovimentoDTO : BaseDTO
    {
        public int? TituloCredCobId { get; set; }
        public TituloCredCobDTO TituloCredCob { get; set; }

        public int? MovimentoFinanceiroId { get; set; }
        public MovimentoFinanceiroDTO MovimentoFinanceiro { get; set; }

    }
}
