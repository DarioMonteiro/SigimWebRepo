using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Repository.Admin;
using GIR.Sigim.Infrastructure.Crosscutting.IoC;
using GIR.Sigim.Infrastructure.Data.Repository.Admin;
using Microsoft.Practices.Unity;

namespace GIR.Sigim.Infrastructure.Data.IoC
{
    public class AdminBootstraper
    {
        public static void Initialise()
        {
            Container.Current.RegisterType<IModuloRepository, ModuloRepository>();
            Container.Current.RegisterType<IPerfilFuncionalidadeRepository, PerfilFuncionalidadeRepository>();
            Container.Current.RegisterType<IPerfilRepository, PerfilRepository>();
            Container.Current.RegisterType<IUsuarioRepository, UsuarioRepository>();
        }
    }
}