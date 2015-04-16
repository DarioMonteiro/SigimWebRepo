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
    public class ServicoConfiguration : EntityTypeConfiguration<Servico>
    {
        public ServicoConfiguration()
        {
            ToTable("Servico","Sigim");

            Property(l => l.Id)
                .HasColumnName("codigo")
                .HasColumnOrder(1);

            Property(l => l.Descricao)
                .HasColumnName("descricao")
                .HasMaxLength(400)
                .HasColumnOrder(2);

            Property(l => l.SiglaUnidadeMedida)
                .HasMaxLength(6)
                .HasColumnName("unidadeMedida")
                .HasColumnOrder(3);

            HasOptional(l => l.UnidadeMedida)
                .WithMany(c => c.ListaServico)
                .HasForeignKey(l => l.SiglaUnidadeMedida);

            Property(l => l.PrecoUnitario)
                .HasPrecision(18, 5)
                .HasColumnName("precoUnitario")
                .HasColumnOrder(5);

            Property(l => l.Situacao)
                .HasColumnName("situacao")
                .HasMaxLength(1)
                .HasColumnOrder(15); 
        }
    }
}
