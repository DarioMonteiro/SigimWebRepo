using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Application.DTO.Sigim
{
    public class FormaRecebimentoDTO : BaseDTO
    {
        [Required]
        [StringLength(50, ErrorMessageResourceType = typeof(Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "LimiteMaximoCaracteresExcedido")]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        public bool Automatico { get; set; }

        [Display(Name = "Tipo")]
        public string TipoRecebimento { get; set; }
        public string TipoRecebimentoDescricao
        {
          get { return TipoRecebimento == "0" ? "Compensação Imediata" : TipoRecebimento == "1" ? "Compensação Posterior" : "Sem Compensação"; }
        }

        [RegularExpression(@"[0-9]*$", ErrorMessageResourceType = typeof(Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "ValorDeveSerNumerico")]
        [Display(Name = "No dias")]
        public int? NumeroDias { get; set; }

    }
}