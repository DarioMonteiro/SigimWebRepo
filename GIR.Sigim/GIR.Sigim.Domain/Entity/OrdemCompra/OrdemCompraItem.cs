using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Financeiro;
using GIR.Sigim.Domain.Entity.Sigim;

namespace GIR.Sigim.Domain.Entity.OrdemCompra
{
    public class OrdemCompraItem : BaseEntity
    {
        public int? OrdemCompraId { get; set; }
        public virtual OrdemCompra OrdemCompra { get; set; }
        public int? RequisicaoMaterialItemId { get; set; }
        public RequisicaoMaterialItem RequisicaoMaterialItem { get; set; }
        public int? CotacaoItemId { get; set; }
        public CotacaoItem CotacaoItem { get; set; }
        public int? MaterialId { get; set; }
        public Material Material { get; set; }
        public string CodigoClasse { get; set; }
        public Classe Classe { get; set; }
        public int Sequencial { get; set; }
        public string Complemento { get; set; }
        public decimal? Quantidade { get; set; }
        public decimal? QuantidadeEntregue { get; set; }
        public decimal? ValorUnitario { get; set; }
        public decimal? PercentualIPI { get; set; }
        public decimal? PercentualDesconto { get; set; }
        public decimal? ValorTotalComImposto { get; set; }
        public decimal? ValorTotalItem { get; set; }

        public ICollection<EntradaMaterialItem> ListaEntradaMaterialItem { get; set; }

        public OrdemCompraItem()
        {
            this.ListaEntradaMaterialItem = new HashSet<EntradaMaterialItem>();
        }
    }
}