using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Financeiro;

namespace GIR.Sigim.Application.DTO.OrdemCompra
{
    public class EntradaMaterialImpostoDTO : BaseDTO
    {
        public ImpostoFinanceiroDTO ImpostoFinanceiro { get; set; }
        public Nullable<DateTime> DataVencimento { get; set; }
        public decimal BaseCalculo { get; set; }
        public decimal Valor { get; set; }
        public int? TituloPagar { get; set; }
    }
}