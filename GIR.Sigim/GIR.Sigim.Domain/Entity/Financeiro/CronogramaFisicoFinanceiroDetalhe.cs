using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Domain.Entity.Financeiro
{
    public class CronogramaFisicoFinanceiroDetalhe : BaseEntity
    {
        public int CronogramaFisicoFinanceiroId { get; set; }
        public CronogramaFisicoFinanceiro CronogramaFisicoFinanceiro { get; set; }
        public string CodigoClasse { get; set; }
        public Classe Classe { get; set; }
        public decimal Percentual { get; set; }
    }
}
