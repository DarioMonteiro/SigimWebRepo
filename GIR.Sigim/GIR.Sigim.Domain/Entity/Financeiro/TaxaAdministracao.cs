using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Financeiro;
using GIR.Sigim.Domain.Entity.Sigim;

namespace GIR.Sigim.Domain.Entity.Financeiro
{
    public class TaxaAdministracao : BaseEntity
    {
        public CentroCusto CentroCusto { get; set; }
        public string CentroCustoId { get; set; }
        public Classe Classe { get; set; }
        public string ClasseId { get; set; }
        public ClienteFornecedor Cliente { get; set; }
        public int ClienteId { get; set; }
        public decimal Percentual { get; set; }

        public TaxaAdministracao() { }
    }
}
