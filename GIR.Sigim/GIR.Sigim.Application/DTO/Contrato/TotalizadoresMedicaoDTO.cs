using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Application.DTO.Contrato
{
    public class TotalizadoresMedicaoDTO
    {
        public decimal ValorTotalMedido { get; set; }
        public decimal QuantidadeTotalMedida { get; set; }
        public decimal ValorTotalLiberado { get; set; }
        public decimal QuantidadeTotalLiberada { get; set; }
        public decimal QuantidadeTotalMedidaLiberada { get; set; }
        public decimal ValorTotalMedidoLiberado { get; set; }

        public decimal QuantidadePendente { get; set; }
        public decimal ValorPendente { get; set; }



    }
}
