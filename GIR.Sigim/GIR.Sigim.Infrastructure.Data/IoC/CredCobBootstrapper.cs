using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Repository.CredCob;
using GIR.Sigim.Infrastructure.Crosscutting.IoC;
using GIR.Sigim.Infrastructure.Data.Repository.CredCob;
using Microsoft.Practices.Unity;

namespace GIR.Sigim.Infrastructure.Data.IoC
{
    public class CredCobBootstrapper
    {
        public static void Initialise()
        {
            Container.Current.RegisterType<ITituloCredCobRepository, TituloCredCobRepository>();
        }
    }
}
