using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Domain.Entity.Orcamento
{
    public class ParametrosOrcamento : BaseEntity
    {
        public string MascaraClasseInsumo { get; set; }
        public string EmpresaNomeRaiz { get; set; }
        public string EmpresaResponsavel { get; set; }
        public byte[] IconeRelatorio { get; set; }
    }
}