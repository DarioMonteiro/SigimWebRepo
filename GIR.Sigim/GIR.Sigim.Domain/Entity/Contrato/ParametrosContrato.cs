using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Domain.Entity.Contrato
{
    public class ParametrosContrato : BaseEntity
    {
        public string MascaraClasseInsumo { get; set; }
        public byte[] IconeRelatorio { get; set; }
    }
}