using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Domain.Entity.Sigim
{
    public class ParametrosSigim : BaseEntity, IParametros
    {
        public decimal PercentualMultaAtraso { get; set; }
        public decimal PercentualEncargosAtraso { get; set; }
        public byte CorrecaoProRata { get; set; }
        public int? IndiceVendas { get; set; }
        public string MetodoDescapitalizacao { get; set; }
        public int ClienteId { get; set; }
        public ClienteFornecedor Cliente { get; set; }
        public byte[] IconeRelatorio { get; set; }
        public bool? AplicaEncargosPorMes { get; set; }
        public bool? CorrecaoMesCheioDiaPrimeiro { get; set; }
    }
}
