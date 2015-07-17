using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Sigim;

namespace GIR.Sigim.Infrastructure.Data.Configuration.Sigim
{
    public class MaterialClasseInsumoConfiguration : EntityTypeConfiguration<MaterialClasseInsumo>
    {
        public MaterialClasseInsumoConfiguration()
        {
            ToTable("MaterialClasseInsumo", "Sigim");

            Ignore(l => l.Id);

            HasKey(l => l.Codigo);

            Property(l => l.Codigo)
                .HasMaxLength(18)
                .HasColumnName("codigo")
                .HasColumnOrder(1);

            Property(l => l.Descricao)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("descricao")
                .HasColumnOrder(2);

            Property(l => l.EhMovimentoTemporario)
                .HasColumnName("movimentoTemporario")
                .HasColumnOrder(3);

            Property(l => l.Sequencial)
                .HasColumnName("sequencial")
                .HasColumnOrder(4);

            Property(l => l.NaoGeraSPED)
                .HasColumnName("naoGeraSPED")
                .HasColumnOrder(5);

            Property(l => l.CodigoPai)
                .HasMaxLength(18)
                .HasColumnName("pai")
                .HasColumnOrder(6);

            HasOptional<MaterialClasseInsumo>(l => l.ClassePai)
                .WithMany(c => c.ListaFilhos)
                .HasForeignKey(c => c.CodigoPai);

            HasMany<Material>(l => l.ListaMaterial)
                .WithOptional(l => l.MaterialClasseInsumo);
        }
    }
}