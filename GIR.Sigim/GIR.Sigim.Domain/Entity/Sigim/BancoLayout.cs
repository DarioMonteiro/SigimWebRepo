using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Domain.Entity.Sigim
{
    public class BancoLayout : BaseEntity
    {
        public string Descricao { get; set; }
        public short Padrao { get; set; }
        public TipoLayout Tipo { get; set; }
        public bool? DesconsideraPosicao { get; set; }
        public int? BancoId { get; set; }
        public Banco Banco { get; set; }
        public ICollection<OrdemCompra.ParametrosOrdemCompra> ListaParametrosOrdemCompra { get; set; }

        public BancoLayout()
        {
            this.ListaParametrosOrdemCompra = new HashSet<OrdemCompra.ParametrosOrdemCompra>();
        }
    }
}