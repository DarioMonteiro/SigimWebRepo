using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Application.DTO.Financeiro
{
    public class TipoMovimentoDTO : BaseDTO
    {

        [Required]
        [StringLength(50, ErrorMessageResourceType = typeof(Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "LimiteMaximoCaracteresExcedido")]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        [Required]
        [StringLength(1, ErrorMessageResourceType = typeof(Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "LimiteMaximoCaracteresExcedido")]
        [Display(Name = "Operação")]
        public string Operacao { get; set; }
        public string OperacaoDescricao
        {
            get { return Operacao == "D" ? "Débito" : Operacao == "C" ? "Crédito" : ""; }
        }


        [Display(Name = "Automático")]
        public bool? Automatico { get; set; }

        [Display(Name = "Histórico Contábil")]
        public int? HistoricoContabilId { get; set; }
        public HistoricoContabilDTO HistoricoContabil { get; set; }

        public string HistoricoContabilDescricao
        {
            get { return HistoricoContabil != null ? HistoricoContabil.Descricao : ""; }
        }

        
        [Required]
        [StringLength(1, ErrorMessageResourceType = typeof(Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "LimiteMaximoCaracteresExcedido")]
        [Display(Name = "Tipo")]
        public string Tipo { get; set; }

        public string TipoDescricao
        {
            get { return Tipo == "B" ? "Bancário" : Tipo == "C" ? "Caixa" : ""; }
        }


        public Byte? FormaPagamento { get; set; }
    }

}
