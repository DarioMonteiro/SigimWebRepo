using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Repository.Estoque;
using GIR.Sigim.Infrastructure.Crosscutting.IoC;
using GIR.Sigim.Infrastructure.Data.Repository.Estoque;
using Microsoft.Practices.Unity;

namespace GIR.Sigim.Infrastructure.Data.IoC
{
    public class EstoqueBootstrapper
    {
        public static void Initialise()
        {
            Container.Current.RegisterType<IEstoqueRepository, EstoqueRepository>();
        }
    }
}