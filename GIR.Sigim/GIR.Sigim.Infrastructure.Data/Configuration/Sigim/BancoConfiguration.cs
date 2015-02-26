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
    public class BancoConfiguration : EntityTypeConfiguration<Banco>
    {
        public BancoConfiguration()
        {
            ToTable("Banco", "Sigim");

            Property(l => l.Id)
                .HasColumnName("codigoBC")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None)
                .HasColumnOrder(1);

            Property(l => l.Nome)
                .HasMaxLength(50)
                .HasColumnName("nome")
                .HasColumnOrder(2);

            Property(l => l.Situacao)
                .HasColumnType("char")
                .HasMaxLength(1)
                .HasColumnName("situacao")
                .HasColumnOrder(3);

            Property(l => l.NumeroRemessa)
                .IsOptional()
                .HasColumnName("numeroRemessa")
                .HasColumnOrder(4);

            Property(l => l.InterfaceEletronica)
                .IsOptional()
                .HasColumnName("interfaceEletronica")
                .HasColumnOrder(5);

            Property(l => l.NumeroRemessaPagamento)
                .IsOptional()
                .HasColumnName("numeroRemessaPagamento")
                .HasColumnOrder(6);

            Ignore(l => l.Ativo);

            HasMany(l => l.ListaBancoLayout).WithOptional(l => l.Banco);
        }
    }
}