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
            AdminBootstrapper.Initialise();
            ComercialBootstrapper.Initialise();
            ContratoBootstrapper.Initialise();
            CredCobBootstrapper.Initialise();
            FinanceiroBootstrapper.Initialise();
            OrcamentoBootstrapper.Initialise();
            OrdemCompraBootstrapper.Initialise();
            SacBootstrapper.Initialise();
            SigimBootstrapper.Initialise();
        }
    }
}