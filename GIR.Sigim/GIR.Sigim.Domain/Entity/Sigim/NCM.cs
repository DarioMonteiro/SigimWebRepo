using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Domain.Entity.Sigim
{
    public class NCM : BaseEntity
    {
        public string Codigo { get; set; }
        public string Descricao { get; set; }
        public ICollection<Material> ListaMaterial { get; set; }

        public NCM()
        {
            this.ListaMaterial = new HashSet<Material>();
        }
    }
}