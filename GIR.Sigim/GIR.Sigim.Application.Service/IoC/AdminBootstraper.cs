using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.Service.Admin;
using GIR.Sigim.Infrastructure.Crosscutting.IoC;
using Microsoft.Practices.Unity;

namespace GIR.Sigim.Application.Service.IoC
{
    public class AdminBootstraper
    {
        public static void Initialise()
        {
            Container.Current.RegisterType<IFuncionalidadeAppService, FuncionalidadeAppService>();
            Container.Current.RegisterType<IModuloAppService, ModuloAppService>();
            Container.Current.RegisterType<IPerfilAppService, PerfilAppService>();
            Container.Current.RegisterType<IUsuarioAppService, UsuarioAppService>();
        }
    }
}