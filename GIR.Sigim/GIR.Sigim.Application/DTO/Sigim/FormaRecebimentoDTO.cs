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
        
        public string TipoRecebimento { get; set; }
        public string TipoRecebimentoDescricao
        {
          get { return TipoRecebimento == "0" ? "Compensação imediata" : TipoRecebimento == "1" ? "Compensação posterior" : "Sem compensação"; }
        }

        public int? NumeroDias { get; set; }

    }
}