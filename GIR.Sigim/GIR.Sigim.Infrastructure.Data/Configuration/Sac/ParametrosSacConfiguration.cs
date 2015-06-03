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
    public class ParametrosSacConfiguration : EntityTypeConfiguration<ParametrosSac>
    {
        public ParametrosSacConfiguration()
        {
            ToTable("Parametros", "Sac");

            Property(l => l.Id)
                .HasColumnName("codigo")
                .HasColumnOrder(1);

            Property(l => l.ClienteId)
                .HasColumnName("cliente")
                .HasColumnOrder(2);

            //HasOptional(l => l.Cliente)
            //    .WithMany(l => l.ListaParametrosSac);
            
            Property(l => l.Mascara)
                .HasMaxLength(18)
                .HasColumnName("mascara")
                .HasColumnOrder(4);

            Property(l => l.IconeRelatorio)
                .HasColumnType("image")
                .HasColumnName("iconeRelatorio")
                .HasColumnOrder(5);
            
            Property(l => l.PrazoAvaliacao )
                .HasColumnName("prazoAvaliacao")
                .HasColumnOrder(9);

            Property(l => l.PrazoConclusao)
                .HasColumnName("prazoConclusao")
                .HasColumnOrder(10);

            //Property(l => l.HabilitaSSL)
            //    .HasColumnName("habilitaSSL")
            //    .HasColumnOrder(17);
        }
    }
}