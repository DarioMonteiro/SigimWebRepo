using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Financeiro;

namespace GIR.Sigim.Infrastructure.Data.Configuration.Financeiro
{
    public class CentroCustoEmpresaConfiguration : EntityTypeConfiguration<CentroCustoEmpresa>
    {
        public CentroCustoEmpresaConfiguration()
        {
            ToTable("CentroCustoEmpresa", "Financeiro");

            Property(l => l.Id)
                .HasColumnName("codigo")
                .HasColumnOrder(1);

            Property(l => l.CodigoCentroCusto)
                .HasColumnName("centroCusto")
                .HasColumnOrder(2);

            HasRequired(l => l.CentroCusto)
                .WithMany(l => l.ListaCentroCustoEmpresa)
                .HasForeignKey(l => l.CodigoCentroCusto);

            Property(l => l.ClienteId)
                .HasColumnName("cliente")
                .HasColumnOrder(3);

            HasRequired(l => l.Cliente)
                .WithMany(l => l.ListaCentroCustoEmpresa)
                .HasForeignKey(l => l.ClienteId);

            Property(l => l.EnderecoEntrega)
                .HasMaxLength(80)
                .HasColumnName("enderecoEntrega")
                .HasColumnOrder(4);

            Property(l => l.EnderecoCobranca)
                .HasMaxLength(80)
                .HasColumnName("enderecoCobranca")
                .HasColumnOrder(5);

            Property(l => l.ResponsavelObra)
                .HasMaxLength(80)
                .HasColumnName("responsavelObra")
                .HasColumnOrder(6);

            Property(l => l.IconeRelatorio)
                .HasColumnType("image")
                .HasColumnName("iconeRelatorio")
                .HasColumnOrder(7);

            Property(l => l.CodigoEnderecoConstrucompras)
                .HasColumnName("codigoEnderecoConstrucompras")
                .HasColumnOrder(8);

            Property(l => l.CodigoObraConstrucompras)
                .HasColumnName("codigoObraConstrucompras")
                .HasColumnOrder(9);

            Property(l => l.CaminhoConstrucompras)
                .HasMaxLength(200)
                .HasColumnName("caminhoConstrucompras")
                .HasColumnOrder(10);
            
            Property(l => l.PracaPagamento)
                .HasMaxLength(20)
                .HasColumnName("pracaPagamento")
                .HasColumnOrder(11);

            Property(l => l.EhClasseOrcamento)
                .HasColumnName("classeOrcamento")
                .HasColumnOrder(12);
        }
    }
}