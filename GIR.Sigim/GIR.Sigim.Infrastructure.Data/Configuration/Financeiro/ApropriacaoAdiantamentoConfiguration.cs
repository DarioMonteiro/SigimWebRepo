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
    public class ApropriacaoAdiantamentoConfiguration : EntityTypeConfiguration<ApropriacaoAdiantamento>
    {
        public ApropriacaoAdiantamentoConfiguration()
        {
            ToTable("ApropriacaoAdiantamento", "Financeiro");

            Property(l => l.Id)
                .HasColumnName("codigo")
                .HasColumnOrder(1);

            Property(l => l.CodigoClasse)
                .HasMaxLength(18)
                .HasColumnName("classe");

            HasRequired(l => l.Classe)
                .WithMany(l => l.ListaApropriacaoAdiantamento)
                .HasForeignKey(l => l.CodigoClasse);

            Property(l => l.CodigoCentroCusto)
                .HasMaxLength(18)
                .HasColumnName("centroCusto");

            HasRequired(l => l.CentroCusto)
                .WithMany(l => l.ListaApropriacaoAdiantamento)
                .HasForeignKey(l => l.CodigoCentroCusto);

            Property(l => l.TituloPagarAdiantamentoId)
                .HasColumnName("tituloPagarAdiantamento");

            HasRequired(l => l.TituloPagarAdiantamento)
                .WithMany(l => l.ListaApropriacaoAdiantamento)
                .HasForeignKey(l => l.TituloPagarAdiantamentoId);

            Property(l => l.Valor)
                .HasPrecision(18, 5)
                .HasColumnName("valor");

            Property(l => l.Percentual)
                .HasPrecision(18, 5)
                .HasColumnName("percentual");
        }
    }
}