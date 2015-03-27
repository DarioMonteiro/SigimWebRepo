using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Financeiro;

namespace GIR.Sigim.Domain.Entity.Orcamento
{
    public class Orcamento : BaseEntity
    {
        public int? ObraId { get; set; }
        public Obra Obra { get; set; }
        public int? Sequencial { get; set; }
        public string Descricao { get; set; }
        public Nullable<DateTime> Data { get; set; }
        [Obsolete("Esta propriedade será removida em uma versão futura. Caso NÃO esteja codificando em um repositório, utilize a propriedade \"Ativo\"")]
        public string Situacao { get; set; }
        public bool Ativo
        {
            get { return Situacao == "A"; }
            set { Situacao = value ? "A" : "I"; }
        }
        public bool? EhControlado { get; set; }
        public ICollection<OrcamentoComposicao> ListaOrcamentoComposicao { get; set; }

        public Orcamento()
        {
            this.ListaOrcamentoComposicao = new HashSet<OrcamentoComposicao>();
        }
    }
}