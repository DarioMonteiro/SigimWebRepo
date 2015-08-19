using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.OrdemCompra;

namespace GIR.Sigim.Application.DTO.Estoque
{
    public class MovimentoDTO : BaseDTO
    {
        public EntradaMaterialDTO EntradaMaterial { get; set; }
    }
}