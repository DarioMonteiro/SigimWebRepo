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
    public class ClienteFornecedorConfiguration : EntityTypeConfiguration<ClienteFornecedor>
    {
        public ClienteFornecedorConfiguration()
        {
            ToTable("ClienteFornecedor", "Sigim");

            Property(l => l.Id)
                .HasColumnName("codigo")
                .HasColumnOrder(1);

            Property(l => l.Nome)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("nome")
                .HasColumnOrder(2);

            Property(l => l.Situacao)
                .IsRequired()
                .HasColumnType("char")
                .HasMaxLength(1)
                .HasColumnName("situacao");

            Ignore(l => l.Ativo);

            HasMany(l => l.ListaParametrosOrdemCompra)
                .WithOptional(l => l.Cliente);
        }
    }
}