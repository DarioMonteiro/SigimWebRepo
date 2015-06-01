using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Application.DTO.Financeiro
{
    public class CaixaDTO : BaseDTO
    {
        [Required]
        [StringLength(50, ErrorMessageResourceType = typeof(Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "LimiteMaximoCaracteresExcedido")]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        [Display(Name = "Situacao")]
        public string Situacao { get; set; }
        public bool Inativo
        {
            get { return Situacao == "I"; }
            set { Situacao = value ? "I" : "A"; }
        }

        [StringLength(20, ErrorMessageResourceType = typeof(Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "LimiteMaximoCaracteresExcedido")]
        [Display(Name = "CentroContabil")]
        public string CentroContabil { get; set; }

        public CentroCustoDTO CentroCusto { get; set; }
        public string CentroCustoDescricao
        {
            get { return this.CentroCusto.Codigo + " - " + this.CentroCusto.Descricao; }
        }
         
        public CaixaDTO()
        {            
            this.CentroCusto = new CentroCustoDTO();            
        }              

    }
}
