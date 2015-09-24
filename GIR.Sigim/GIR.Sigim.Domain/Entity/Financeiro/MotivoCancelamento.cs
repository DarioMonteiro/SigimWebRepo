using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Domain.Entity.Financeiro
{
    public class MotivoCancelamento : BaseEntity
    {
        public string Descricao { get; set; }

        public ICollection<TituloReceber> ListaTituloReceber { get; set; }
        public ICollection<TituloPagar> ListaTituloPagar { get; set; }

        public MotivoCancelamento()
        {
            this.ListaTituloPagar = new HashSet<TituloPagar>();
            this.ListaTituloReceber = new HashSet<TituloReceber>();
        }

    }
}