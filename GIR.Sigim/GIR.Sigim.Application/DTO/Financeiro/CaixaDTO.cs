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
        public string DescricaoSituacao
        {
            get { return Situacao == "A" ? "Ativa" : "Inativa"; }
        }
        public bool Ativa
        {
            get { return Situacao == "A"; }
            set { Situacao = value ? "A" : "I"; }
        }

        [StringLength(20, ErrorMessageResourceType = typeof(Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "LimiteMaximoCaracteresExcedido")]
        [Display(Name = "Centro contábil")]
        public string CentroContabil { get; set; }

        public CentroCustoDTO CentroCusto { get; set; }
        public string CentroCustoDescricao
        {
            get { return CentroCusto != null ? this.CentroCusto.CentroCustoDescricao : string.Empty; }
        }
         
        public CaixaDTO()
        {            
            this.CentroCusto = new CentroCustoDTO();            
        }              

    }
}
