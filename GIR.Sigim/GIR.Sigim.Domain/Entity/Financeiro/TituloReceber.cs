using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Contrato;
using GIR.Sigim.Domain.Entity.Sigim;

namespace GIR.Sigim.Domain.Entity.Financeiro
{
    public class TituloReceber : AbstractTitulo
    {
        public ClienteFornecedor Cliente { get; set; }
        public SituacaoTituloReceber Situacao { get; set; }
        public TipoCompromisso TipoCompromisso { get; set; }
        public TipoDocumento TipoDocumento { get; set; }
        public ContratoRetificacaoProvisao ContratoRetificacaoProvisao { get; set; }

        public ICollection<ContratoRetificacaoItemMedicao> ListaContratoRetificacaoItemMedicao { get; set; }
        public ICollection<ContratoRetencaoLiberada> ListaContratoRetencaoLiberada { get; set; }

        public TituloReceber()
        {
            this.ListaContratoRetificacaoItemMedicao = new HashSet<ContratoRetificacaoItemMedicao>();
            this.ListaContratoRetencaoLiberada = new HashSet<ContratoRetencaoLiberada>();
        }
    }
}
