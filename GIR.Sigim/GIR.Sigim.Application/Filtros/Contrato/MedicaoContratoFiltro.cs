using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; 
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Financeiro;
using GIR.Sigim.Application.DTO.Sigim; 

namespace GIR.Sigim.Application.Filtros.Contrato
{
    public class MedicaoContratoFiltro : BaseFiltro 
    {
        public CentroCustoDTO CentroCusto { get; set; }

        [RegularExpression(@"[0-9]*$", ErrorMessageResourceType = typeof(Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "ValorDeveSerNumerico")]
        [Display(Name = "Contrato")]
        public int? Id { get; set; }

        [Display(Name = "Contratante")]
        public int? ContratanteId { get; set; }
        public ClienteFornecedorDTO Contratante { get; set; }

        [Display(Name = "Contratado")]
        public int? ContratadoId { get; set; }
        public ClienteFornecedorDTO Contratado { get; set; }

    }
}
