using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Application.Filtros
{
    public class BaseFiltro
    {
        public PaginationParameters PaginationParameters { get; set; }

        public BaseFiltro()
        {
            PaginationParameters = new PaginationParameters();
        }
    }
}