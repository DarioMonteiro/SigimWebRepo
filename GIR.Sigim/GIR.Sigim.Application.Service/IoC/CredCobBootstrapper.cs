using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.Service.CredCob ;
using GIR.Sigim.Infrastructure.Crosscutting.IoC;
using Microsoft.Practices.Unity;

namespace GIR.Sigim.Application.Service.IoC
{
    public class CredCobBootstrapper
    {
        public static void Initialise()
        {
            Container.Current.RegisterType<ITituloCredCobAppService, TituloCredCobAppService>();
            Container.Current.RegisterType<ITituloMovimentoAppService, TituloMovimentoAppService>();
        }
    }
}
