using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Application.DTO.Comercial
{
    public class TipoParticipanteDTO : BaseDTO
    {
        public string Descricao { get; set; }
        public bool? Automatico { get; set; }

        public List<VendaParticipanteDTO> ListaVendaParticipante { get; set; }

        public TipoParticipanteDTO()
        {
            this.ListaVendaParticipante = new List<VendaParticipanteDTO>();
        }

    }
}
