using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Financeiro;
using GIR.Sigim.Domain.Entity.Sigim;

namespace GIR.Sigim.Domain.Entity.OrdemCompra
{
    public class PreRequisicaoMaterialItem : AbstractRequisicaoMaterialItem
    {
        public int? PreRequisicaoMaterialId { get; set; }
        public PreRequisicaoMaterial PreRequisicaoMaterial { get; set; }
        public string CodigoCentroCusto { get; set; }
        public virtual CentroCusto CentroCusto { get; set; }
        public SituacaoPreRequisicaoMaterialItem Situacao { get; set; }
        public Nullable<DateTime> DataAprovacao { get; set; }
        //TODO: Criar relação com a classe Usuario
        public string LoginUsuarioAprovacao { get; set; }
        public virtual ICollection<RequisicaoMaterialItem> ListaRequisicaoMaterialItem { get; set; }

        public PreRequisicaoMaterialItem()
        {
            this.ListaRequisicaoMaterialItem = new HashSet<RequisicaoMaterialItem>();
        }
    }
}