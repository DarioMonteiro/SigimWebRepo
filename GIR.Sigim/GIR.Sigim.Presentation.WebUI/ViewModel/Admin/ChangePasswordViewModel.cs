using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GIR.Sigim.Presentation.WebUI.ViewModel.Admin
{
    public class ChangePasswordViewModel
    {
        [Required]
        [StringLength(50, ErrorMessageResourceType = typeof(Application.Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "LimiteMaximoCaracteresExcedido")]
        [DataType(DataType.Password)]
        [Display(Name = "Senha atual")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(50, ErrorMessageResourceType = typeof(Application.Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "LimiteMaximoCaracteresExcedido")]
        //[MinLength(7, ErrorMessageResourceType = typeof(Resource.ErrorMessages), ErrorMessageResourceName = "LimiteMinimoCaracteresNaoAtingido")]
        [DataType(DataType.Password)]
        [Display(Name = "Nova senha")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessageResourceType = typeof(Application.Resource.Admin.ErrorMessages), ErrorMessageResourceName = "ConfirmacaoNovaSenhaNaoConfere")]
        [Display(Name = "Confirme a nova senha")]
        public string ConfirmPassword { get; set; }
    }
}