﻿using System;
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
using GIR.Sigim.Infrastructure.Data.Configuration.Contrato;
using GIR.Sigim.Infrastructure.Data.Configuration.Financeiro;
using GIR.Sigim.Infrastructure.Data.Configuration.Orcamento;
using GIR.Sigim.Infrastructure.Data.Configuration.OrdemCompra;
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

            //Admin
            //modelBuilder.Configurations.Add(new FuncionalidadeConfiguration());
            modelBuilder.Configurations.Add(new ModuloConfiguration());
            //modelBuilder.Configurations.Add(new PerfilConfiguration());
            modelBuilder.Configurations.Add(new UsuarioConfiguration());

            //Contrato
            modelBuilder.Configurations.Add(new ParametrosContratoConfiguration());
            modelBuilder.Configurations.Add(new LicitacaoDescricaoConfiguration());
            modelBuilder.Configurations.Add(new LicitacaoConfiguration());
            modelBuilder.Configurations.Add(new LicitacaoCronogramaConfiguration());
            modelBuilder.Configurations.Add(new ContratoConfiguration());
            modelBuilder.Configurations.Add(new ContratoRetificacaoConfiguration());
            modelBuilder.Configurations.Add(new ContratoRetificacaoItemConfiguration());  

            //Financeiro
            modelBuilder.Configurations.Add(new CentroCustoConfiguration());
            modelBuilder.Configurations.Add(new CentroCustoEmpresaConfiguration());
            modelBuilder.Configurations.Add(new ClasseConfiguration());
            modelBuilder.Configurations.Add(new TipoCompromissoConfiguration());
            modelBuilder.Configurations.Add(new ParametrosUsuarioFinanceiroConfiguration());
            modelBuilder.Configurations.Add(new TipoDocumentoConfiguration());

            //Orcamento
            modelBuilder.Configurations.Add(new ObraConfiguration());
            modelBuilder.Configurations.Add(new OrcamentoComposicaoConfiguration());
            modelBuilder.Configurations.Add(new OrcamentoConfiguration());
            modelBuilder.Configurations.Add(new ParametrosOrcamentoConfiguration());

            //OrdemCompra
            modelBuilder.Configurations.Add(new ParametrosOrdemCompraConfiguration());
            modelBuilder.Configurations.Add(new ParametrosUsuarioConfiguration());
            modelBuilder.Configurations.Add(new PreRequisicaoMaterialConfiguration());
            modelBuilder.Configurations.Add(new PreRequisicaoMaterialItemConfiguration());
            modelBuilder.Configurations.Add(new RequisicaoMaterialConfiguration());
            modelBuilder.Configurations.Add(new RequisicaoMaterialItemConfiguration());

            //Sigim
            modelBuilder.Configurations.Add(new AssuntoContatoConfiguration());
            modelBuilder.Configurations.Add(new BancoConfiguration());
            modelBuilder.Configurations.Add(new BancoLayoutConfiguration());
            modelBuilder.Configurations.Add(new ClienteFornecedorConfiguration());
            modelBuilder.Configurations.Add(new PessoaFisicaConfiguration());
            modelBuilder.Configurations.Add(new PessoaJuridicaConfiguration()); 
            modelBuilder.Configurations.Add(new ComposicaoConfiguration());
            modelBuilder.Configurations.Add(new MaterialClasseInsumoConfiguration());
            modelBuilder.Configurations.Add(new MaterialConfiguration());
            modelBuilder.Configurations.Add(new ServicoConfiguration());  
            modelBuilder.Configurations.Add(new NCMConfiguration());
            modelBuilder.Configurations.Add(new SituacaoMercadoriaConfiguration());
            modelBuilder.Configurations.Add(new UnidadeMedidaConfiguration());
            modelBuilder.Configurations.Add(new UsuarioCentroCustoConfiguration());
            modelBuilder.Configurations.Add(new TipoCompraConfiguration());
            modelBuilder.Configurations.Add(new CifFobConfiguration());
            modelBuilder.Configurations.Add(new NaturezaOperacaoConfiguration());
            modelBuilder.Configurations.Add(new SerieNFConfiguration());
            modelBuilder.Configurations.Add(new CSTConfiguration());
            modelBuilder.Configurations.Add(new CodigoContribuicaoConfiguration());
        }
        #endregion
    }
}