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
    public class ApropriacaoConfiguration : EntityTypeConfiguration<Apropriacao>
    {
        public ApropriacaoConfiguration()
        {
            ToTable("Apropriacao", "Financeiro");

            Property(l => l.Id)
                .HasColumnName("codigo")
                .HasColumnOrder(1);

            Property(l => l.CodigoClasse)
                .HasMaxLength(18)
                .HasColumnName("classe");

            HasRequired(l => l.Classe)
                .WithMany(l => l.ListaApropriacao)
                .HasForeignKey(l => l.CodigoClasse);

            Property(l => l.CodigoCentroCusto)
                .HasMaxLength(18)
                .HasColumnName("centroCusto");

            HasRequired(l => l.CentroCusto)
                .WithMany(l => l.ListaApropriacao)
                .HasForeignKey(l => l.CodigoCentroCusto);

            Property(l => l.TituloPagarId)
                .HasColumnName("tituloPagar");

            HasOptional(l => l.TituloPagar)
                .WithMany(l => l.ListaApropriacao)
                .HasForeignKey(l => l.TituloPagarId);

            Property(l => l.TituloReceberId)
                .HasColumnName("tituloReceber");

            HasOptional(l => l.TituloReceber)
                .WithMany(l => l.ListaApropriacao)
                .HasForeignKey(l => l.TituloReceberId);

            Property(l => l.MovimentoId)
                .HasColumnName("movimento");

            HasOptional(l => l.Movimento)
                .WithMany(l => l.ListaApropriacao)
                .HasForeignKey(l => l.MovimentoId);

            Property(l => l.Valor)
                .HasPrecision(18, 5)
                .HasColumnName("valor");

            Property(l => l.Percentual)
                .HasPrecision(18, 5)
                .HasColumnName("percentual");
        }
    }
}