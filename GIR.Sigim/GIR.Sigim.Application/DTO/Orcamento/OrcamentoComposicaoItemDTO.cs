using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Financeiro;
using GIR.Sigim.Application.DTO.Sigim;

namespace GIR.Sigim.Application.DTO.Orcamento
{
    public class OrcamentoComposicaoItemDTO : BaseDTO
    {
        public MaterialDTO Material { get; set; }
        public ClasseDTO Classe { get; set; }
        public ComposicaoDTO Composicao { get; set; }
        public decimal? Consumo { get; set; }
        public decimal? PercentualPerda { get; set; }
        public decimal? Preco { get; set; }
        public bool? EhControlado { get; set; }
        public decimal? QuantidadeOrcada { get; set; }
        public decimal? QuantidadeRequisitada { get; set; }
    }
}