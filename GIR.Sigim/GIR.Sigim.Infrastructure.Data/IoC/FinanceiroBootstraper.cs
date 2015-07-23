﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Repository.Financeiro;
using GIR.Sigim.Infrastructure.Crosscutting.IoC;
using GIR.Sigim.Infrastructure.Data.Repository.Financeiro;
using Microsoft.Practices.Unity;

namespace GIR.Sigim.Infrastructure.Data.IoC
{
    public class FinanceiroBootstraper
    {
        public static void Initialise()
        {
            Container.Current.RegisterType<ICaixaRepository, CaixaRepository>();
            Container.Current.RegisterType<ICentroCustoRepository, CentroCustoRepository>();
            Container.Current.RegisterType<IClasseRepository, ClasseRepository>();
            Container.Current.RegisterType<IImpostoFinanceiroRepository, ImpostoFinanceiroRepository>();
            Container.Current.RegisterType<IMotivoCancelamentoRepository, MotivoCancelamentoRepository>();
            Container.Current.RegisterType<IParametrosFinanceiroRepository, ParametrosFinanceiroRepository>();
            Container.Current.RegisterType<IParametrosUsuarioFinanceiroRepository, ParametrosUsuarioFinanceiroRepository>();
            Container.Current.RegisterType<IRateioAutomaticoRepository, RateioAutomaticoRepository>();
            Container.Current.RegisterType<ITaxaAdministracaoRepository, TaxaAdministracaoRepository>();
            Container.Current.RegisterType<ITipoCompromissoRepository, TipoCompromissoRepository>();
            Container.Current.RegisterType<ITipoDocumentoRepository, TipoDocumentoRepository>();
            Container.Current.RegisterType<ITipoRateioRepository, TipoRateioRepository>();
            Container.Current.RegisterType<ITituloPagarRepository, TituloPagarRepository>();
        }
    }
}