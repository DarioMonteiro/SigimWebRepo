using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Application.DTO.Contrato
{
    public class LicitacaoDescricaoDTO : BaseDTO
    {
        public string Descricao { get; set; }

        public List<ContratoDTO> ListaContrato { get; set; }

        public LicitacaoDescricaoDTO()
        {
            this.ListaContrato = new List<ContratoDTO>(); 
        }

    }
}
