using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations; 
using GIR.Sigim.Domain.Entity.Sigim;

namespace GIR.Sigim.Application.DTO.Sigim
{
    public class UnidadeMedidaDTO : BaseDTO
    {
        [Required]
        [StringLength(6, ErrorMessageResourceType = typeof(Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "LimiteMaximoCaracteresExcedido")]
        [Display(Name = "Sigla")]
        public string Sigla { get; set; }

        [Required]
        [StringLength(50, ErrorMessageResourceType = typeof(Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "LimiteMaximoCaracteresExcedido")]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }       
    }
}