using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.OrdemCompra;

namespace GIR.Sigim.Domain.Entity.Sigim
{
    public class NaturezaReceita : BaseEntity
    {
        public string Codigo { get; set; }
        public string Descricao { get; set; }

        public ICollection<EntradaMaterialItem> ListaEntradaMaterialItem { get; set; }

        public NaturezaReceita()
        {
            this.ListaEntradaMaterialItem = new HashSet<EntradaMaterialItem>();
        }
    }
}