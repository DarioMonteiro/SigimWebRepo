using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Financeiro;

namespace GIR.Sigim.Domain.Entity.CredCob
{
    public class VerbaCobranca : BaseEntity
    {
        public string Descricao { get; set; }
        public string CodigoClasse { get; set; }
        public Classe Classe { get; set; }
        public bool? Automatico { get; set; }

        public ICollection<TituloCredCob> ListaTituloCredCob { get; set; }

        public VerbaCobranca()
        {
            this.ListaTituloCredCob = new HashSet<TituloCredCob>();
        }

    }
}
