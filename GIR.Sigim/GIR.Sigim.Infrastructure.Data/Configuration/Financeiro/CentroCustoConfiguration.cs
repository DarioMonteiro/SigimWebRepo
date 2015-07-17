using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Financeiro;
using GIR.Sigim.Domain.Entity.OrdemCompra;
using GIR.Sigim.Domain.Entity.Sigim;
using GIR.Sigim.Domain.Entity.Orcamento;
using GIR.Sigim.Domain.Entity.Contrato;

namespace GIR.Sigim.Infrastructure.Data.Configuration.Financeiro
{
    public class CentroCustoConfiguration : EntityTypeConfiguration<CentroCusto>
    {
        public CentroCustoConfiguration()
        {
            ToTable("CentroCusto", "Financeiro");

            Ignore(l => l.Id);

            HasKey(l => l.Codigo);

            Property(l => l.Codigo)
                .IsRequired()
                .HasMaxLength(18)
                .HasColumnName("codigo");

            Property(l => l.Descricao)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("descricao");

            Property(l => l.CentroContabil)
                .HasMaxLength(20)
                .HasColumnName("centroContabil");

            Property(l => l.AnoMes)
                .HasColumnName("anoMes");

            Property(l => l.TipoTabela)
                .HasColumnName("tipoTabela");

            Property(l => l.Situacao)
                .IsRequired()
                .HasColumnType("char")
                .HasMaxLength(1)
                .HasColumnName("situacao");

            Ignore(l => l.Ativo);

            Property(l => l.CodigoPai)
                .HasMaxLength(18)
                .HasColumnName("pai");

            HasOptional(l => l.CentroCustoPai)
                .WithMany(l => l.ListaFilhos)
                .HasForeignKey(l => l.CodigoPai);

            HasMany(l => l.ListaPreRequisicaoMaterialItem)
                .WithRequired(l => l.CentroCusto);

        HasMany<CentroCustoEmpresa>(l => l.ListaCentroCustoEmpresa)
            .WithRequired(c => c.CentroCusto)
            .HasForeignKey(c => c.CodigoCentroCusto);

        HasMany<ParametrosUsuario>(l => l.ListaParametrosUsuario)
            .WithOptional(c => c.CentroCusto)
            .HasForeignKey(c => c.CodigoCentroCusto);

        HasMany<UsuarioCentroCusto>(l => l.ListaUsuarioCentroCusto)
            .WithRequired(c => c.CentroCusto)
            .HasForeignKey(c => c.CodigoCentroCusto);

        HasMany<RequisicaoMaterial>(l => l.ListaRequisicaoMaterial)
            .WithRequired(c => c.CentroCusto)
            .HasForeignKey(c => c.CodigoCentroCusto);

        HasMany<Obra>(l => l.ListaObra)
            .WithOptional(c => c.CentroCusto)
            .HasForeignKey(c => c.CodigoCentroCusto);

        HasMany<Domain.Entity.Contrato.Contrato>(l => l.ListaContrato)
            .WithRequired(c => c.CentroCusto)
            .HasForeignKey(c => c.CodigoCentroCusto);

        HasMany<Licitacao>(l => l.ListaLicitacao)
            .WithRequired(c => c.CentroCusto)
            .HasForeignKey(c => c.CodigoCentroCusto);

        HasMany<LicitacaoCronograma>(l => l.ListaLicitacaoCronograma)
            .WithRequired(c => c.CentroCusto)
            .HasForeignKey(c => c.CodigoCentroCusto);

        HasMany<BloqueioContabil>(l => l.ListaBloqueioContabil)
            .WithOptional(c => c.CentroCusto)
            .HasForeignKey(c => c.CodigoCentroCusto);

        HasMany<Caixa>(l => l.ListaCaixa)
            .WithOptional(c => c.CentroCusto)
            .HasForeignKey(c => c.CodigoCentroCusto);

        HasMany<OrcamentoInsumoRequisitado>(l => l.ListaOrcamentoInsumoRequisitado)
            .WithRequired(c => c.CentroCusto)
            .HasForeignKey(c => c.CodigoCentroCusto);

        HasMany<Domain.Entity.OrdemCompra.OrdemCompra>(l => l.ListaOrdemCompra)
            .WithRequired(c => c.CentroCusto)
            .HasForeignKey(c => c.CodigoCentroCusto);

        }
    }
}