using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Application.DTO.Contrato
{
    public class ContratoRetencaoDTO : BaseDTO
    {
        public int? ContratoId { get; set; }
        public ContratoDTO Contrato { get; set; }
        public int ContratoRetificacaoId { get; set; }
        public ContratoRetificacaoDTO ContratoRetificacao { get; set; }
        public int ContratoRetificacaoItemId { get; set; }
        public ContratoRetificacaoItemDTO ContratoRetificacaoItem { get; set; }
        public int ContratoRetificacaoItemMedicaoId { get; set; }
        public ContratoRetificacaoItemMedicaoDTO ContratoRetificacaoItemMedicao { get; set; }
        public decimal? ValorRetencao { get; set; }
        public List<ContratoRetencaoLiberadaDTO> ListaContratoRetencaoLiberada { get; set; }

        public ContratoRetencaoDTO()
        {
            this.ListaContratoRetencaoLiberada = new List<ContratoRetencaoLiberadaDTO>();
        }

    }
}
