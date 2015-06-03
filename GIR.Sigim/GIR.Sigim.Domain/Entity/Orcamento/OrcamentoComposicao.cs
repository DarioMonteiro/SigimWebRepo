using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Financeiro;
using GIR.Sigim.Domain.Entity.Sigim;

namespace GIR.Sigim.Domain.Entity.Orcamento
{
    public class OrcamentoComposicao : BaseEntity
    {
        public int? OrcamentoId { get; set; }
        public Orcamento Orcamento { get; set; }
        public int? ComposicaoId { get; set; }
        public Composicao Composicao { get; set; }
        public string codigoClasse { get; set; }
        public Classe Classe { get; set; }
        public decimal? Quantidade { get; set; }
        public decimal? Preco { get; set; }
        public bool? EhSincronizada { get; set; }
        public string EspecificacaoTecnica { get; set; }
        public ICollection<OrcamentoComposicaoItem> ListaOrcamentoComposicaoItem { get; set; }

        public OrcamentoComposicao()
        {
            this.ListaOrcamentoComposicaoItem = new HashSet<OrcamentoComposicaoItem>();
        }
    }
}