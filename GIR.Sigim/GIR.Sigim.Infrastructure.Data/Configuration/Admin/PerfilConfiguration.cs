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

            Property(l => l.Descricao)
                .IsRequired()
                .HasMaxLength(100);

            HasRequired(l => l.Modulo).WithMany(l => l.ListaPerfil);

            HasMany(l => l.ListaFuncionalidade).WithMany(l => l.ListaPerfil)
                .Map(m =>
                {
                    m.ToTable("PerfilFuncionalidade", "Sigim");
                    m.MapLeftKey("PerfilId");
                    m.MapRightKey("FuncionalidadeId");
                });
        }
    }
}