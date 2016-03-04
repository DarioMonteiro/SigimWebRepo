using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Application.DTO.Comercial
{
    public class IncorporadorDTO : BaseDTO
    {
        public string RazaoSocial { get; set; }
        public string TipoPessoa { get; set; }
        public string Cnpj { get; set; }
        public string InscricaoMunicipal { get; set; }
        public string InscricaoEstadual { get; set; }
        public string CodigoSUFRAMA { get; set; }

        public List<EmpreendimentoDTO> ListaEmpreendimento { get; set; }

        public IncorporadorDTO()
        {
            this.ListaEmpreendimento = new List<EmpreendimentoDTO>();
        }

    }
}
