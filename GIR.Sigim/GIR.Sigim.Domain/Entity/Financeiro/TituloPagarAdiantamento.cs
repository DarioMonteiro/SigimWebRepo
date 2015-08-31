using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.OrdemCompra;
using GIR.Sigim.Domain.Entity.Sigim;

namespace GIR.Sigim.Domain.Entity.Financeiro
{
    public class TituloPagarAdiantamento : BaseEntity
    {
        public int? ClienteId { get; set; }
        public ClienteFornecedor Cliente { get; set; }
        public string Identificacao { get; set; }
        public int? TipoDocumentoId { get; set; }
        public TipoDocumento TipoDocumento { get; set; }
        public string Documento { get; set; }
        public DateTime DataEmissaoDocumento { get; set; }
        public decimal ValorAdiantamento { get; set; }
        public string LoginUsuarioCadastro { get; set; }
        public DateTime DataCadastro { get; set; }
        public int? TituloPagarId { get; set; }
        public TituloPagar TituloPagar { get; set; }
        public int? EntradaMaterialFormaPagamentoId { get; set; }
        public EntradaMaterialFormaPagamento EntradaMaterialFormaPagamento { get; set; }

        public ICollection<ApropriacaoAdiantamento> ListaApropriacaoAdiantamento { get; set; }

        public TituloPagarAdiantamento()
        {
            this.ListaApropriacaoAdiantamento = new HashSet<ApropriacaoAdiantamento>();
        }
    }
}