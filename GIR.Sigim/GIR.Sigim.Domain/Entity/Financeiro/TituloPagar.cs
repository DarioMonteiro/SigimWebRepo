using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Contrato;
using GIR.Sigim.Domain.Entity.OrdemCompra;
using GIR.Sigim.Domain.Entity.Sigim;

namespace GIR.Sigim.Domain.Entity.Financeiro
{
    public class TituloPagar : AbstractTitulo
    {
        public ClienteFornecedor Cliente { get; set; }
        public TipoCompromisso TipoCompromisso { get; set; }
        public TipoDocumento TipoDocumento { get; set; }
        public ContratoRetificacaoProvisao ContratoRetificacaoProvisao { get; set; }
        public int? TituloPaiId { get; set; }
        public virtual TituloPagar TituloPai { get; set; }
        public virtual ICollection<TituloPagar> ListaFilhos { get; set; }
        public virtual ICollection<ImpostoPagar> ListaImpostoPagar { get; set; }
        public ICollection<ContratoRetificacaoItemMedicao> ListaContratoRetificacaoItemMedicao { get; set; }
        public ICollection<OrdemCompraFormaPagamento> ListaOrdemCompraFormaPagamento { get; set; }
        public ICollection<EntradaMaterial> ListaEntradaMaterial { get; set; }
        public ICollection<EntradaMaterialFormaPagamento> ListaEntradaMaterialFormaPagamento { get; set; }

        public TituloPagar()
        {
            this.ListaFilhos = new HashSet<TituloPagar>();
            this.ListaImpostoPagar = new HashSet<ImpostoPagar>();
            this.ListaContratoRetificacaoItemMedicao = new HashSet<ContratoRetificacaoItemMedicao>();
            this.ListaOrdemCompraFormaPagamento = new HashSet<OrdemCompraFormaPagamento>();
            this.ListaEntradaMaterial = new HashSet<EntradaMaterial>();
            this.ListaEntradaMaterialFormaPagamento = new HashSet<EntradaMaterialFormaPagamento>();
        }
    }
}