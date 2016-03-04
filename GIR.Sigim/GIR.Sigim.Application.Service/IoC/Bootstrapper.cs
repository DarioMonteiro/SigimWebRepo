using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Application.Service.IoC
{
    public class Bootstrapper
    {
        public static void Initialise()
        {
            GIR.Sigim.Infrastructure.Data.IoC.Bootstrapper.Initialise();
            AdminBootstraper.Initialise();
            ComercialBootstrapper.Initialise();
            ContratoBootstraper.Initialise();
            CredCobBootstraper.Initialise();
            FinanceiroBootstraper.Initialise();
            OrcamentoBootstraper.Initialise();
            OrdemCompraBootstraper.Initialise();
            SacBootstraper.Initialise();
            SigimBootstraper.Initialise();
        }
    }
}