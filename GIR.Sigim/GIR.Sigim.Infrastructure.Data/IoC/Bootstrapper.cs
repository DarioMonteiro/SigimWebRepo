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

            AdminBootstraper.Initialise();
            ContratoBootstraper.Initialise();
            EstoqueBootstraper.Initialise();
            FinanceiroBootstraper.Initialise();
            OrcamentoBootstraper.Initialise();
            OrdemCompraBootstraper.Initialise();
            SacBootstraper.Initialise();
            SigimBootstraper.Initialise();
        }
    }
}