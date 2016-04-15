using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Financeiro;

namespace GIR.Sigim.Application.Filtros.Financeiro
{
    public class RelAcompanhamentoFinanceiroFiltro : BaseFiltro
    {
        [DataType(DataType.Date, ErrorMessageResourceType = typeof(Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "InformeDataValida")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Data inicial")]
        public Nullable<DateTime> DataInicial { get; set; }

        [DataType(DataType.Date, ErrorMessageResourceType = typeof(Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "InformeDataValida")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Data final")]
        public Nullable<DateTime> DataFinal { get; set; }

        public CentroCustoDTO CentroCusto { get; set; }

        public List<ClasseDTO> ListaClasse { get; set; }

        public short BaseadoPor { get; set; }

        [Display(Name = "Valor corrigido")]
        public bool EhValorCorrigido { get; set; }

        [Display(Name = "Índice")]
        public int? IndiceId { get; set; }

        [RegularExpression(@"[0-9]*$", ErrorMessageResourceType = typeof(Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "ValorDeveSerNumerico")]
        [Display(Name = "Defasagem")]
        public int? Defasagem { get; set; }

        public RelAcompanhamentoFinanceiroFiltro()
        {
            CentroCusto = new CentroCustoDTO();
            ListaClasse = new List<ClasseDTO>();
        }

    }
}
