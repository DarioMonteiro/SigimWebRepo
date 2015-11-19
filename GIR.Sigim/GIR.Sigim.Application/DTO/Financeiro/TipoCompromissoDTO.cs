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
        [Required]
        [StringLength(50, ErrorMessageResourceType = typeof(Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "LimiteMaximoCaracteresExcedido")]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        [Display(Name = "Gera título")]
        public bool GeraTitulo { get; set; }

        public string GeraTituloDescricao
        {
            get { return GeraTitulo == true ? "Sim" : "Não"; }
        }

        [Display(Name = "Tipo a pagar")]
        public bool TipoPagar { get; set; }

        public string TipoPagarDescricao
        {
            get { return TipoPagar == true ? "Sim" : "Não"; }
        }

        [Display(Name = "Tipo a receber")]
        public bool TipoReceber { get; set; }

        public string TipoReceberDescricao
        {
            get { return TipoReceber == true ? "Sim" : "Não"; }
        }
    }
}