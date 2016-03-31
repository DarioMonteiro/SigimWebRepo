using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GIR.Sigim.Application.Filtros;
using GIR.Sigim.Presentation.WebUI.ViewModel;
using System.ComponentModel.DataAnnotations;


namespace GIR.Sigim.Presentation.WebUI.Areas.Financeiro.ViewModel
{
    public class ListaViewModelRelContasPagarTitulo : PaginationParameters
    {
        public SelectList PageSizeList { get; set; }
        public Pagination Pagination { get; set; }
        public object Records { get; set; }
        [Display(Name = "Valor título")]
        public decimal TotalValorTitulo { get; set; }
        [Display(Name = "Valor liquido")]
        public decimal TotalValorLiquido { get; set; }
        [Display(Name = "Valor apropriado")]
        public decimal TotalValorApropriacao { get; set; }
        public string TotalizadoPorDescricao { get; set; }
    }
}