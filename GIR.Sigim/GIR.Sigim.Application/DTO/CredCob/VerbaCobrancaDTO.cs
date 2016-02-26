using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Financeiro;

namespace GIR.Sigim.Application.DTO.CredCob
{
    public class VerbaCobrancaDTO : BaseDTO
    {
        public string Descricao { get; set; }
        public string CodigoClasse { get; set; }
        public ClasseDTO Classe { get; set; }
        public bool? Automatico { get; set; }

        public List<TituloCredCobDTO> ListaTituloCredCob { get; set; }

        public VerbaCobrancaDTO()
        {
            this.ListaTituloCredCob = new List<TituloCredCobDTO>();
        }
    }
}
