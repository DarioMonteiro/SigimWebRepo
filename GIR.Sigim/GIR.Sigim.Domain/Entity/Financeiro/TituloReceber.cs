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
        public int? TituloPaiId { get; set; }
        public virtual TituloReceber TituloPai { get; set; }
        public short? Parcela { get; set; }
        public decimal? ValorImposto { get; set; }

        public decimal? Desconto { get; set; }
        public Nullable<DateTime> DataLimiteDesconto { get; set; }
        public decimal? Multa { get; set; }
        public bool? EhMultaPercentual { get; set; }
        public decimal? TaxaPermanencia { get; set; }
        public bool? EhTaxaPermanenciaPercentual { get; set; }
        public string MotivoDesconto { get; set; }
        public Nullable<DateTime> DataRecebimento { get; set; }
        public decimal? ValorRecebido { get; set; }
        public string LoginUsuarioCadastro { get; set; }
        public Nullable<DateTime> DataCadastro { get; set; }
        public string LoginUsuarioSituacao { get; set; }
        public Nullable<DateTime> DataSituacao { get; set; }
        public string LoginUsuarioApropriacao { get; set; }
        public Nullable<DateTime> DataApropriacao { get; set; }
        public int? MotivoCancelamentoId { get; set; }
        public MotivoCancelamento MotivoCancelamento { get; set; }
        public Nullable<DateTime> DataBaixa { get; set; }
        public string SistemaOrigem { get; set; }
        public string Observacao { get; set; }

        public decimal? Retencao { get; set; }

        public virtual ICollection<TituloReceber> ListaFilhos { get; set; }
        public ICollection<ImpostoReceber> ListaImpostoReceber { get; set; }
        public ICollection<ContratoRetificacaoItemMedicao> ListaContratoRetificacaoItemMedicao { get; set; }
        public ICollection<ContratoRetencaoLiberada> ListaContratoRetencaoLiberada { get; set; }
        public ICollection<Apropriacao> ListaApropriacao { get; set; }
        public ICollection<ContratoRetificacaoProvisao> ListaContratoRetificacaoProvisao { get; set; }

        public TituloReceber()
        {
            this.ListaContratoRetificacaoItemMedicao = new HashSet<ContratoRetificacaoItemMedicao>();
            this.ListaContratoRetencaoLiberada = new HashSet<ContratoRetencaoLiberada>();
            this.ListaApropriacao = new HashSet<Apropriacao>();
            this.ListaImpostoReceber = new HashSet<ImpostoReceber>();
            this.ListaContratoRetificacaoProvisao = new HashSet<ContratoRetificacaoProvisao>();
        }
    }
}
