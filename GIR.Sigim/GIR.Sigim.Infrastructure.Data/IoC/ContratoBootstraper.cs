using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Repository.Contrato;
using GIR.Sigim.Infrastructure.Crosscutting.IoC;
using GIR.Sigim.Infrastructure.Data.Repository.Contrato;
using Microsoft.Practices.Unity;

namespace GIR.Sigim.Infrastructure.Data.IoC
{
    public class ContratoBootstraper
    {
        public static void Initialise()
        {
            Container.Current.RegisterType<IContratoRepository, ContratoRepository>();
            Container.Current.RegisterType<IContratoRetificacaoItemMedicaoRepository, ContratoRetificacaoItemMedicaoRepository>();
            Container.Current.RegisterType<IContratoRetificacaoProvisaoRepository, ContratoRetificacaoProvisaoRepository>();
            Container.Current.RegisterType<IParametrosContratoRepository, ParametrosContratoRepository>();
        }
    }
}