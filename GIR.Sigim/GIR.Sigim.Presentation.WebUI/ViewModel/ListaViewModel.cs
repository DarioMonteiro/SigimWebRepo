using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GIR.Sigim.Application.Filtros;

namespace GIR.Sigim.Presentation.WebUI.ViewModel
{
    public class ListaViewModel : PaginationParameters
    {
        public SelectList PageSizeList { get; set; }
        public Pagination Pagination { get; set; }
        public object Records { get; set; }
    }
}