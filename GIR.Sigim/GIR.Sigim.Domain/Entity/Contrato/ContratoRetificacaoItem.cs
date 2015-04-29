using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Financeiro;
using GIR.Sigim.Domain.Entity.Sigim;

namespace GIR.Sigim.Domain.Entity.Contrato
{
    public class ContratoRetificacaoItem : BaseEntity
    {
        public int ContratoId { get; set; }
        public Contrato Contrato { get; set; }
        public int ContratoRetificacaoId { get; set; }
        public ContratoRetificacao ContratoRetificacao { get; set; }
        public Int16 Sequencial { get; set; }
        public string ComplementoDescricao { get; set; }
        public NaturezaItem NaturezaItem { get; set; }
        public int ServicoId { get; set; }
        public Servico Servico { get; set; }
        public decimal Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
        public decimal? ValorItem { get; set; }
        public string CodigoClasse { get; set; }
        public Classe Classe { get; set;}
        public decimal? RetencaoItem { get; set; }
        public decimal? BaseRetencaoItem { get; set; }
        public int? RetencaoPrazoResgate { get; set; }
        public bool? Alterado { get; set; }
        public int? RetencaoTipoCompromissoId { get; set; }
        public TipoCompromisso RetencaoTipoCompromisso { get; set; }

        public ICollection<ContratoRetificacaoProvisao> ListaContratoRetificacaoProvisao { get; set; }
        public ICollection<ContratoRetificacaoItemCronograma> ListaContratoRetificacaoItemCronograma { get; set; }
        public ICollection<ContratoRetificacaoItemMedicao> ListaContratoRetificacaoItemMedicao { get; set; }

        public ContratoRetificacaoItem()
        {
            ListaContratoRetificacaoProvisao = new HashSet<ContratoRetificacaoProvisao>();
            ListaContratoRetificacaoItemCronograma = new HashSet<ContratoRetificacaoItemCronograma>();
            ListaContratoRetificacaoItemMedicao = new HashSet<ContratoRetificacaoItemMedicao>();
        }
    }
}
