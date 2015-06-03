using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Financeiro;

namespace GIR.Sigim.Infrastructure.Data.Configuration.Financeiro
{
    public class ClasseConfiguration : EntityTypeConfiguration<Classe>
    {
        public ClasseConfiguration()
        {
            ToTable("Classe", "Financeiro");

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

            Property(l => l.ContaContabil)
                .HasMaxLength(50)
                .HasColumnName("contaContabil");

            Property(l => l.CodigoPai)
                .HasMaxLength(18)
                .HasColumnName("pai");

            HasOptional(l => l.ClassePai)
                .WithMany(l => l.ListaFilhos)
                .HasForeignKey(l => l.CodigoPai);

            HasMany(l => l.ListaPreRequisicaoMaterialItem)
                .WithRequired(l => l.Classe);

            HasMany(l => l.ListaRequisicaoMaterialItem)
                .WithRequired(l => l.Classe);

            HasMany(l => l.ListaOrcamentoComposicao)
                .WithRequired(l => l.Classe);

            HasMany(l => l.ListaOrdemCompraItem)
                .WithRequired(l => l.Classe);
        }
    }
}