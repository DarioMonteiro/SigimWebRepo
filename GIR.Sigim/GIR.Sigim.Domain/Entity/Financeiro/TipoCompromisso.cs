using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Domain.Entity.Financeiro
{
    public class TipoCompromisso : BaseEntity
    {
        public string Descricao { get; set; }
        public bool? GeraTitulo { get; set; }
        //TODO: Juntar as infomações dos campos TipoPagar e TipoReceber em um único campo
        public bool? TipoPagar { get; set; }
        public bool? TipoReceber { get; set; }
        
        public ICollection<OrdemCompra.ParametrosOrdemCompra> ListaParametrosOrdemCompra { get; set; }

        public TipoCompromisso()
        {
            this.ListaParametrosOrdemCompra = new HashSet<OrdemCompra.ParametrosOrdemCompra>();
        }
    }
}