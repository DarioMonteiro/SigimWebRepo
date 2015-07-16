using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Application.DTO.OrdemCompra
{
    public class RelOCItensOrdemCompraDTO : BaseDTO
    {
        public OrdemCompraDTO OrdemCompra { get; set; }
        public OrdemCompraItemDTO OrdemCompraItem { get; set; }
    }
}
