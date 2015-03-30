using System;
using System.Reflection;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Application.DTO.Financeiro
{
    [Flags] 
    public enum TipoImpressoraEnum
    {
        [Display(Name = "DP 20 PLUS (BEMATECH)")] 
        Bematech,
        [Display(Name = "PERTOCHECK")]
        Pertocheck
    }

  
    public class ParametrosUsuarioFinanceiroDTO : BaseDTO
    {

        //[StringLength(50, ErrorMessageResourceType = typeof(Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "LimiteMaximoCaracteresExcedido")]
        //[Display(Name = "Tipo de Impressora")]
        //public String TipoImpressora { get; set; }

        [Display(Name = "Tipo de Impressora")]
        public TipoImpressoraEnum TipoImpressoraEscolhida { get; set; }

        [StringLength(50, ErrorMessageResourceType = typeof(Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "LimiteMaximoCaracteresExcedido")]
        [Display(Name = "Porta serial")]
        public string PortaSerial { get; set; }

    }
}
