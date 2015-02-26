using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Admin;
using GIR.Sigim.Domain.Entity.Financeiro;

namespace GIR.Sigim.Domain.Entity.Sigim
{
    public class UsuarioCentroCusto : BaseEntity
    {
        public int? UsuarioId { get; set; }
        public virtual Usuario Usuario { get; set; }
        public int? ModuloId { get; set; }
        public virtual Modulo Modulo { get; set; }
        public string CodigoCentroCusto { get; set; }
        public virtual CentroCusto CentroCusto { get; set; }
    }
}