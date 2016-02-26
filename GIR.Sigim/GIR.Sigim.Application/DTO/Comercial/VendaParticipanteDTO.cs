using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Sigim;

namespace GIR.Sigim.Application.DTO.Comercial
{
    public class VendaParticipanteDTO : BaseDTO
    {
        public int ContratoId { get; set; }
        public ContratoComercialDTO Contrato { get; set; }
        public int ClienteId { get; set; }
        public ClienteFornecedorDTO Cliente { get; set; }
        public int? TipoParticipanteId { get; set; }
        public TipoParticipanteDTO TipoParticipante { get; set; }
        public Decimal PercentualParticipacao { get; set; }

    }
}
