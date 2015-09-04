using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Application.DTO.Contrato
{
    public class ContratoRetificacaoItemCronogramaDTO : BaseDTO
    {
        public int ContratoId { get; set; }
        //public ContratoDTO Contrato { get; set; }
        public int ContratoRetificacaoId { get; set; }
        //public ContratoRetificacaoDTO ContratoRetificacao { get; set; }
        public int ContratoRetificacaoItemId { get; set; }
        public int Sequencial { get; set; }
        public string Descricao { get; set; }
        public DateTime DataInicial { get; set; }
        public DateTime DataFinal { get; set; }
        public DateTime DataVencimento { get; set; }
        public decimal Quantidade { get; set; }
        public decimal PercentualExecucao { get; set; }
        public decimal Valor { get; set; }
    }
}
