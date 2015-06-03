using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Domain.Entity.OrdemCompra
{
    public class CotacaoItem : BaseEntity
    {
        public int? CotacaoId { get; set; }
        public virtual Cotacao Cotacao { get; set; }
        public int? RequisicaoMaterialItemId { get; set; }
        public RequisicaoMaterialItem RequisicaoMaterialItem { get; set; }
        //TODO: Relacionar com Fornecedor Vencedor
        public int Sequencial { get; set; }
        public decimal Quantidade { get; set; }
        public int? OrdemCompraItemUltimoPreco { get; set; }
        public decimal? ValorUltimoPreco { get; set; }
        public Nullable<DateTime> DataUltimoPreco { get; set; }
        public decimal? ValorOrcado { get; set; }
        public string LoginUsuarioEleicao { get; set; }
        public Nullable<DateTime> DataEleicao { get; set; }
        public virtual ICollection<OrdemCompraItem> ListaOrdemCompraItem { get; set; }

        public CotacaoItem()
        {
            this.ListaOrdemCompraItem = new HashSet<OrdemCompraItem>();
        }
    }
}