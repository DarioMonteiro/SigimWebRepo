using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Admin;

namespace GIR.Sigim.Infrastructure.Data.Configuration.Admin
{
    public class UsuarioFuncionalidadeConfiguration : EntityTypeConfiguration<UsuarioFuncionalidade>
    {
        public UsuarioFuncionalidadeConfiguration()
        {
            ToTable("usuarioFuncionalidade", "Sigim");

            Property(l => l.Id)
               .HasColumnName("codigo")
               .HasColumnOrder(1);

            Property(l => l.UsuarioId)
               .HasColumnName("usuario")
               .HasColumnOrder(2);

            HasRequired(l => l.Usuario)
               .WithMany(l => l.ListaUsuarioFuncionalidade)
               .HasForeignKey(l => l.UsuarioId);

            Property(l => l.ModuloId)
               .HasColumnName("sistema")
               .HasColumnOrder(3);

            Property(l => l.Funcionalidade)
                .IsRequired()
                .HasColumnName("funcionalidade")
                .HasMaxLength(200)
                .HasColumnOrder(4);

        }
    }
}