using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using GIR.Sigim.Domain.Entity.Comercial;

namespace GIR.Sigim.Infrastructure.Data.Configuration.Comercial
{
    public class IncorporadorAssociadoConfiguration : EntityTypeConfiguration<IncorporadorAssociado>
    {
        public IncorporadorAssociadoConfiguration()
        {
            ToTable("IncorporadorAssociado", "Comercial");

            Property(l => l.Id)
                .HasColumnName("codigo")
                .HasColumnOrder(1);

            Property(l => l.IncorporadorId)
                .IsRequired()
                .HasColumnName("incorporador")
                .HasColumnOrder(2);

            HasRequired<Incorporador>(l => l.Incorporador)
                .WithMany(c => c.ListaIncorporadorAssociado)
                .HasForeignKey(l => l.IncorporadorId);

            Property(l => l.BlocoId)
                .IsRequired()
                .HasColumnName("bloco")
                .HasColumnOrder(3);

            HasRequired<Bloco>(l => l.Bloco)
                .WithMany(c => c.ListaIncorporadorAssociado)
                .HasForeignKey(l => l.BlocoId);

            Property(l => l.UnidadeId)
                .IsRequired()
                .HasColumnName("unidade")
                .HasColumnOrder(4);

            HasRequired<Unidade>(l => l.Unidade)
                .WithMany(c => c.ListaIncorporadorAssociado)
                .HasForeignKey(l => l.UnidadeId);

            Property(l => l.DataVigencia)
                .IsRequired()
                .HasColumnName("dataVigencia")
                .HasColumnOrder(5);

            Property(l => l.PercentualCusto)
                .HasColumnName("PercentualCusto")
                .HasPrecision(18,5)
                .HasColumnOrder(6);

            Property(l => l.PercentualReceita)
                .HasColumnName("percentualReceita")
                .HasPrecision(18, 5)
                .HasColumnOrder(7);

            Property(l => l.EfetuaRateioBaixaTitulo)
                .HasColumnName("efetuaRateioBaixaTitulo")
                .HasColumnType("char")
                .HasMaxLength(1)
                .HasColumnOrder(8);

        }
    }
}
