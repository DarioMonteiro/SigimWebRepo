using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Application.DTO.Financeiro
{
    public class TipoCompromissoDTO : BaseDTO
    {
        [StringLength(50, ErrorMessageResourceType = typeof(Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "LimiteMaximoCaracteresExcedido")]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        [Display(Name = "Gera título")]
        public bool GeraTitulo { get; set; }

        [Display(Name = "Tipo a pagar")]
        public bool TipoPagar { get; set; }

        [Display(Name = "Tipo a receber")]
        public bool TipoReceber { get; set; }
    }
}