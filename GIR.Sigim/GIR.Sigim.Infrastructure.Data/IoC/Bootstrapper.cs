using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Infrastructure.Crosscutting.IoC;
using Microsoft.Practices.Unity;

namespace GIR.Sigim.Infrastructure.Data.IoC
{
    public class Bootstrapper
    {
        public static void Initialise()
        {
            Container.Current.RegisterType(typeof(UnitOfWork), new PerResolveLifetimeManager());

            AdminBootstrapper.Initialise();
            ContratoBootstrapper.Initialise();
            CredCobBootstrapper.Initialise();
            EstoqueBootstrapper.Initialise();
            FinanceiroBootstrapper.Initialise();
            OrcamentoBootstrapper.Initialise();
            OrdemCompraBootstrapper.Initialise();
            SacBootstrapper.Initialise();
            SigimBootstrapper.Initialise();
        }
    }
}