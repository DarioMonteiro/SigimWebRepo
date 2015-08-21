using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Financeiro;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Application.DTO.OrdemCompra;


namespace GIR.Sigim.Application.Filtros.OrdemCompras
{
    public class RelOcItensOrdemCompraFiltro : BaseFiltro
    {
        [DataType(DataType.Date, ErrorMessageResourceType = typeof(Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "InformeDataValida")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Data inicial")]
        public Nullable<DateTime> DataInicial { get; set; }

        [DataType(DataType.Date, ErrorMessageResourceType = typeof(Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "InformeDataValida")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Data final")]
        public Nullable<DateTime> DataFinal { get; set; }

        //[RegularExpression(@"[0-9]*$", ErrorMessageResourceType = typeof(Resource.Sigim.ErrorMessages), ErrorMessageResourceName = "ValorDeveSerNumerico")]
        //[Display(Name = "Ordem Compra")]
        //public int? OrdemCompraId { get; set; }
        public OrdemCompraDTO OrdemCompra { get; set; }

        public CentroCustoDTO CentroCusto { get; set; }

        public ClasseDTO Classe { get; set; }

        public MaterialClasseInsumoDTO ClasseInsumo { get; set; }

        public ClienteFornecedorDTO ClienteFornecedor { get; set; }

        public MaterialDTO Material { get; set; }

        [Display(Name = "Liberadas")]
        public bool EhLiberada { get; set; }

        [Display(Name = "Fechadas")]
        public bool EhFechada { get; set; }

        [Display(Name = "Pendentes")]
        public bool EhPendente { get; set; }

        [Display(Name = "Exibir somente com saldo")]
        public bool EhExibirSomentecomSaldo { get; set; }

    }
}
