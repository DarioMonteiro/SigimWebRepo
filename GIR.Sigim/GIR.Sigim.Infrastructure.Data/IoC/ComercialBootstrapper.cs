using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Repository.Comercial;
using GIR.Sigim.Infrastructure.Crosscutting.IoC;
using GIR.Sigim.Infrastructure.Data.Repository.Comercial;
using Microsoft.Practices.Unity;

namespace GIR.Sigim.Infrastructure.Data.IoC
{
    public class ComercialBootstrapper
    {
        public static void Initialise()
        {
            Container.Current.RegisterType<IIncorporadorRepository, IncorporadorRepository>();
            Container.Current.RegisterType<IEmpreendimentoRepository, EmpreendimentoRepository>();
            Container.Current.RegisterType<IBlocoRepository, BlocoRepository>();
            Container.Current.RegisterType<IVendaRepository, VendaRepository>();
        }
    }
}
