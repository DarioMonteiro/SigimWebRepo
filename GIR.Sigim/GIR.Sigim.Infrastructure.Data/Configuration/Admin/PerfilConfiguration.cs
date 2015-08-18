using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Admin;

namespace GIR.Sigim.Infrastructure.Data.Configuration.Admin
{
    public class PerfilConfiguration : EntityTypeConfiguration<Perfil>
    {
        public PerfilConfiguration()
        {
            ToTable("Perfil", "Sigim");

            Property(l => l.Id)
               .HasColumnName("codigo")
               .HasColumnOrder(1);

            Property(l => l.Descricao)
                .IsRequired()
                .HasColumnName("descricao")
                .HasMaxLength(100)
                .HasColumnOrder(2);

            Property(l => l.ModuloId)
                .HasColumnName("sistema")
                .HasColumnOrder(3);

            HasMany(l => l.ListaFuncionalidade)
                .WithRequired(l => l.Perfil);

            //HasOptional<Modulo>(l => l.Modulo)
            //    .WithMany(l => l.ListaPerfil)
            //    .HasForeignKey(l => l.ModuloId);

            //HasRequired(l => l.Modulo).WithMany(l => l.ListaPerfil);

            //HasMany(l => l.ListaFuncionalidade).WithMany(l => l.ListaPerfil)
            //    .Map(m =>
            //    {
            //        m.ToTable("PerfilFuncionalidade", "Sigim");
            //        m.MapLeftKey("PerfilId");
            //        m.MapRightKey("FuncionalidadeId");
            //    });
        }
    }
}