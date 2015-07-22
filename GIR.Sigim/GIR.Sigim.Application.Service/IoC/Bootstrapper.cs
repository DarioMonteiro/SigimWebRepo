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
            //Trul.Data.EntityFramework.Bootstrapper.Initialise();
            GIR.Sigim.Infrastructure.Data.IoC.Bootstrapper.Initialise();
            AdminBootstraper.Initialise();
            ContratoBootstraper.Initialise();
            FinanceiroBootstraper.Initialise();
            OrcamentoBootstraper.Initialise();
            OrdemCompraBootstraper.Initialise();
            SacBootstraper.Initialise();
            SigimBootstraper.Initialise();
        }
    }
}