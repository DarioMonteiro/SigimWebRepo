using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Repository.OrdemCompra;
using GIR.Sigim.Infrastructure.Crosscutting.IoC;
using GIR.Sigim.Infrastructure.Data.Repository.OrdemCompra;
using Microsoft.Practices.Unity;

namespace GIR.Sigim.Infrastructure.Data.IoC
{
    public class OrdemCompraBootstraper
    {
        public static void Initialise()
        {
            Container.Current.RegisterType<IEntradaMaterialRepository, EntradaMaterialRepository>();
            Container.Current.RegisterType<IParametrosOrdemCompraRepository, ParametrosOrdemCompraRepository>();
            Container.Current.RegisterType<IParametrosUsuarioRepository, ParametrosUsuarioRepository>();
            Container.Current.RegisterType<IPreRequisicaoMaterialRepository, PreRequisicaoMaterialRepository>();
            Container.Current.RegisterType<IOrdemCompraRepository, OrdemCompraRepository>();
            Container.Current.RegisterType<IOrdemCompraItemRepository, OrdemCompraItemRepository>();
            Container.Current.RegisterType<IRequisicaoMaterialRepository, RequisicaoMaterialRepository>();
        }
    }
}