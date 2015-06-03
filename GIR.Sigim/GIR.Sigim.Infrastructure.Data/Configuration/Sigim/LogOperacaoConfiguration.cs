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
    public class LogOperacaoConfiguration : EntityTypeConfiguration<LogOperacao>
    {
        public LogOperacaoConfiguration()
        {
            ToTable("LogOperacao", "Sigim");

            Property(l => l.Id)
                .HasColumnName("codigo")
                .HasColumnOrder(1);

            Property(l => l.Data)
                .HasColumnName("data")
                .HasColumnOrder(2);

            Property(l => l.LoginUsuario)
                .IsRequired()
                .HasColumnType("nvarchar")
                .HasMaxLength(128)
                .HasColumnName("usuario")
                .HasColumnOrder(3);

            Property(l => l.Descricao)
                .HasMaxLength(100)
                .HasColumnName("descricaoOperacao")
                .HasColumnOrder(4);

            Property(l => l.NomeRotina)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("nomeRotina")
                .HasColumnOrder(5);

            Property(l => l.NomeTabela)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("nomeTabela")
                .HasColumnOrder(6);

            Property(l => l.NomeComando)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("nomeComando")
                .HasColumnOrder(7);

            Property(l => l.Dados)
                .HasColumnType("xml")
                .HasColumnName("dados")
                .HasColumnOrder(8);

            Property(l => l.HostName)
                .HasMaxLength(50)
                .HasColumnName("hostName")
                .HasColumnOrder(9);
        }
    }
}