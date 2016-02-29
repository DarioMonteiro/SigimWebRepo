using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Domain.Entity.Comercial
{
    public class TipoParticipante : BaseEntity
    {
        public string Descricao { get; set; }
        public bool? Automatico { get; set; }

        public ICollection<VendaParticipante> ListaVendaParticipante { get; set; }

        public TipoParticipante()
        {
            this.ListaVendaParticipante = new HashSet<VendaParticipante>();
        }

    }
}
