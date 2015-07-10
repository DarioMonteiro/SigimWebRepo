using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Sigim;

namespace GIR.Sigim.Domain.Entity.Financeiro
{
    public class CentroCustoEmpresa : BaseEntity
    {
        public string CodigoCentroCusto { get; set; }
        public CentroCusto CentroCusto { get; set; }
        public int? ClienteId { get; set; }
        public virtual ClienteFornecedor Cliente { get; set; }
        public string EnderecoEntrega { get; set; }
        public string EnderecoCobranca { get; set; }
        public string ResponsavelObra { get; set; }
        public byte[] IconeRelatorio { get; set; }
        public int? CodigoEnderecoConstrucompras { get; set; }
        public int? CodigoObraConstrucompras { get; set; }
        public string CaminhoConstrucompras { get; set; }
        public string PracaPagamento { get; set; }
        public bool? EhClasseOrcamento { get; set; }
    }
}