using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Domain.Entity.Financeiro
{
    public class MovimentoFinanceiro : BaseEntity
    {
        public int TipoMovimentoId { get; set; }
        public TipoMovimento TipoMovimento { get; set; }
    }

}
