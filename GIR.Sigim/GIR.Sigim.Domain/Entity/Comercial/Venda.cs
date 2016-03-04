using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Domain.Entity.Comercial
{
    public class Venda : BaseEntity
    {
        public ContratoComercial Contrato { get; set; }
        public Nullable<DateTime> DataVenda { get; set; }
    }
}
