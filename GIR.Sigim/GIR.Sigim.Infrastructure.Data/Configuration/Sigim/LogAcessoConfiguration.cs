using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Sigim;

namespace GIR.Sigim.Infrastructure.Data.Configuration.Sigim
{
    public class LogAcessoConfiguration : EntityTypeConfiguration<LogAcesso>
    {
        public LogAcessoConfiguration()
        {
            ToTable("LogAcesso", "Sigim");

            Property(l => l.Id)
                .HasColumnName("codigo")
                .HasColumnOrder(1);

            Property(l => l.Data)
                .HasColumnName("data")
                .HasColumnOrder(2);

            Property(l => l.Tipo)
                .HasColumnType("char")
                .HasMaxLength(3)
                .HasColumnName("tipo")
                .HasColumnOrder(3);

            Property(l => l.HostName)
                .HasMaxLength(200)
                .HasColumnName("hostName")
                .HasColumnOrder(4);

            Property(l => l.LoginUsuario)
                .IsRequired()
                .HasColumnType("nvarchar")
                .HasMaxLength(128)
                .HasColumnName("usuario")
                .HasColumnOrder(5);

            Property(l => l.Sistema)
                .HasMaxLength(100)
                .HasColumnName("sistema")
                .HasColumnOrder(6);

            Property(l => l.Versao)
                .HasMaxLength(50)
                .HasColumnName("versaoSistema")
                .HasColumnOrder(7);

            Property(l => l.NomeBaseDados)
                .HasMaxLength(50)
                .HasColumnName("nomeBaseDados")
                .HasColumnOrder(8);

            Property(l => l.Servidor)
                .HasMaxLength(100)
                .HasColumnName("servidor")
                .HasColumnOrder(9);
        }
    }
}