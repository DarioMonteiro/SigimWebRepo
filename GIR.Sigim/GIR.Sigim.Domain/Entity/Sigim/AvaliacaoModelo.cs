using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Domain.Entity.Sigim
{
    public class AvaliacaoModelo : BaseEntity
    {
        public string Descricao { get; set; }
        public int MediaMinima { get; set; }
        public int Validade { get; set; }
        public TipoValidade TipoValidade { get; set; }
        public ICollection<AvaliacaoFornecedor> ListaAvaliacaoFornecedor { get; set; }

        public AvaliacaoModelo()
        {
            this.ListaAvaliacaoFornecedor = new HashSet<AvaliacaoFornecedor>();
        }
    }
}