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
        public int EmpresaId { get; set; }
        public Empresa Empresa { get; set; }
        public int ObraId { get; set; }
        public Obra Obra { get; set; }
        public int? Sequencial { get; set; }
        public string Descricao { get; set; }
        public Nullable<DateTime> Data { get; set; }
        //TODO: Alterar o campo situação no BD para int e criar um enum para representar a situação.
        public string Situacao { get; set; }
        public bool? EhControlado { get; set; }
        public ICollection<OrcamentoComposicao> ListaOrcamentoComposicao { get; set; }
        public ICollection<OrcamentoClasse> ListaOrcamentoClasse { get; set; }

        public Orcamento()
        {
            this.ListaOrcamentoComposicao = new HashSet<OrcamentoComposicao>();
            this.ListaOrcamentoClasse = new HashSet<OrcamentoClasse>();
        }
    }
}