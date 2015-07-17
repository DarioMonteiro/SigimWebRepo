using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Domain.Entity.Sigim
{
    public class MaterialClasseInsumo : BaseEntity
    {
        public string Codigo { get; set; }
        public string Descricao { get; set; }
        public bool? EhMovimentoTemporario { get; set; }
        public int? Sequencial { get; set; }
        public bool? NaoGeraSPED { get; set; }
        public string CodigoPai { get; set; }
        public MaterialClasseInsumo ClassePai { get; set; }
        public virtual ICollection<MaterialClasseInsumo> ListaFilhos { get; set; }

        public ICollection<Material> ListaMaterial { get; set; }

        public MaterialClasseInsumo()
        {
            this.ListaFilhos = new HashSet<MaterialClasseInsumo>();
            this.ListaMaterial = new HashSet<Material>();
        }
    }
}