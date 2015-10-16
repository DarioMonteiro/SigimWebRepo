using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Financeiro;

namespace GIR.Sigim.Domain.Entity.OrdemCompra
{
    public class OrdemCompraFormaPagamento : BaseEntity
    {
        public int? OrdemCompraId { get; set; }
        public OrdemCompra OrdemCompra { get; set; }
        public DateTime Data { get; set; }
        public decimal Valor { get; set; }
        public int? TipoCompromissoId { get; set; }
        public TipoCompromisso TipoCompromisso { get; set; }
        public int? TituloPagarId { get; set; }
        public TituloPagar TituloPagar { get; set; }
        private bool? ehPagamentoAntecipado;
        public bool? EhPagamentoAntecipado
        {
            get { return ehPagamentoAntecipado.HasValue ? ehPagamentoAntecipado : false; }
            set { ehPagamentoAntecipado = value; }
        }

        private bool? ehUtilizada;
        public bool? EhUtilizada
        {
            get { return ehUtilizada; }
            set { ehUtilizada = value; }
        }

        public bool? EhAssociada { get; set; }
        public ICollection<EntradaMaterialFormaPagamento> ListaEntradaMaterialFormaPagamento { get; set; }

        public OrdemCompraFormaPagamento()
        {
            this.ListaEntradaMaterialFormaPagamento = new HashSet<EntradaMaterialFormaPagamento>();
        }
    }
}