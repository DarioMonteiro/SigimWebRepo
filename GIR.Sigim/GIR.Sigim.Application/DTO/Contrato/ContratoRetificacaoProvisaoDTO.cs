using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Financeiro;

namespace GIR.Sigim.Application.DTO.Contrato
{
    public class ContratoRetificacaoProvisaoDTO : BaseDTO
    {
        public int ContratoId { get; set; }
        public ContratoDTO Contrato { get; set; }
        public int ContratoRetificaocaoId { get; set; }
        public ContratoRetificacaoDTO ContratoRetificacao { get; set; }
        public int? ContratoRetificacaoItemId { get; set; }
        public ContratoRetificacaoItemDTO ContratoRetificacaoItem { get; set; }
        public int? SequencialItem { get; set; }
        public int? ContratoRetificacaoItemCronogramaId { get; set; }
        public ContratoRetificacaoItemCronogramaDTO ContratoRetificacaoItemCronograma { get; set; }
        public int? SequencialCronograma { get; set; }
        public int? TituloPagarId { get; set; }
        public TituloPagarDTO TituloPagar { get; set; }
        public int? TituloReceberId { get; set; }
        public TituloReceberDTO TituloReceber { get; set; }
        public decimal Valor { get; set; }
        public decimal Quantidade { get; set; }

        //*****************
        public decimal ValorTotalMedido { get; set; }
        //*****************

        public TotalizadoresMedicaoDTO Totalizadores { get; set; }

        public ContratoRetificacaoProvisaoDTO()
        {
            this.Totalizadores = new TotalizadoresMedicaoDTO();
        }

    }
}
