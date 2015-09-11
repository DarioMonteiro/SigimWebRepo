using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;  
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Application.DTO.Contrato
{
    public class ResumoLiberacaoDTO
    {
        [Display(Name = "Valor Contratado")]
        public decimal ValorContratado { get; set; }
        [Display(Name = "Valor Retido Contrato")]
        public decimal ValorRetidoContrato { get; set; }
        [Display(Name = "Valor Provisionado")]
        public decimal ValorProvisionado { get; set; }
        [Display(Name = "Valor Medido")]
        public decimal ValorMedido { get; set; }
        [Display(Name = "Diferença")]
        public decimal Diferenca { get; set; }
        [Display(Name = "Aguardando Liberação")]
        public decimal AguardandoLiberacao { get; set; }
        [Display(Name = "Retido")]
        public decimal Retido { get; set; }
        [Display(Name = "Liberado")]
        public decimal Liberado { get; set; }
        [Display(Name = "Retenção Liberada")]
        public decimal RetencaoLiberada { get; set; }
        [Display(Name = "Saldo")]
        public decimal Saldo { get; set; }
        [Display(Name = "Valor")]
        public decimal TotalValorSelecionado { get; set; }
        [Display(Name = "Retido")]
        public decimal TotalRetidoSelecionado { get; set; }
    }
}
