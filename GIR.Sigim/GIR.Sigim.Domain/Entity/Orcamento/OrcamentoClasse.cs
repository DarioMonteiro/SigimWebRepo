using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Financeiro;

namespace GIR.Sigim.Domain.Entity.Orcamento
{
    public class OrcamentoClasse : BaseEntity
    {
        public int OrcamentoId { get; set; }
        public Orcamento Orcamento { get; set; }
        public string ClasseCodigo { get; set; }
        public Classe Classe { get; set; }
        public bool? Fechada { get; set; }
        public bool? Controlada { get; set; }
    }
}
