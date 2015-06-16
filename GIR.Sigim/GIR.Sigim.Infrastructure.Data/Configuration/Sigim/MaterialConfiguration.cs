using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Sigim;
using GIR.Sigim.Domain.Entity.Orcamento;
using GIR.Sigim.Domain.Entity.OrdemCompra;

namespace GIR.Sigim.Infrastructure.Data.Configuration.Sigim
{
    public class MaterialConfiguration : EntityTypeConfiguration<Material>
    {
        public MaterialConfiguration()
        {
            ToTable("Material", "Sigim");

            Property(l => l.Id)
                .HasColumnName("codigo")
                .HasColumnOrder(1);

            Property(l => l.Descricao)
                .HasMaxLength(400)
                .HasColumnName("descricao")
                .HasColumnOrder(2);

            Property(l => l.SiglaUnidadeMedida)
                .HasMaxLength(6)
                .HasColumnName("unidadeMedida")
                .HasColumnOrder(3);

            HasOptional(l => l.UnidadeMedida)
                .WithMany(l => l.ListaMaterial)
                .HasForeignKey(l => l.SiglaUnidadeMedida);

            Property(l => l.CodigoMaterialClasseInsumo)
                .HasMaxLength(18)
                .HasColumnName("materialClasseInsumo")
                .HasColumnOrder(4);

            HasOptional(l => l.MaterialClasseInsumo)
                .WithMany(l => l.ListaMaterial)
                .HasForeignKey(l => l.CodigoMaterialClasseInsumo);

            Property(l => l.PrecoUnitario)
                .HasPrecision(18, 5)
                .HasColumnName("precoUnitario")
                .HasColumnOrder(5);

            Property(l => l.LoginUsuarioCadastro)
                .HasMaxLength(50)
                .HasColumnName("usuarioCadastro")
                .HasColumnOrder(6);

            Property(l => l.DataCadastro)
                .HasColumnName("dataCadastro")
                .HasColumnOrder(7);

            Property(l => l.LoginUsuarioAlteracao)
                .HasMaxLength(50)
                .HasColumnName("usuarioAlteracao")
                .HasColumnOrder(8);

            Property(l => l.DataAlteracao)
                .HasColumnName("dataAlteracao")
                .HasColumnOrder(9);

            Property(l => l.DataPreco)
                .HasColumnName("dataPreco")
                .HasColumnOrder(10);

            Property(l => l.QuantidadeMinima)
                .HasPrecision(18, 5)
                .HasColumnName("quantidadeMinima")
                .HasColumnOrder(11);

            Property(l => l.EhControladoPorEstoque)
                .HasColumnName("controladoEstoque")
                .HasColumnOrder(12);

            Property(l => l.ContaContabil)
                .HasMaxLength(50)
                .HasColumnName("contaContabil")
                .HasColumnOrder(13);

            Property(l => l.TipoTabela)
                .HasColumnName("tipoTabela")
                .HasColumnOrder(14);

            Property(l => l.AnoMes)
                .HasColumnName("anoMes")
                .HasColumnOrder(15);

            Property(l => l.CodigoExterno)
                .HasMaxLength(100)
                .HasColumnName("codigoExterno")
                .HasColumnOrder(16);

            Property(l => l.CodigoSituacaoMercadoria)
                .HasMaxLength(10)
                .HasColumnName("situacaoMercadoria")
                .HasColumnOrder(17);

            HasOptional(l => l.SituacaoMercadoria)
                .WithMany(l => l.ListaMaterial)
                .HasForeignKey(l => l.CodigoSituacaoMercadoria);

            Property(l => l.CodigoNCM)
                .HasMaxLength(10)
                .HasColumnName("NCM")
                .HasColumnOrder(18);

            HasOptional(l => l.NCM)
                .WithMany(l => l.ListaMaterial)
                .HasForeignKey(l => l.CodigoNCM);

            Property(l => l.TipoMaterial)
                .HasMaxLength(1)
                .HasColumnName("tipoMaterial")
                .HasColumnOrder(19);

            Property(l => l.Situacao)
                .HasColumnType("char")
                .HasMaxLength(1)
                .HasColumnName("situacao")
                .HasColumnOrder(20);

            Ignore(l => l.Ativo);

            HasMany<PreRequisicaoMaterialItem>(l => l.ListaPreRequisicaoMaterialItem)
                .WithRequired(c => c.Material)
                .HasForeignKey(c => c.MaterialId);

            HasMany<RequisicaoMaterialItem>(l => l.ListaRequisicaoMaterialItem)
                .WithRequired(c => c.Material)
                .HasForeignKey(c => c.MaterialId);

            HasMany<OrdemCompraItem>(l => l.ListaOrdemCompraItem)
                .WithRequired(c => c.Material)
                .HasForeignKey(c => c.MaterialId);

            HasMany<OrcamentoInsumoRequisitado>(l => l.ListaOrcamentoInsumoRequisitado)
                .WithOptional(c => c.Material)
                .HasForeignKey(c => c.MaterialId);

            HasMany<OrcamentoInsumoRequisitado>(l => l.ListaOrcamentoInsumoRequisitado)
                .WithOptional(c => c.Material)
                .HasForeignKey(c => c.MaterialId);

            HasMany<OrcamentoComposicaoItem>(l => l.ListaOrcamentoComposicaoItem)
                .WithOptional(c => c.Material)
                .HasForeignKey(c => c.MaterialId);

        }
    }
}