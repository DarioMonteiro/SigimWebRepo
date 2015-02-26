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

            Property(l => l.ModuloId)
                .HasColumnName("sistema");

            Property(l => l.CodigoCentroCusto)
                .HasColumnName("centroCusto");

            HasRequired(l => l.Usuario)
                .WithMany(l => l.ListaUsuarioCentroCusto)
                .HasForeignKey(l => l.UsuarioId);

            HasOptional(l => l.Modulo)
                .WithMany(l => l.ListaUsuarioCentroCusto)
                .HasForeignKey(l => l.ModuloId);

            HasRequired(l => l.CentroCusto)
                .WithMany(l => l.ListaUsuarioCentroCusto)
                .HasForeignKey(l => l.CodigoCentroCusto);
        }
    }
}