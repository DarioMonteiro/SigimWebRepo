using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.OrdemCompra;
using GIR.Sigim.Domain.Entity.Sigim;

namespace GIR.Sigim.Domain.Entity.Financeiro
{
    public class Classe : BaseEntity
    {
        public string Codigo { get; set; }
        public string Descricao { get; set; }
        public string ContaContabil { get; set; }
        public string CodigoPai { get; set; }
        public Classe ClassePai { get; set; }
        public virtual ICollection<Classe> ListaFilhos { get; set; }
        public ICollection<PreRequisicaoMaterialItem> ListaPreRequisicaoMaterialItem { get; set; }
        public ICollection<RequisicaoMaterialItem> ListaRequisicaoMaterialItem { get; set; }

        public Classe()
        {
            this.ListaFilhos = new HashSet<Classe>();
            this.ListaPreRequisicaoMaterialItem = new HashSet<PreRequisicaoMaterialItem>();
            this.ListaRequisicaoMaterialItem = new HashSet<RequisicaoMaterialItem>();
        }
    }
}