using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Financeiro;
using GIR.Sigim.Domain.Entity.Sigim;

namespace GIR.Sigim.Domain.Entity.Financeiro
{
    public class RateioAutomatico : BaseEntity
    {
        public TipoRateio TipoRateio { get; set; }
        public int TipoRateioId { get; set; }
        public Classe Classe { get; set; }
        public string ClasseId { get; set; }
        public CentroCusto CentroCusto { get; set; }
        public string CentroCustoId { get; set; }
        public decimal Percentual { get; set; }

        public RateioAutomatico() { }
    }
}
