using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Sigim;

namespace GIR.Sigim.Application.DTO.Contrato
{
    public class ParametrosContratoDTO : BaseDTO
    {
        public int? ClienteId { get; set; }
        public ClienteFornecedorDTO Cliente { get; set; }
        public string MascaraClasseInsumo { get; set; }
        public byte[] IconeRelatorio { get; set; }
        public int? DiasMedicao { get; set; }
        public int? DiasPagamento { get; set; }
        public bool? DadosSped { get; set; }
    }
}