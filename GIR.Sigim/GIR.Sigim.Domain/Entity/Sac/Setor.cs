using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Domain.Entity.Sac
{
    public class Setor : BaseEntity
    {
        public string Descricao { get; set; }

        public ICollection<ParametrosEmailSac> ListaParametrosEmailSac { get; set; }

        public Setor()
        {
            this.ListaParametrosEmailSac = new HashSet<ParametrosEmailSac>();
        }

    }
}