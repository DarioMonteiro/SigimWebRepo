using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Sigim;

namespace GIR.Sigim.Domain.Entity.Comercial
{
    public class VendaParticipante : BaseEntity
    {
        public int ContratoId { get; set; }
        public ContratoComercial Contrato { get; set; }
        public int ClienteId { get; set; }
        public ClienteFornecedor Cliente { get; set; }
        public int? TipoParticipanteId { get; set; }
        public TipoParticipante TipoParticipante { get; set; }
        public Decimal PercentualParticipacao { get; set; }

    }
}
