using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Financeiro;

namespace GIR.Sigim.Domain.Entity.OrdemCompra
{
    public class EntradaMaterialFormaPagamento : BaseEntity
    {
        public int? EntradaMaterialId { get; set; }
        public EntradaMaterial EntradaMaterial { get; set; }
        public int? OrdemCompraFormaPagamentoId { get; set; }
        public OrdemCompraFormaPagamento OrdemCompraFormaPagamento { get; set; }
        public DateTime Data { get; set; }
        public decimal Valor { get; set; }
        public int? TipoCompromissoId { get; set; }
        public TipoCompromisso TipoCompromisso { get; set; }
        public int? TituloPagarId { get; set; }
        public TituloPagar TituloPagar { get; set; }
        public ICollection<TituloPagarAdiantamento> ListaTituloPagarAdiantamento { get; set; }

        public EntradaMaterialFormaPagamento()
        {
            this.ListaTituloPagarAdiantamento = new HashSet<TituloPagarAdiantamento>();
        }
    }
}