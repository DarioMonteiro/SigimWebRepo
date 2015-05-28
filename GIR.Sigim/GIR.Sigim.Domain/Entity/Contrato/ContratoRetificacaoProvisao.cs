using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Financeiro;

namespace GIR.Sigim.Domain.Entity.Contrato
{
    public class ContratoRetificacaoProvisao : BaseEntity
    {
        public int ContratoId { get; set; }
        public Contrato Contrato { get; set; }
        public int ContratoRetificacaoId { get; set; }
        public ContratoRetificacao ContratoRetificacao { get; set; }
        public int? ContratoRetificacaoItemId { get; set; }
        public ContratoRetificacaoItem ContratoRetificacaoItem { get; set; }
        public int? SequencialItem { get; set; }
        public int? ContratoRetificacaoItemCronogramaId { get; set; }
        public ContratoRetificacaoItemCronograma ContratoRetificacaoItemCronograma { get; set; }
        public int? SequencialCronograma { get; set; }
        public int? TituloPagarId { get; set; }
        public TituloPagar TituloPagar { get; set; }
        public int? TituloReceberId { get; set; }
        public TituloReceber TituloReceber { get; set; }
        public decimal Valor { get; set; }
        public decimal Quantidade { get; set; }

        public decimal ValorTotalMedido { get; set; }
        public decimal QuantidadeTotalMedida { get; set; }
        public decimal ValorTotalLiberado { get; set; }
        public decimal QuantidadeTotalLiberada { get; set; }

    }
}
