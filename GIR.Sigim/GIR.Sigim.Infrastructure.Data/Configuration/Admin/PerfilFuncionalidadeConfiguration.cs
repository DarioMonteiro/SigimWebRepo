using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Admin;

namespace GIR.Sigim.Infrastructure.Data.Configuration.Admin
{
    public class PerfilFuncionalidadeConfiguration : EntityTypeConfiguration<PerfilFuncionalidade>
    {
        public PerfilFuncionalidadeConfiguration()
        {
            ToTable("PerfilFuncionalidade", "Sigim");

            Property(l => l.Id)
               .HasColumnName("codigo");

            Property(l => l.PerfilId)
                .HasColumnName("perfil");

            //HasOptional<Perfil>(l => l.Perfil)
            //    .WithMany(l => l.ListaFuncionalidade)
            //    .HasForeignKey(l => l.PerfilId);

            Property(l => l.Funcionalidade)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnOrder(2);

            HasRequired(l => l.Perfil)
               .WithMany(l => l.ListaFuncionalidade);

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