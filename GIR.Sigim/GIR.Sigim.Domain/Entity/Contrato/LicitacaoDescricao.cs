using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Domain.Entity.Contrato
{
    public class LicitacaoDescricao : BaseEntity 
    {
        public string Descricao { get; set; }

        public ICollection<Contrato> ListaContrato { get; set; }
        public ICollection<LicitacaoCronograma> ListaLicitacaoCronograma { get; set; }

        public LicitacaoDescricao() 
        {
            this.ListaContrato = new HashSet<Contrato>();
            this.ListaLicitacaoCronograma = new HashSet<LicitacaoCronograma>(); 
        }
    }
}
