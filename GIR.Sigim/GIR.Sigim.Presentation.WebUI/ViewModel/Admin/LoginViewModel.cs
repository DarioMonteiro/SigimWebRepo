using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GIR.Sigim.Presentation.WebUI.ViewModel.Admin
{
    public class LoginViewModel
    {
        [Required]
        [StringLength(50, ErrorMessageResourceType = typeof(Application.Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "LimiteMaximoCaracteresExcedido")]
        [Display(Name = "Usuário")]
        public string UserName { get; set; }

        [Required]
        [StringLength(50, ErrorMessageResourceType = typeof(Application.Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "LimiteMaximoCaracteresExcedido")]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Password { get; set; }

        [Display(Name = "Mantenha-me conectado")]
        public bool RememberMe { get; set; }
    }
}