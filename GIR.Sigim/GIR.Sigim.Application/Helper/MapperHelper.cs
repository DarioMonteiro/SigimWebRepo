using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Application.Helper
{
    public class MapperHelper
    {
        public static void Initialise()
        {
            AdminMapperHelper.Initialise();
            ComercialMapperHelper.Initialise();
            ContratoMapperHelper.Initialise();
            EstoqueMapperHelper.Initialise();
            FinanceiroMapperHelper.Initialise();
            OrcamentoMapperHelper.Initialise();
            OrdemCompraMapperHelper.Initialise();
            SigimMapperHelper.Initialise();
            SacMapperHelper.Initialise();
        }
    }
}