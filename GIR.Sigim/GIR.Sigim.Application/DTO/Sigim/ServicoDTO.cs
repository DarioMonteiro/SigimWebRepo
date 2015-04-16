using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Application.DTO.Sigim
{
    public class ServicoDTO : BaseDTO 
    {
        public string Descricao { get; set; }
        public string SiglaUnidadeMedida { get; set; }
        public UnidadeMedidaDTO UnidadeMedida { get; set; }
        public decimal? PrecoUnitario { get; set; }
        public string Situacao { get; set; }
    }
}
