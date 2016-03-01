using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.Service.OrdemCompra;
using GIR.Sigim.Infrastructure.Crosscutting.IoC;
using Microsoft.Practices.Unity;

namespace GIR.Sigim.Application.Service.IoC
{
    public class OrdemCompraBootstrapper
    {
        public static void Initialise()
        {
            Container.Current.RegisterType<IEntradaMaterialAppService, EntradaMaterialAppService>();
            Container.Current.RegisterType<IOrdemCompraAppService, OrdemCompraAppService>();
            Container.Current.RegisterType<IOrdemCompraItemAppService, OrdemCompraItemAppService>();
            Container.Current.RegisterType<IParametrosOrdemCompraAppService, ParametrosOrdemCompraAppService>();
            Container.Current.RegisterType<IParametrosUsuarioAppService, ParametrosUsuarioAppService>();
            Container.Current.RegisterType<IPreRequisicaoMaterialAppService, PreRequisicaoMaterialAppService>();
            Container.Current.RegisterType<IRequisicaoMaterialAppService, RequisicaoMaterialAppService>();
        }
    }
}