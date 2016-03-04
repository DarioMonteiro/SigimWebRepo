using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Domain.Entity.Sigim
{
    public class ParametrosSigim : BaseEntity
    {
        public int? IndiceVendas { get; set; }
        public bool? CorrecaoMesCheioDiaPrimeiro { get; set; }
        public string MetodoDescapitalizacao { get; set; }
    }
}
