using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Domain.Entity.Financeiro
{
    public class CronogramaFisicoFinanceiro : BaseEntity
    {
        public DateTime DataElaboracao { get; set; }
        public string CodigoCentroCusto { get; set; }
        public CentroCusto CentroCusto { get; set; }
        public int Sequencial { get; set; }

        public ICollection<CronogramaFisicoFinanceiroDetalhe> ListaCronogramaFisicoFinanceiroDetalhe { get; set; }

        public CronogramaFisicoFinanceiro()
        {
            this.ListaCronogramaFisicoFinanceiroDetalhe = new HashSet<CronogramaFisicoFinanceiroDetalhe>();
        }
    }
}
