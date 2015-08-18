using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.Service.Contrato;
using GIR.Sigim.Infrastructure.Crosscutting.IoC;
using Microsoft.Practices.Unity;

namespace GIR.Sigim.Application.Service.IoC
{
    public class ContratoBootstraper
    {
        public static void Initialise()
        {
            Container.Current.RegisterType<IContratoAppService, ContratoAppService>();
            Container.Current.RegisterType<IContratoRetificacaoAppService, ContratoRetificacaoAppService>();
            Container.Current.RegisterType<IContratoRetificacaoItemAppService, ContratoRetificacaoItemAppService>();
            Container.Current.RegisterType<IContratoRetificacaoItemMedicaoAppService, ContratoRetificacaoItemMedicaoAppService>();
            Container.Current.RegisterType<IParametrosContratoAppService, ParametrosContratoAppService>();
        }
    }
}