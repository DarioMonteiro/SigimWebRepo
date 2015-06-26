using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Financeiro;
using GIR.Sigim.Application.DTO.Sigim;

namespace GIR.Sigim.Application.DTO.Orcamento
{
    public class OrcamentoInsumoRequisitadoDTO : BaseDTO
    {
        public CentroCustoDTO CentroCusto { get; set; }
        public ClasseDTO Classe { get; set; }
        //public int? ComposicaoId { get; set; }
        public ComposicaoDTO Composicao { get; set; }
        public MaterialDTO Material { get; set; }
        public decimal? Quantidade { get; set; }
    }
}