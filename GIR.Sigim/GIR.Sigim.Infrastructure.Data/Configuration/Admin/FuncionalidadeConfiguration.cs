using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Admin;

namespace GIR.Sigim.Infrastructure.Data.Configuration.Admin
{
    public class FuncionalidadeConfiguration : EntityTypeConfiguration<Funcionalidade>
    {
        public FuncionalidadeConfiguration()
        {
            ToTable("Funcionalidade", "Sigim");

            Property(l => l.Descricao)
                .IsRequired()
                .HasMaxLength(100);

            Property(l => l.ChaveAcesso)
                .IsRequired()
                .HasMaxLength(100);

            Property(l => l.Ativo)
                .IsRequired();

            HasRequired(l => l.Modulo).WithMany(l => l.ListaFuncionalidade);
        }
    }
}