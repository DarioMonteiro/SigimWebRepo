using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.Service.Financeiro;
using GIR.Sigim.Infrastructure.Crosscutting.IoC;
using Microsoft.Practices.Unity;

namespace GIR.Sigim.Application.Service.IoC
{
    public class FinanceiroBootstrapper
    {
        public static void Initialise()
        {
            Container.Current.RegisterType<IApropriacaoAppService, ApropriacaoAppService>();
            Container.Current.RegisterType<ICaixaAppService, CaixaAppService>();
            Container.Current.RegisterType<ICentroCustoAppService, CentroCustoAppService>();
            Container.Current.RegisterType<IClasseAppService, ClasseAppService>();
            Container.Current.RegisterType<IFormaPagamentoAppService, FormaPagamentoAppService>();
            Container.Current.RegisterType<IHistoricoContabilAppService, HistoricoContabilAppService>();
            Container.Current.RegisterType<IImpostoFinanceiroAppService, ImpostoFinanceiroAppService>();
            Container.Current.RegisterType<IMotivoCancelamentoAppService, MotivoCancelamentoAppService>();
            Container.Current.RegisterType<IParametrosFinanceiroAppService, ParametrosFinanceiroAppService>();
            Container.Current.RegisterType<IParametrosUsuarioFinanceiroAppService, ParametrosUsuarioFinanceiroAppService>();
            Container.Current.RegisterType<IRateioAutomaticoAppService, RateioAutomaticoAppService>();
            Container.Current.RegisterType<ITabelaBasicaAppService, TabelaBasicaAppService>();
            Container.Current.RegisterType<ITaxaAdministracaoAppService, TaxaAdministracaoAppService>();
            Container.Current.RegisterType<ITipoCompromissoAppService, TipoCompromissoAppService>();
            Container.Current.RegisterType<ITipoDocumentoAppService, TipoDocumentoAppService>();
            Container.Current.RegisterType<ITipoMovimentoAppService, TipoMovimentoAppService>();
            Container.Current.RegisterType<ITipoRateioAppService, TipoRateioAppService>();
            Container.Current.RegisterType<ITituloPagarAppService, TituloPagarAppService>();
        }
    }
}