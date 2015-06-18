using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Financeiro;

namespace GIR.Sigim.Domain.Entity.Contrato
{
    public class ContratoRetificacao : BaseEntity
    {
        public int ContratoId { get; set; }
        public Contrato Contrato { get; set; }
        public int Sequencial { get; set; }
        public bool Aprovada { get; set; }
        public Nullable<DateTime> DataAprovacao { get; set; }
        public string UsuarioAprovacao { get; set; }
        public string Motivo { get; set; }
        public string Observacao { get; set; }
        public string Anotacoes { get; set; }
        public string ReferenciaDigital { get; set; }
        public decimal? RetencaoContratual { get; set; }
        public int? RetencaoPrazoResgate { get; set; }
        public int? RetencaoTipoCompromissoId { get; set; }
        public TipoCompromisso RetencaoTipoCompromisso { get; set; }

        public ICollection<ContratoRetificacaoItem> ListaContratoRetificacaoItem { get; set; }
        public ICollection<ContratoRetificacaoProvisao> ListaContratoRetificacaoProvisao { get; set; }
        public ICollection<ContratoRetificacaoItemCronograma> ListaContratoRetificacaoItemCronograma { get; set; }
        public ICollection<ContratoRetificacaoItemMedicao> ListaContratoRetificacaoItemMedicao { get; set; }
        public ICollection<ContratoRetificacaoItemImposto> ListaContratoRetificacaoItemImposto { get; set; }

        public ContratoRetificacao()
        {
            this.ListaContratoRetificacaoItem = new HashSet<ContratoRetificacaoItem>();
            this.ListaContratoRetificacaoProvisao = new HashSet<ContratoRetificacaoProvisao>();
            this.ListaContratoRetificacaoItemCronograma = new HashSet<ContratoRetificacaoItemCronograma>();
            this.ListaContratoRetificacaoItemMedicao = new HashSet<ContratoRetificacaoItemMedicao>();
            this.ListaContratoRetificacaoItemImposto = new HashSet<ContratoRetificacaoItemImposto>();
        }
    }
}
