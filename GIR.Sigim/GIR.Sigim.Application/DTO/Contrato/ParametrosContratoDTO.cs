using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Application.DTO.Contrato
{
    public class ParametrosContratoDTO : BaseDTO
    {
        public int? DiasPagamento { get; set; }
        public int? DiasMedicao { get; set; }
        public string MascaraClasseInsumo { get; set; }
        public byte[] IconeRelatorio { get; set; }
    }
}