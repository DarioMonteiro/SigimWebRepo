using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.OrdemCompra;

namespace GIR.Sigim.Infrastructure.Data.Configuration.OrdemCompra
{
    public class EmpresaCentroCustoConfiguration : EntityTypeConfiguration<EmpresaCentroCusto>
    {
        public EmpresaCentroCustoConfiguration()
        {
            ToTable("EmpresaCentroCusto", "OrdemCompra");

            Property(l => l.Id)
                .HasColumnName("codigo")
                .HasColumnOrder(1);

            Property(l => l.ClienteId)
                .HasColumnName("cliente")
                .HasColumnOrder(2);

            HasRequired(l => l.Cliente)
                .WithMany(l => l.ListaEmpresaOrdemCompra)
                .HasForeignKey(l => l.ClienteId);

            Property(l => l.CodigoCentroCusto)
                .HasColumnName("centroCusto")
                .HasColumnOrder(3);

            HasRequired(l => l.CentroCusto)
                .WithMany(l => l.ListaEmpresaOrdemCompra)
                .HasForeignKey(l => l.CodigoCentroCusto);

            Property(l => l.EnderecoEntrega)
                .HasMaxLength(80)
                .HasColumnName("enderecoEntrega")
                .HasColumnOrder(4);

            Property(l => l.EnderecoCobranca)
                .HasMaxLength(80)
                .HasColumnName("enderecoCobranca")
                .HasColumnOrder(5);
        }
    }
}