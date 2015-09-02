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

        //TODO: Tornar campo obrigatório no banco com valor default 0
        private decimal? quantidadeEntregue;
        public decimal? QuantidadeEntregue
        {
            get { return quantidadeEntregue.HasValue ? quantidadeEntregue : 0; }
            set { quantidadeEntregue = value; }
        }

        public decimal? Saldo { get { return Quantidade - QuantidadeEntregue; } }
        
        public decimal? ValorUnitario { get; set; }

        private decimal? percentualIPI;
        public decimal? PercentualIPI
        {
            get { return percentualIPI.HasValue ? percentualIPI : 0; }
            set { percentualIPI = value; }
        }

        private decimal? percentualDesconto;
        public decimal? PercentualDesconto
        {
            get { return percentualDesconto.HasValue ? percentualDesconto : 0; }
            set { percentualDesconto = value; }
        }

        public decimal? ValorTotalComImposto { get; set; }
        public decimal? ValorTotalItem { get; set; }

        public ICollection<EntradaMaterialItem> ListaEntradaMaterialItem { get; set; }

        public OrdemCompraItem()
        {
            this.ListaEntradaMaterialItem = new HashSet<EntradaMaterialItem>();
        }
    }
}