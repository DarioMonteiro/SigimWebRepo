using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.Service.Comercial ;
using GIR.Sigim.Infrastructure.Crosscutting.IoC;
using Microsoft.Practices.Unity;

namespace GIR.Sigim.Application.Service.IoC
{
    public class ComercialBootstrapper
    {
        public static void Initialise()
        {
            Container.Current.RegisterType<IIncorporadorAppService, IncorporadorAppService>();
            Container.Current.RegisterType<IEmpreendimentoAppService, EmpreendimentoAppService>();
            Container.Current.RegisterType<IBlocoAppService, BlocoAppService>();
            Container.Current.RegisterType<IVendaAppService, VendaAppService>();

        }
    }
}
