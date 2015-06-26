using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Sigim;

namespace GIR.Sigim.Infrastructure.Data.Configuration.Sigim
{
    public class UsuarioCentroCustoConfiguration : EntityTypeConfiguration<UsuarioCentroCusto>
    {
        public UsuarioCentroCustoConfiguration()
        {
            ToTable("UsuarioCentroCusto", "Sigim");

            Property(l => l.Id)
                .HasColumnName("codigo");

            Property(l => l.UsuarioId)
                .HasColumnName("usuario");

            HasRequired(l => l.Usuario)
                .WithMany(l => l.ListaUsuarioCentroCusto)
                .HasForeignKey(l => l.UsuarioId);

            Property(l => l.ModuloId)
                .HasColumnName("sistema");

            HasOptional(l => l.Modulo)
                .WithMany(l => l.ListaUsuarioCentroCusto)
                .HasForeignKey(l => l.ModuloId);

            Property(l => l.CodigoCentroCusto)
                .IsRequired()
                .HasMaxLength(18)
                .HasColumnName("centroCusto");

            HasRequired(l => l.CentroCusto)
                .WithMany(c => c.ListaUsuarioCentroCusto)
                .HasForeignKey(l => l.CodigoCentroCusto);



        }
    }
}