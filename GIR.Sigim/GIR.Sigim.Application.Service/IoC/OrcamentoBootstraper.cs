using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.Service.Orcamento;
using GIR.Sigim.Infrastructure.Crosscutting.IoC;
using Microsoft.Practices.Unity;

namespace GIR.Sigim.Application.Service.IoC
{
    public class OrcamentoBootstraper
    {
        public static void Initialise()
        {
            Container.Current.RegisterType<IOrcamentoAppService, OrcamentoAppService>();
            Container.Current.RegisterType<IParametrosOrcamentoAppService, ParametrosOrcamentoAppService>();
        }
    }
}