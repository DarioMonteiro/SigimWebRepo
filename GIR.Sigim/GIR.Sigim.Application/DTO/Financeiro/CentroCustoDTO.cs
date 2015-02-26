using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Application.DTO.Financeiro
{
    [Serializable]
    public class CentroCustoDTO : BaseDTO
    {
        [StringLength(18, ErrorMessageResourceType = typeof(Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "LimiteMaximoCaracteresExcedido")]
        [Display(Name = "Centro de Custo")]
        public string Codigo { get; set; }

        public string Descricao { get; set; }
        public string CentroContabil { get; set; }
        public int? AnoMes { get; set; }
        public int? TipoTabela { get; set; }
        public bool Ativo { get; set; }
        public CentroCustoDTO CentroCustoPai { get; set; }
        public ICollection<CentroCustoDTO> ListaFilhos { get; set; }

        public CentroCustoDTO()
        {
            this.ListaFilhos = new HashSet<CentroCustoDTO>();
        }
    }
}