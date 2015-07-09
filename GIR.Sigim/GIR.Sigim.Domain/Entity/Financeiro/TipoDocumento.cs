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
    public class TipoDocumento : BaseEntity
    {
        public string Sigla { get; set; }
        public string Descricao { get; set; }

        public ICollection<TituloPagar> ListaTituloPagar { get; set; }
        public ICollection<TituloReceber> ListaTituloReceber { get; set; }
        public ICollection<ContratoRetificacaoItemMedicao> ListaContratoRetificacaoItemMedicao { get; set; }
        public ICollection<EntradaMaterial> ListaEntradaMaterialNotaFiscal { get; set; }
        public ICollection<EntradaMaterial> ListaEntradaMaterialNotaFrete { get; set; }
        public ICollection<AvaliacaoFornecedor> ListaAvaliacaoFornecedor { get; set; }

        public TipoDocumento()
        {
            this.ListaTituloPagar = new HashSet<TituloPagar>();
            this.ListaTituloReceber = new HashSet<TituloReceber>();
            this.ListaContratoRetificacaoItemMedicao = new HashSet<ContratoRetificacaoItemMedicao>();
            this.ListaEntradaMaterialNotaFiscal = new HashSet<EntradaMaterial>();
            this.ListaEntradaMaterialNotaFrete = new HashSet<EntradaMaterial>();
            this.ListaAvaliacaoFornecedor = new HashSet<AvaliacaoFornecedor>();
        }
    }
}