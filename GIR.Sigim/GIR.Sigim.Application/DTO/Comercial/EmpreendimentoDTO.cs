using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Application.DTO.Comercial
{
    public class EmpreendimentoDTO : BaseDTO
    {
        public string Nome { get; set; }
        public int? EnderecoId { get; set; }
        public int IncorporadorId { get; set; }
        public IncorporadorDTO Incorporador { get; set; }
        
        public List<BlocoDTO> ListaBloco { get; set; }
        public List<UnidadeDTO> ListaUnidade { get; set; }

        public EmpreendimentoDTO()
        {
            this.ListaBloco = new List<BlocoDTO>();
            this.ListaUnidade = new List<UnidadeDTO>();
        }

    }
}
