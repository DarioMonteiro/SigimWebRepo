using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Domain.Entity.Sigim
{
    public class Feriado : BaseEntity
    {
        public Nullable<DateTime> Data { get; set; }
        public string Descricao { get; set; }
    }
}