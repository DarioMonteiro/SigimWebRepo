using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Financeiro;
using GIR.Sigim.Domain.Entity.Sigim;

namespace GIR.Sigim.Domain.Entity.OrdemCompra
{
    public class EmpresaCentroCusto : BaseEntity
    {
        public int? ClienteId { get; set; }
        public virtual ClienteFornecedor Cliente { get; set; }
        public string CodigoCentroCusto { get; set; }
        public CentroCusto CentroCusto { get; set; }
        public string EnderecoEntrega { get; set; }
        public string EnderecoCobranca { get; set; }
    }
}