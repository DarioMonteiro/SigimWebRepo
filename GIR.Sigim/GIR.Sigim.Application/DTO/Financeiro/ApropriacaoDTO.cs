using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Contrato;
using GIR.Sigim.Application.DTO.Financeiro;

namespace GIR.Sigim.Application.DTO.Financeiro
{
    public class ApropriacaoDTO : BaseDTO
    {
        public string CodigoClasse { get; set; }
        public ClasseDTO Classe { get; set; }
        public string CodigoCentroCusto { get; set; }
        public CentroCustoDTO CentroCusto { get; set; }
        public int? TituloPagarId { get; set; }
        public TituloPagarDTO TituloPagar { get; set; }
        public int? TituloReceberId { get; set; }
        public TituloReceberDTO TituloReceber { get; set; }
        public int? MovimentoId { get; set; }
        public MovimentoFinanceiroDTO Movimento { get; set; }
        public decimal Valor { get; set; }
        public decimal Percentual { get; set; }
    }
}