using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Domain.Entity.Sigim
{
    public class UnidadeMedida : BaseEntity
    {
        public string Sigla { get; set; }
        public string Descricao { get; set; }
        public ICollection<Material> ListaMaterial { get; set; }
        public ICollection<Servico> ListaServico { get; set; } 

        public UnidadeMedida()
        {
            this.ListaMaterial = new HashSet<Material>();
            this.ListaServico = new HashSet<Servico>(); 
        }
    }
}