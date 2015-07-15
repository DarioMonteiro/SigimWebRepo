using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Application.DTO.OrdemCompra
{
    public class RelOCItensOrdemCompraDTO : BaseDTO
    {

        public int OrdemCompraId { get; set; }
        public OrdemCompraDTO OrdemCompra { get; set; }

        public int OrdemCompraItemId { get; set; }
        public OrdemCompraItemDTO OrdemCompraItem { get; set; }

    }
}
