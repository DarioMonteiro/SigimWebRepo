using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Sac;

namespace GIR.Sigim.Infrastructure.Data.Configuration.Sac
{
    public class ParametrosEmailSacConfiguration : EntityTypeConfiguration<ParametrosEmailSac>
    {
        public ParametrosEmailSacConfiguration()
        {
            ToTable("ParametrosEmail", "Sac");

            Property(l => l.Id)
                .HasColumnName("codigo")
                .HasColumnOrder(1);

            Property(l => l.ParametrosId)
                .HasColumnName("parametros")
                .HasColumnOrder(2);

            //HasOptional<ParametrosSac>(l => l.Parametros)
            //   .WithMany(l => l.ListaParametrosEmailSac)
            //   .HasForeignKey(l => l.ParametrosId);

            Property(l => l.SetorId)
                .HasColumnName("setor")
                .HasColumnOrder(3);

            HasRequired<Setor>(l => l.Setor)
               .WithMany(l => l.ListaParametrosEmailSac)
               .HasForeignKey(l => l.SetorId);
            
            Property(l => l.Tipo)
                .HasColumnName("tipo")
                .HasColumnOrder(4);
            
            Property(l => l.Email)
                .HasColumnName("email")
                .HasMaxLength(100)
                .IsRequired() 
                .HasColumnOrder(5);

            Property(l => l.Anexo)
                .HasColumnName("anexo")
                .HasColumnType("bit")
                .HasColumnOrder(6);
        }
    }

}