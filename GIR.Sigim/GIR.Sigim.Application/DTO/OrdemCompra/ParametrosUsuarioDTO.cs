using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Financeiro;

namespace GIR.Sigim.Application.DTO.OrdemCompra
{
    public class ParametrosUsuarioDTO : BaseDTO
    {
        public CentroCustoDTO CentroCusto { get; set; }

        [StringLength(50, ErrorMessageResourceType = typeof(Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "LimiteMaximoCaracteresExcedido")]
        [EmailAddress(ErrorMessage = null, ErrorMessageResourceType = typeof(Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "EmailInvalido")]
        [Display(Name = "E-mail do usuário")]
        public string Email { get; set; }

        [StringLength(50, ErrorMessageResourceType = typeof(Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "LimiteMaximoCaracteresExcedido")]
        [DataType(DataType.Password)]
        [Display(Name = "Senha do e-mail do usuário")]
        public string Senha { get; set; }
    }
}