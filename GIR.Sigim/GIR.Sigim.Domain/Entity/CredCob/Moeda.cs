using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Sigim;

namespace GIR.Sigim.Domain.Entity.CredCob
{
    public class Moeda : BaseEntity
    {
        public string Simbolo { get; set; }
        public string Descricao { get; set; }

        public ICollection<ParametrosSigim> ListaParametrosSigim { get; set; }

        public Moeda()
        {
            this.ListaParametrosSigim = new HashSet<ParametrosSigim>();
        }
    }
}
