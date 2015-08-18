using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Repository.Sac;
using GIR.Sigim.Infrastructure.Crosscutting.IoC;
using GIR.Sigim.Infrastructure.Data.Repository.Sac;
using Microsoft.Practices.Unity;

namespace GIR.Sigim.Infrastructure.Data.IoC
{
    public class SacBootstraper
    {
        public static void Initialise()
        {
            Container.Current.RegisterType<IParametrosSacRepository, ParametrosSacRepository>();
            Container.Current.RegisterType<ISetorRepository, SetorRepository>();
        }
    }
}