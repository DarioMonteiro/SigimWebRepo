using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Repository.Sigim;
using GIR.Sigim.Infrastructure.Crosscutting.IoC;
using GIR.Sigim.Infrastructure.Data.Repository.Sigim;
using Microsoft.Practices.Unity;

namespace GIR.Sigim.Infrastructure.Data.IoC
{
    public class SigimBootstraper
    {
        public static void Initialise()
        {
            Container.Current.RegisterType<IAgenciaRepository, AgenciaRepository>();
            Container.Current.RegisterType<IAssuntoContatoRepository, AssuntoContatoRepository>();
            Container.Current.RegisterType<IBancoLayoutRepository, BancoLayoutRepository>();
            Container.Current.RegisterType<IBancoRepository, BancoRepository>();
            Container.Current.RegisterType<IBloqueioContabilRepository, BloqueioContabilRepository>();
            Container.Current.RegisterType<ICifFobRepository, CifFobRepository>();
            Container.Current.RegisterType<IClienteFornecedorRepository, ClienteFornecedorRepository>();
            Container.Current.RegisterType<ICodigoContribuicaoRepository, CodigoContribuicaoRepository>();
            Container.Current.RegisterType<IComplementoCSTRepository, ComplementoCSTRepository>();
            Container.Current.RegisterType<IComplementoNaturezaOperacaoRepository, ComplementoNaturezaOperacaoRepository>();
            Container.Current.RegisterType<IContaCorrenteRepository, ContaCorrenteRepository>();
            Container.Current.RegisterType<ICSTRepository, CSTRepository>();
            Container.Current.RegisterType<IEstadoCivilRepository, EstadoCivilRepository>();
            Container.Current.RegisterType<IFeriadoRepository, FeriadoRepository>();
            Container.Current.RegisterType<IFonteNegocioRepository, FonteNegocioRepository>();
            Container.Current.RegisterType<IFormaRecebimentoRepository, FormaRecebimentoRepository>();
            Container.Current.RegisterType<IGrupoRepository, GrupoRepository>();
            Container.Current.RegisterType<IInteresseBairroRepository, InteresseBairroRepository>();
            Container.Current.RegisterType<ILogAcessoRepository, LogAcessoRepository>();
            Container.Current.RegisterType<ILogOperacaoRepository, LogOperacaoRepository>();
            Container.Current.RegisterType<IMaterialClasseInsumoRepository, MaterialClasseInsumoRepository>();
            Container.Current.RegisterType<IMaterialRepository, MaterialRepository>();
            Container.Current.RegisterType<INacionalidadeRepository, NacionalidadeRepository>();
            Container.Current.RegisterType<INaturezaOperacaoRepository, NaturezaOperacaoRepository>();
            Container.Current.RegisterType<INaturezaReceitaRepository, NaturezaReceitaRepository>();
            Container.Current.RegisterType<IParentescoRepository, ParentescoRepository>();
            Container.Current.RegisterType<IProfissaoRepository, ProfissaoRepository>();
            Container.Current.RegisterType<IRamoAtividadeRepository, RamoAtividadeRepository>();
            Container.Current.RegisterType<IRelacionamentoRepository, RelacionamentoRepository>();
            Container.Current.RegisterType<ISerieNFRepository, SerieNFRepository>();
            Container.Current.RegisterType<ITipoAreaRepository, TipoAreaRepository>();
            Container.Current.RegisterType<ITipoCaracteristicaRepository, TipoCaracteristicaRepository>();
            Container.Current.RegisterType<ITipoCompraRepository, TipoCompraRepository>();
            Container.Current.RegisterType<ITipoEspecificacaoRepository, TipoEspecificacaoRepository>();
            Container.Current.RegisterType<ITipologiaRepository, TipologiaRepository>();
            Container.Current.RegisterType<ITratamentoRepository, TratamentoRepository>();
            Container.Current.RegisterType<IUnidadeMedidaRepository, UnidadeMedidaRepository>();
            Container.Current.RegisterType<IUnidadeFederacaoRepository, UnidadeFederacaoRepository>();
        }
    }
}