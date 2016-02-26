using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Financeiro;

namespace GIR.Sigim.Application.DTO.Comercial
{
    public class BlocoDTO : BaseDTO
    {
        public int EmpreendimentoId { get; set; }
        public EmpreendimentoDTO Empreendimento { get; set; }
        public string Nome { get; set; }
        public string CodigoCentroCusto { get; set; }
        public CentroCustoDTO CentroCusto { get; set; }

        public List<UnidadeDTO> ListaUnidade { get; set; }

        public BlocoDTO()
        {
            this.ListaUnidade = new List<UnidadeDTO>();
        }

    }
}
