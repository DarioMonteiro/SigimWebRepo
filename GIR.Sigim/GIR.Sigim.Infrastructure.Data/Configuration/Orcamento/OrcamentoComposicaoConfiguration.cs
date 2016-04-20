using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Orcamento;

namespace GIR.Sigim.Infrastructure.Data.Configuration.Orcamento
{
    public class OrcamentoComposicaoConfiguration : EntityTypeConfiguration<OrcamentoComposicao>
    {
        public OrcamentoComposicaoConfiguration()
        {
            ToTable("OrcamentoComposicao", "Orcamento");

            Property(l => l.Id)
                .HasColumnName("codigo")
                .HasColumnOrder(1);

            Property(l => l.OrcamentoId)
                .HasColumnName("orcamento")
                .HasColumnOrder(2);

            HasRequired(l => l.Orcamento)
                .WithMany(l => l.ListaOrcamentoComposicao)
                .HasForeignKey(l => l.OrcamentoId);

            Property(l => l.ComposicaoId)
                .HasColumnName("composicao")
                .HasColumnOrder(3);

            HasOptional(l => l.Composicao)
                .WithMany(l => l.ListaOrcamentoComposicao)
                .HasForeignKey(l => l.ComposicaoId);

            Property(l => l.CodigoClasse)
                .HasMaxLength(18)
                .HasColumnName("classe")
                .HasColumnOrder(4);

            HasRequired(l => l.Classe)
                .WithMany(l => l.ListaOrcamentoComposicao)
                .HasForeignKey(l => l.CodigoClasse);

            Property(l => l.Quantidade)
                .HasPrecision(18, 5)
                .HasColumnName("quantidade")
                .HasColumnOrder(5);

            Property(l => l.Preco)
                .HasPrecision(18, 5)
                .HasColumnName("preco")
                .HasColumnOrder(6);

            Property(l => l.EhSincronizada)
                .HasColumnName("sincronizada")
                .HasColumnOrder(7);

            Property(l => l.EspecificacaoTecnica)
                .HasMaxLength(200)
                .HasColumnName("especificacaoTecnica")
                .HasColumnOrder(8);

            HasMany<OrcamentoComposicaoItem>(l => l.ListaOrcamentoComposicaoItem)
                .WithRequired(l => l.OrcamentoComposicao)
                .HasForeignKey(c => c.OrcamentoComposicaoId);
        }
    }
}