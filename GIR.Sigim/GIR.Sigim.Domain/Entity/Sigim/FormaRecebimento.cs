using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Domain.Entity.Sigim
{
    public class FormaRecebimento : BaseEntity
    {
        public string Descricao { get; set; }
        public string TipoRecebimento { get; set; }
        public bool? Automatico { get; set; }
        public int? NumeroDias { get; set; }
    }
}