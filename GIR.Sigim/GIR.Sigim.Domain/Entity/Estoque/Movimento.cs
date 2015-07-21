using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.OrdemCompra;

namespace GIR.Sigim.Domain.Entity.Estoque
{
    public class Movimento : BaseEntity
    {
        public int? EntradaMaterialId { get; set; }
        public EntradaMaterial EntradaMaterial { get; set; }
    }
}