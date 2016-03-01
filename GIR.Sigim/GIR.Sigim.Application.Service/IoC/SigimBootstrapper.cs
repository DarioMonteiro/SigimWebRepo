using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.Service.Sigim;
using GIR.Sigim.Infrastructure.Crosscutting.IoC;
using Microsoft.Practices.Unity;

namespace GIR.Sigim.Application.Service.IoC
{
    public class SigimBootstrapper
    {
        public static void Initialise()
        {
            Container.Current.RegisterType<IAgenciaAppService, AgenciaAppService>();
            Container.Current.RegisterType<IAssuntoContatoAppService, AssuntoContatoAppService>();
            Container.Current.RegisterType<IBancoAppService, BancoAppService>();
            Container.Current.RegisterType<IBancoLayoutAppService, BancoLayoutAppService>();
            Container.Current.RegisterType<IBloqueioContabilAppService, BloqueioContabilAppService>();
            Container.Current.RegisterType<ICifFobAppService, CifFobAppService>();
            Container.Current.RegisterType<IClienteFornecedorAppService, ClienteFornecedorAppService>();
            Container.Current.RegisterType<ICodigoContribuicaoAppService, CodigoContribuicaoAppService>();
            Container.Current.RegisterType<IComplementoCSTAppService, ComplementoCSTAppService>();
            Container.Current.RegisterType<IComplementoNaturezaOperacaoAppService, ComplementoNaturezaOperacaoAppService>();
            Container.Current.RegisterType<IContaCorrenteAppService, ContaCorrenteAppService>();
            Container.Current.RegisterType<ICSTAppService, CSTAppService>();
            Container.Current.RegisterType<IEstadoCivilAppService, EstadoCivilAppService>();
            Container.Current.RegisterType<IFonteNegocioAppService, FonteNegocioAppService>();
            Container.Current.RegisterType<IFormaRecebimentoAppService, FormaRecebimentoAppService>();
            Container.Current.RegisterType<IGrupoAppService, GrupoAppService>();
            Container.Current.RegisterType<IInteresseBairroAppService, InteresseBairroAppService>();
            Container.Current.RegisterType<ILogOperacaoAppService, LogOperacaoAppService>();
            Container.Current.RegisterType<IMaterialAppService, MaterialAppService>();
            Container.Current.RegisterType<IMaterialClasseInsumoAppService, MaterialClasseInsumoAppService>();
            Container.Current.RegisterType<INacionalidadeAppService, NacionalidadeAppService>();
            Container.Current.RegisterType<INaturezaOperacaoAppService, NaturezaOperacaoAppService>();
            Container.Current.RegisterType<INaturezaReceitaAppService, NaturezaReceitaAppService>();
            Container.Current.RegisterType<IParentescoAppService, ParentescoAppService>();
            Container.Current.RegisterType<IProfissaoAppService, ProfissaoAppService>();
            Container.Current.RegisterType<IRamoAtividadeAppService, RamoAtividadeAppService>();
            Container.Current.RegisterType<IRelacionamentoAppService, RelacionamentoAppService>();
            Container.Current.RegisterType<ISerieNFAppService, SerieNFAppService>();
            Container.Current.RegisterType<ITipoAreaAppService, TipoAreaAppService>();
            Container.Current.RegisterType<ITipoCaracteristicaAppService, TipoCaracteristicaAppService>();
            Container.Current.RegisterType<ITipoCompraAppService, TipoCompraAppService>();
            Container.Current.RegisterType<ITipoEspecificacaoAppService, TipoEspecificacaoAppService>();
            Container.Current.RegisterType<ITipologiaAppService, TipologiaAppService>();
            Container.Current.RegisterType<ITratamentoAppService, TratamentoAppService>();
            Container.Current.RegisterType<IUnidadeMedidaAppService, UnidadeMedidaAppService>();
            Container.Current.RegisterType<IUnidadeFederacaoAppService, UnidadeFederacaoAppService>();
        }
    }
}