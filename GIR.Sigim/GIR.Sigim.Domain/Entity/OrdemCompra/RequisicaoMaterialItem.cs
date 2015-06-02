using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Financeiro;
using GIR.Sigim.Domain.Entity.Orcamento;
using GIR.Sigim.Domain.Entity.Sigim;

namespace GIR.Sigim.Domain.Entity.OrdemCompra
{
    public class RequisicaoMaterialItem : AbstractRequisicaoMaterialItem
    {
        public virtual Material Material { get; set; }
        public int? RequisicaoMaterialId { get; set; }
        public RequisicaoMaterial RequisicaoMaterial { get; set; }
        public SituacaoRequisicaoMaterialItem Situacao { get; set; }
        public int? PreRequisicaoMaterialItemId { get; set; }
        public virtual PreRequisicaoMaterialItem PreRequisicaoMaterialItem { get; set; }
        public virtual ICollection<CotacaoItem> ListaCotacaoItem { get; set; }
        public virtual ICollection<OrdemCompraItem> ListaOrdemCompraItem { get; set; }
        public virtual ICollection<OrcamentoInsumoRequisitado> ListaOrcamentoInsumoRequisitado { get; set; }

        public RequisicaoMaterialItem()
        {
            this.ListaCotacaoItem = new HashSet<CotacaoItem>();
            this.ListaOrdemCompraItem = new HashSet<OrdemCompraItem>();
            this.ListaOrcamentoInsumoRequisitado = new HashSet<OrcamentoInsumoRequisitado>();
        }
    }
}