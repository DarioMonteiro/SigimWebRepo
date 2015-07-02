using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Sigim;

namespace GIR.Sigim.Application.DTO.Financeiro
{
    public class TaxaAdministracaoDTO : BaseDTO 
    {
        [Display(Name = "Centro de custo")]
        public CentroCustoDTO CentroCusto { get; set; }

        [Display(Name = "Cliente")]
        public int ClienteId { get; set; }
        public ClienteFornecedorDTO Cliente { get; set; }

        [Display(Name = "Classe")]
        public ClasseDTO Classe { get; set; }

        [RegularExpression(@"^\d+(.\d+){0,1}$", ErrorMessageResourceType = typeof(Application.Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "ValorDeveSerNumerico")]
        [Display(Name = "Percentual")]
        public decimal Percentual { get; set; }

    }
}
