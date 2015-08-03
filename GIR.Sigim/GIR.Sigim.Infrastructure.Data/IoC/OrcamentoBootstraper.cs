using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Repository.Orcamento;
using GIR.Sigim.Infrastructure.Crosscutting.IoC;
using GIR.Sigim.Infrastructure.Data.Repository.Orcamento;
using Microsoft.Practices.Unity;

namespace GIR.Sigim.Infrastructure.Data.IoC
{
    public class OrcamentoBootstraper
    {
        public static void Initialise()
        {
            Container.Current.RegisterType<IOrcamentoRepository, OrcamentoRepository>();
            Container.Current.RegisterType<IParametrosOrcamentoRepository, ParametrosOrcamentoRepository>();
        }
    }
}