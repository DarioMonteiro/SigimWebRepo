using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity;
using GIR.Sigim.Domain.Entity.Admin;
using GIR.Sigim.Infrastructure.Data;
using GIR.Sigim.Infrastructure.Data.Configuration.Admin;
using GIR.Sigim.Infrastructure.Data.Configuration.Comercial;
using GIR.Sigim.Infrastructure.Data.Configuration.Contrato;
using GIR.Sigim.Infrastructure.Data.Configuration.CredCob;
using GIR.Sigim.Infrastructure.Data.Configuration.Financeiro;
using GIR.Sigim.Infrastructure.Data.Configuration.Orcamento;
using GIR.Sigim.Infrastructure.Data.Configuration.OrdemCompra;
using GIR.Sigim.Infrastructure.Data.Configuration.Sac;
using GIR.Sigim.Infrastructure.Data.Configuration.Sigim;

namespace GIR.Sigim.Infrastructure.Data
{
    public class UnitOfWork : DbContext, IQueryableUnitOfWork
    {
        #region Constructor

        public UnitOfWork()
            : base("DefaultConnection")
        {
            //Database.SetInitializer<UnitOfWork>(new DropCreateDatabaseIfModelChanges<UnitOfWork>());
            //Database.SetInitializer(new DropCreateDatabaseAlways<Context>());
        }

        #endregion

        #region IQueryableUnitOfWork Members

        public DbSet<TEntity> CreateSet<TEntity>() where TEntity : BaseEntity
        {
            return base.Set<TEntity>();
        }

        public void Attach<TEntity>(TEntity item) where TEntity : BaseEntity
        {
            base.Entry<TEntity>(item).State = System.Data.Entity.EntityState.Unchanged;
        }

        public void SetModified<TEntity>(TEntity item) where TEntity : BaseEntity
        {
            if (base.Entry<TEntity>(item).State == System.Data.Entity.EntityState.Detached)
            {
                var oldItem = base.Set<TEntity>().Find(item.Id);
                ApplyCurrentValues<TEntity>(oldItem, item);
            }
            else
            {
                base.Entry<TEntity>(item).State = System.Data.Entity.EntityState.Modified;
            }
        }

        public void ApplyCurrentValues<TEntity>(TEntity original, TEntity current) where TEntity : BaseEntity
        {
            base.Entry<TEntity>(original).CurrentValues.SetValues(current);
        }

        public void Commit()
        {
            try
            {
                base.SaveChanges();
            }
            catch(Exception ex)
            {
                this.RollbackChanges();
                throw ex;
            }
        }

        public void RollbackChanges()
        {
            base.ChangeTracker.Entries()
                              .ToList()
                              .ForEach(entry => entry.State = System.Data.Entity.EntityState.Unchanged);
        }

        #endregion

        #region DbContext Overrides

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            modelBuilder.Properties<string>()
                .Configure(p => p.HasColumnType("varchar"));

            #region Admin

            modelBuilder.Configurations.Add(new UsuarioConfiguration());
            modelBuilder.Configurations.Add(new UsuarioFuncionalidadeConfiguration());
            modelBuilder.Configurations.Add(new UsuarioPerfilConfiguration());
            modelBuilder.Configurations.Add(new ModuloConfiguration());
            modelBuilder.Configurations.Add(new PerfilConfiguration());
            modelBuilder.Configurations.Add(new PerfilFuncionalidadeConfiguration());
            modelBuilder.Configurations.Add(new UnidadeFederacaoConfiguration());

            #endregion

            #region Comercial

            modelBuilder.Configurations.Add(new IncorporadorConfiguration());
            modelBuilder.Configurations.Add(new EmpreendimentoConfiguration());
            modelBuilder.Configurations.Add(new BlocoConfiguration());
            modelBuilder.Configurations.Add(new ContratoComercialConfiguration());
            modelBuilder.Configurations.Add(new TipoParticipanteConfiguration());
            modelBuilder.Configurations.Add(new UnidadeConfiguration());
            modelBuilder.Configurations.Add(new VendaConfiguration());
            modelBuilder.Configurations.Add(new VendaParticipanteConfiguration());
            modelBuilder.Configurations.Add(new VendaSerieConfiguration());
            modelBuilder.Configurations.Add(new TabelaVendaConfiguration());

            #endregion

            #region Contrato

            modelBuilder.Configurations.Add(new ParametrosContratoConfiguration());
            modelBuilder.Configurations.Add(new LicitacaoDescricaoConfiguration());
            modelBuilder.Configurations.Add(new LicitacaoConfiguration());
            modelBuilder.Configurations.Add(new LicitacaoCronogramaConfiguration());
            modelBuilder.Configurations.Add(new ContratoConfiguration());
            modelBuilder.Configurations.Add(new ContratoRetencaoConfiguration());
            modelBuilder.Configurations.Add(new ContratoRetencaoLiberadaConfiguration());
            modelBuilder.Configurations.Add(new ContratoRetificacaoConfiguration());
            modelBuilder.Configurations.Add(new ContratoRetificacaoItemConfiguration());
            modelBuilder.Configurations.Add(new ContratoRetificacaoProvisaoConfiguration());
            modelBuilder.Configurations.Add(new ContratoRetificacaoItemCronogramaConfiguration());
            modelBuilder.Configurations.Add(new ContratoRetificacaoItemMedicaoConfiguration());
            modelBuilder.Configurations.Add(new ContratoRetificacaoItemMedicaoNFConfiguration());
            modelBuilder.Configurations.Add(new ContratoRetificacaoItemImpostoConfiguration());

            #endregion

            #region CrebCob

            modelBuilder.Configurations.Add(new TituloCredCobConfiguration());
            modelBuilder.Configurations.Add(new VerbaCobrancaConfiguration());

            #endregion

            #region Estoque

            modelBuilder.Configurations.Add(new EstoqueConfiguration());
            modelBuilder.Configurations.Add(new EstoqueMaterialConfiguration());
            modelBuilder.Configurations.Add(new MovimentoConfiguration());
            modelBuilder.Configurations.Add(new MovimentoItemConfiguration());

            #endregion

            #region Financeiro

            modelBuilder.Configurations.Add(new ApropriacaoAdiantamentoConfiguration());
            modelBuilder.Configurations.Add(new ApropriacaoConfiguration());
            modelBuilder.Configurations.Add(new CaixaConfiguration());
            modelBuilder.Configurations.Add(new CentroCustoConfiguration());
            modelBuilder.Configurations.Add(new CentroCustoEmpresaConfiguration());
            modelBuilder.Configurations.Add(new ClasseConfiguration());
            modelBuilder.Configurations.Add(new HistoricoContabilConfiguration());
            modelBuilder.Configurations.Add(new ImpostoFinanceiroConfiguration());
            modelBuilder.Configurations.Add(new ImpostoPagarConfiguration());
            modelBuilder.Configurations.Add(new ImpostoReceberConfiguration());
            modelBuilder.Configurations.Add(new MotivoCancelamentoConfiguration());
            modelBuilder.Configurations.Add(new MovimentoFinanceiroConfiguration());
            modelBuilder.Configurations.Add(new ParametrosFinanceiroConfiguration());
            modelBuilder.Configurations.Add(new ParametrosUsuarioFinanceiroConfiguration());
            modelBuilder.Configurations.Add(new RateioAutomaticoConfiguration());
            modelBuilder.Configurations.Add(new TaxaAdministracaoConfiguration());
            modelBuilder.Configurations.Add(new TipoCompromissoConfiguration());
            modelBuilder.Configurations.Add(new TipoDocumentoConfiguration());
            modelBuilder.Configurations.Add(new TipoMovimentoConfiguration());
            modelBuilder.Configurations.Add(new TipoRateioConfiguration());
            modelBuilder.Configurations.Add(new TituloPagarAdiantamentoConfiguration());
            modelBuilder.Configurations.Add(new TituloPagarConfiguration());
            modelBuilder.Configurations.Add(new TituloReceberConfiguration());

            #endregion

            #region Orcamento

            modelBuilder.Configurations.Add(new ObraConfiguration());
            modelBuilder.Configurations.Add(new OrcamentoComposicaoConfiguration());
            modelBuilder.Configurations.Add(new OrcamentoComposicaoItemConfiguration());
            modelBuilder.Configurations.Add(new OrcamentoConfiguration());
            modelBuilder.Configurations.Add(new OrcamentoInsumoRequisitadoConfiguration());
            modelBuilder.Configurations.Add(new ParametrosOrcamentoConfiguration());

            #endregion

            #region OrdemCompra

            modelBuilder.Configurations.Add(new CotacaoConfiguration());
            modelBuilder.Configurations.Add(new CotacaoItemConfiguration());
            modelBuilder.Configurations.Add(new EntradaMaterialConfiguration());
            modelBuilder.Configurations.Add(new EntradaMaterialFormaPagamentoConfiguration());
            modelBuilder.Configurations.Add(new EntradaMaterialImpostoConfiguration());
            modelBuilder.Configurations.Add(new EntradaMaterialItemConfiguration());
            modelBuilder.Configurations.Add(new OrdemCompraConfiguration());
            modelBuilder.Configurations.Add(new OrdemCompraFormaPagamentoConfiguration());
            modelBuilder.Configurations.Add(new OrdemCompraItemConfiguration());
            modelBuilder.Configurations.Add(new ParametrosOrdemCompraConfiguration());
            modelBuilder.Configurations.Add(new ParametrosUsuarioConfiguration());
            modelBuilder.Configurations.Add(new PreRequisicaoMaterialConfiguration());
            modelBuilder.Configurations.Add(new PreRequisicaoMaterialItemConfiguration());
            modelBuilder.Configurations.Add(new RequisicaoMaterialConfiguration());
            modelBuilder.Configurations.Add(new RequisicaoMaterialItemConfiguration());

            #endregion

            #region Sigim

            modelBuilder.Configurations.Add(new AgenciaConfiguration());
            modelBuilder.Configurations.Add(new AssuntoContatoConfiguration());
            modelBuilder.Configurations.Add(new AvaliacaoFornecedorConfiguration());
            modelBuilder.Configurations.Add(new AvaliacaoModeloConfiguration());
            modelBuilder.Configurations.Add(new BancoConfiguration());
            modelBuilder.Configurations.Add(new BancoLayoutConfiguration());
            modelBuilder.Configurations.Add(new BloqueioContabilConfiguration());
            modelBuilder.Configurations.Add(new CifFobConfiguration());
            modelBuilder.Configurations.Add(new ClienteFornecedorConfiguration());
            modelBuilder.Configurations.Add(new CodigoContribuicaoConfiguration());
            modelBuilder.Configurations.Add(new ComplementoCSTConfiguration());
            modelBuilder.Configurations.Add(new ComplementoNaturezaOperacaoConfiguration());
            modelBuilder.Configurations.Add(new ComposicaoConfiguration());
            modelBuilder.Configurations.Add(new ContaCorrenteConfiguration());
            modelBuilder.Configurations.Add(new CotacaoValoresConfiguration());
            modelBuilder.Configurations.Add(new CSTConfiguration());
            modelBuilder.Configurations.Add(new EnderecoConfiguration());
            modelBuilder.Configurations.Add(new EstadoCivilConfiguration());
            modelBuilder.Configurations.Add(new FeriadoConfiguration());
            modelBuilder.Configurations.Add(new FonteNegocioConfiguration());
            modelBuilder.Configurations.Add(new FormaRecebimentoConfiguration());
            modelBuilder.Configurations.Add(new GrupoConfiguration());
            modelBuilder.Configurations.Add(new IndiceFinanceiroConfiguration());
            modelBuilder.Configurations.Add(new InteresseBairroConfiguration());
            modelBuilder.Configurations.Add(new LogAcessoConfiguration());
            modelBuilder.Configurations.Add(new LogOperacaoConfiguration());
            modelBuilder.Configurations.Add(new MaterialClasseInsumoConfiguration());
            modelBuilder.Configurations.Add(new MaterialConfiguration());
            modelBuilder.Configurations.Add(new NacionalidadeConfiguration());
            modelBuilder.Configurations.Add(new NaturezaOperacaoConfiguration());
            modelBuilder.Configurations.Add(new NaturezaReceitaConfiguration());
            modelBuilder.Configurations.Add(new NCMConfiguration());
            modelBuilder.Configurations.Add(new ParentescoConfiguration());
            modelBuilder.Configurations.Add(new PessoaFisicaConfiguration());
            modelBuilder.Configurations.Add(new PessoaJuridicaConfiguration());
            modelBuilder.Configurations.Add(new ProfissaoConfiguration());
            modelBuilder.Configurations.Add(new RamoAtividadeConfiguration());
            modelBuilder.Configurations.Add(new RelacionamentoConfiguration());
            modelBuilder.Configurations.Add(new SerieNFConfiguration());
            modelBuilder.Configurations.Add(new ServicoConfiguration());  
            modelBuilder.Configurations.Add(new SituacaoMercadoriaConfiguration());
            modelBuilder.Configurations.Add(new TipoAreaConfiguration());
            modelBuilder.Configurations.Add(new TipoCaracteristicaConfiguration());
            modelBuilder.Configurations.Add(new TipoCompraConfiguration());
            modelBuilder.Configurations.Add(new TipoEspecificacaoConfiguration());
            modelBuilder.Configurations.Add(new TipologiaConfiguration());
            modelBuilder.Configurations.Add(new TratamentoConfiguration());
            modelBuilder.Configurations.Add(new UnidadeMedidaConfiguration());
            modelBuilder.Configurations.Add(new UsuarioCentroCustoConfiguration());

            #endregion

            #region Sac

            modelBuilder.Configurations.Add(new ParametrosSacConfiguration());
            modelBuilder.Configurations.Add(new SetorConfiguration());
            modelBuilder.Configurations.Add(new ParametrosEmailSacConfiguration());
            
            #endregion
        }

        #endregion
    }
}