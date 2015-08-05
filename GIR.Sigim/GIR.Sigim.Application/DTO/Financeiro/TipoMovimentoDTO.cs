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
        [StringLength(2, ErrorMessageResourceType = typeof(Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "LimiteMaximoCaracteresExcedido")]
        [Display(Name = "Histórico")]
        public int? Historico { get; set; }
        
        [Required]
        [StringLength(1, ErrorMessageResourceType = typeof(Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "LimiteMaximoCaracteresExcedido")]
        [Display(Name = "Tipo")]
        public string Tipo { get; set; }

        [Required]
        [StringLength(1, ErrorMessageResourceType = typeof(Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "LimiteMaximoCaracteresExcedido")]
        [Display(Name = "Operação")]
        public string Operacao { get; set; }
    }

}
