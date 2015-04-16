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

            Property(l => l.TipoPessoa)
                .IsRequired()
                .HasColumnType("char")
                .HasMaxLength(1)
                .HasColumnName("tipoPessoa")
                .HasColumnOrder(5);
            
            Property(l => l.Situacao)
                .IsRequired()
                .HasColumnType("char")
                .HasMaxLength(1)
                .HasColumnName("situacao")
                .HasColumnOrder(6);

            Property(l => l.TipoCliente)
                .HasColumnType("char")
                .HasMaxLength(1)
                .HasColumnName("tipoCliente")
                .HasColumnOrder(7);

            Property(l => l.ClienteAPagar)
                .HasColumnType("char")
                .HasMaxLength(1)
                .HasColumnName("clienteAPagar")
                .HasColumnOrder(11);

            Property(l => l.ClienteAReceber)
                .HasColumnType("char")
                .HasMaxLength(1)
                .HasColumnName("clienteAReceber")
                .HasColumnOrder(12);

            Property(l => l.ClienteOrdemCompra)
                .HasColumnType("char")
                .HasMaxLength(1)
                .HasColumnName("clienteOrdemCompra")
                .HasColumnOrder(13);

            Property(l => l.ClienteContrato)
                .HasColumnType("char")
                .HasMaxLength(1)
                .HasColumnName("clienteContrato")
                .HasColumnOrder(14);

            Property(l => l.ClienteAluguel)
                .HasColumnType("char")
                .HasMaxLength(1)
                .HasColumnName("clienteAluguel")
                .HasColumnOrder(21);

            Property(l => l.ClienteEmpreitada)
                .HasColumnType("char")
                .HasMaxLength(1)
                .HasColumnName("clienteEmpreitada")
                .HasColumnOrder(24);

            Ignore(l => l.Ativo);

            HasMany(l => l.ListaParametrosOrdemCompra)
                .WithOptional(c => c.Cliente);
        }
    }
}