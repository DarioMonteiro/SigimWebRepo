using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Sigim;
using GIR.Sigim.Domain.Entity.Financeiro;

namespace GIR.Sigim.Domain.Entity.OrdemCompra
{
    public class OrdemCompra : BaseEntity
    {
        public string CodigoCentroCusto { get; set; }
        public CentroCusto CentroCusto { get; set; }
        public int ClienteFornecedorId { get; set; }
        public ClienteFornecedor ClienteFornecedor { get; set; }
        public DateTime Data { get; set; }
        public SituacaoOrdemCompra Situacao { get; set; }
        public int? TransportadoraId { get; set; }
        public ClienteFornecedor Transportadora { get; set; }
        public Nullable<DateTime> DataFrete { get; set; }
        public decimal? ValorFrete { get; set; }
        public int? TituloFreteId { get; set; }
        public TituloPagar TituloFrete { get; set; }
        public int? EntradaMaterialFreteId { get; set; }
        public EntradaMaterial EntradaMaterialFrete { get; set; }
        public int? PrazoEntrega { get; set; }

        private decimal? percentualDesconto;
        public decimal? PercentualDesconto
        {
            get { return percentualDesconto.HasValue ? percentualDesconto : 0; }
            set { percentualDesconto = value; }
        }

        public virtual ICollection<OrdemCompraItem> ListaItens { get; set; }
        public ICollection<OrdemCompraFormaPagamento> ListaOrdemCompraFormaPagamento { get; set; }
        public ICollection<EntradaMaterial> ListaEntradaMaterialFrete { get; set; }

        public OrdemCompra()
        {
            this.ListaItens = new HashSet<OrdemCompraItem>();
            this.ListaOrdemCompraFormaPagamento = new HashSet<OrdemCompraFormaPagamento>();
            this.ListaEntradaMaterialFrete = new HashSet<EntradaMaterial>();
        }
    }
}