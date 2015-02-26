using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Financeiro;
using GIR.Sigim.Domain.Entity.Sigim;

namespace GIR.Sigim.Domain.Entity.OrdemCompra
{
    public class RequisicaoMaterialItem : AbstractRequisicaoMaterialItem
    {
        public int? RequisicaoMaterialId { get; set; }
        public RequisicaoMaterial RequisicaoMaterial { get; set; }
        public SituacaoRequisicaoMaterialItem Situacao { get; set; }
        public int? PreRequisicaoMaterialItemId { get; set; }
        public virtual PreRequisicaoMaterialItem PreRequisicaoMaterialItem { get; set; }
    }
}