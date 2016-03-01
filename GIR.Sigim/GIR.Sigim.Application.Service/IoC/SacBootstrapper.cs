using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.Service.Sac;
using GIR.Sigim.Infrastructure.Crosscutting.IoC;
using Microsoft.Practices.Unity;

namespace GIR.Sigim.Application.Service.IoC
{
    public class SacBootstrapper
    {
        public static void Initialise()
        {
            Container.Current.RegisterType<IParametrosSacAppService, ParametrosSacAppService>();
            Container.Current.RegisterType<ISetorAppService, SetorAppService>();
        }
    }
}