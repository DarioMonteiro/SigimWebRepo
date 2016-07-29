using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Financeiro;

namespace GIR.Sigim.Domain.Entity.Orcamento
{
    public class Obra : BaseEntity
    {
        public int? EmpresaId { get; set; }
        public Empresa Empresa { get; set; }
        public string Numero { get; set; }
        public string Descricao { get; set; }
        public string CodigoCentroCusto { get; set; }
        public CentroCusto CentroCusto { get; set; }
        public bool? OrcamentoSimplificado { get; set; }
        public Nullable<Decimal> BDIPercentual { get; set; }
        public Nullable<Decimal> AreaConstrucaoAreaReal { get; set; }
        public Nullable<Decimal> AreaConstrucaoAreaEquivalente { get; set; }


        public ICollection<Orcamento> ListaOrcamento { get; set; }

        public Obra()
        {
            this.ListaOrcamento = new HashSet<Orcamento>();
        }
    }
}