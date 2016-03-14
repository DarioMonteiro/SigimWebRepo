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
    public class ParametrosSigimConfiguration : EntityTypeConfiguration<ParametrosSigim>
    {
        public ParametrosSigimConfiguration()
        {
            ToTable("Parametros", "Sigim");

            Property(l => l.Id)
                .HasColumnName("codigo")
                .HasColumnOrder(46);

            Property(l => l.PercentualMultaAtraso)
                .HasColumnName("percentualMultaAtraso")
                .HasPrecision(18,5)
                .IsRequired()
                .HasColumnOrder(1);

            Property(l => l.PercentualEncargosAtraso)
                .HasColumnName("percentualEncargosAtraso")
                .HasPrecision(18, 5)
                .IsRequired()
                .HasColumnOrder(2);

            Property(l => l.CorrecaoProRata)
                .HasColumnName("correcaoProRata")
                .HasColumnType("tinyint")
                .IsRequired()
                .HasColumnOrder(3);

            Property(l => l.IndiceVendas)
                .HasColumnName("indiceVendas")
                .HasColumnOrder(7);

            Property(l => l.IndiceVendas)
                .HasColumnName("indiceVendas")
                .HasColumnOrder(7);

            Property(l => l.MetodoDescapitalizacao)
                .HasColumnName("metodoDescapitalizacao")
                .HasMaxLength(50)
                .IsRequired()
                .HasColumnOrder(10);

            Property(l => l.ClienteId)
                .HasColumnName("empresa")
                .IsRequired()
                .HasColumnOrder(14);

            HasRequired(l => l.Cliente)
                .WithMany(l => l.ListaParametrosSigim)
                .HasForeignKey(l => l.ClienteId);

            Property(l => l.IconeRelatorio)
                .HasColumnType("image")
                .HasColumnName("iconeRelatorios")
                .HasColumnOrder(15);

            Property(l => l.AplicaEncargosPorMes)
                .HasColumnName("aplicaEncargosPorMes")
                .HasColumnType("bit")
                .HasColumnOrder(33);

            Property(l => l.CorrecaoMesCheioDiaPrimeiro)
                .HasColumnName("correcaoMesCheioDiaPrimeiro")
                .HasColumnType("bit")
                .HasColumnOrder(38);
        }
    }
}
