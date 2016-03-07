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

            Property(l => l.IndiceVendas)
                .HasColumnName("indiceVendas")
                .HasColumnOrder(7);

            Property(l => l.MetodoDescapitalizacao)
                .HasColumnName("metodoDescapitalizacao")
                .HasMaxLength(50)
                .HasColumnOrder(10);

            Property(l => l.ClienteId)
                .HasColumnName("empresa")
                .HasColumnOrder(14);

            HasOptional(l => l.Cliente)
                .WithMany(l => l.ListaParametrosSigim);

            Property(l => l.IconeRelatorio)
                .HasColumnType("image")
                .HasColumnName("iconeRelatorios")
                .HasColumnOrder(15);


            Property(l => l.CorrecaoMesCheioDiaPrimeiro)
                .HasColumnName("correcaoMesCheioDiaPrimeiro")
                .HasColumnOrder(38);
        }
    }
}
