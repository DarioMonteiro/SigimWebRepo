using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Application.Filtros
{
    public class PaginationParameters
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string OrderBy { get; set; }
        public bool Ascending { get; set; }
        public string UniqueIdentifier { get; set; }

        public PaginationParameters()
        {
            PageIndex = 0;
            PageSize = 10;
            Ascending = true;
        }
    }
}