using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Domain.Entity.Comercial
{
    public class Empreendimento : BaseEntity
    {
        public string Nome { get; set; }
        public int? EnderecoId { get; set; }
        public int IncorporadorId { get; set; }
        public Incorporador Incorporador { get; set; }
        
        public ICollection<Bloco> ListaBloco { get; set; }
        public ICollection<Unidade> ListaUnidade { get; set; }

        public Empreendimento()
        {
            this.ListaBloco = new HashSet<Bloco>();
            this.ListaUnidade = new HashSet<Unidade>();
        }

    }
}
