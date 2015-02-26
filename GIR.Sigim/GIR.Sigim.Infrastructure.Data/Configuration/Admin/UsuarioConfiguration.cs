using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Admin;

namespace GIR.Sigim.Infrastructure.Data.Configuration.Admin
{
    public class UsuarioConfiguration : EntityTypeConfiguration<Usuario>
    {
        public UsuarioConfiguration()
        {
            ToTable("Usuario", "Sigim");

            Property(l => l.Id)
                .HasColumnName("codigo");

            Property(l => l.Nome)
                .IsRequired()
                .HasMaxLength(100);

            Property(l => l.Login)
                .IsRequired()
                .HasMaxLength(50);

            Property(l => l.Senha)
                .HasMaxLength(64)
                .HasColumnName("senhaWeb");

            Property(l => l.Situacao)
                .IsRequired()
                .HasColumnType("char")
                .HasMaxLength(1)
                .HasColumnName("situacao");

            Ignore(l => l.Ativo);

            Ignore(l => l.ParametrosUsuarioId);

            HasRequired(l => l.ParametrosUsuario).WithRequiredPrincipal(l => l.Usuario);

            //TODO: Implementar controle de acesso
            Ignore(l => l.ListaFuncionalidade);
            Ignore(l => l.ListaPerfil);
            //HasMany(l => l.ListaFuncionalidade).WithMany(l => l.ListaUsuario)
            //    .Map(m =>
            //    {
            //        m.ToTable("UsuarioFuncionalidade", "Sigim");
            //        m.MapLeftKey("UsuarioId");
            //        m.MapRightKey("FuncionalidadeId");
            //    });

            //HasMany(l => l.ListaPerfil).WithMany(l => l.ListaUsuario)
            //    .Map(m =>
            //    {
            //        m.ToTable("UsuarioPerfil", "Sigim");
            //        m.MapLeftKey("UsuarioId");
            //        m.MapRightKey("PerfilId");
            //    });
        }
    }
}