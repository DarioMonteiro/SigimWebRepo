using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Sigim;

namespace GIR.Sigim.Application.DTO.Financeiro
{
    public class RateioAutomaticoDTO : BaseDTO 
    {
        [Display(Name = "Tipo de rateio")]
        public int TipoRateioId { get; set; }
        public TipoRateioDTO TipoRateio { get; set; }

        [Display(Name = "Classe")]
        public string  ClasseId { get; set; }
        public ClasseDTO Classe { get; set; }

        [Display(Name = "Centro de custo")]
        public string CentroCustoId { get; set; }
        public CentroCustoDTO CentroCusto { get; set; }

        [RegularExpression(@"^\d+(.\d+){0,1}$", ErrorMessageResourceType = typeof(Application.Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "ValorDeveSerNumerico")]
        [Display(Name = "Percentual")]
        public decimal Percentual { get; set; }

    }
}
