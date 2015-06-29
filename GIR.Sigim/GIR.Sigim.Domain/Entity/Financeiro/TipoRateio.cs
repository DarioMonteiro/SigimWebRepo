using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Domain.Entity.Financeiro
{
    public class TipoRateio : BaseEntity
    {
        public string Descricao { get; set; }
        public ICollection<RateioAutomatico> ListaRateioAutomatico { get; set; }

        public TipoRateio()
        {
            this.ListaRateioAutomatico = new HashSet<RateioAutomatico>();
        }
    }
}
