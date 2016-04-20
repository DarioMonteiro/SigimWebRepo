using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Sigim;

namespace GIR.Sigim.Domain.Entity.Orcamento
{
    public class Empresa : BaseEntity
    {
        public string Numero { get; set; }
        public int ClienteFornecedorId { get; set; }
        public ClienteFornecedor ClienteFornecedor { get; set; }

        public ICollection<Orcamento> ListaOrcamento { get; set; }
        public ICollection<Obra> ListaObra { get; set; }

        public Empresa()
        {
            this.ListaOrcamento = new HashSet<Orcamento>();
            this.ListaObra = new HashSet<Obra>();
        }

    }
}
