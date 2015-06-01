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
    public class CaixaConfiguration : EntityTypeConfiguration<Caixa>
    {
        public CaixaConfiguration()
        {
            ToTable("Caixa", "Financeiro");

            Property(l => l.Id)
                .HasColumnName("codigo")
                .HasColumnOrder(1);

            Property(l => l.Descricao)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("descricao")
                .HasColumnOrder(2);

            Property(l => l.Situacao)
                .HasColumnName("situacao")
                .HasColumnType("char")
                .HasMaxLength(1)
                .HasColumnOrder(3);

            Property(l => l.CentroContabil)
                .HasMaxLength(20)
                .HasColumnName("centroContabil")
                .HasColumnOrder(4);

            Property(l => l.CodigoCentroCusto)
                .HasMaxLength(18)
                .HasColumnName("centroCusto")
                .HasColumnOrder(5);

            HasOptional(l => l.CentroCusto)
                .WithMany(l => l.ListaCaixa)
                .HasForeignKey(l => l.CodigoCentroCusto);


        }
    }
}