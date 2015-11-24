using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Application.DTO.Sigim
{
    public class UnidadeFederacaoDTO : BaseDTO
    {
        [StringLength(2, ErrorMessageResourceType = typeof(Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "LimiteMaximoCaracteresExcedido")]
        [Display(Name = "Sigla")]
        public string Sigla { get; set; }

        [StringLength(50, ErrorMessageResourceType = typeof(Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "LimiteMaximoCaracteresExcedido")]
        [Display(Name = "Unidade de federação")]
        public string NomeUnidadeFederacao { get; set; }

        [Display(Name = "Código Ibge")]
        public int? CodigoIBGE { get; set; }

    }
}
