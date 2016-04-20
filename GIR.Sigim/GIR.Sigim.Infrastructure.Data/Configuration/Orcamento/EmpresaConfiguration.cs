using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Orcamento;
using GIR.Sigim.Domain.Entity.Sigim;

namespace GIR.Sigim.Infrastructure.Data.Configuration.Orcamento
{
    public class EmpresaConfiguration : EntityTypeConfiguration<Empresa>
    {

        public EmpresaConfiguration()
        {
            ToTable("Obra", "Empresa");

            Property(l => l.Id)
                .HasColumnName("codigo")
                .HasColumnOrder(1);

            Property(l => l.Numero)
               .HasMaxLength(18)
               .HasColumnName("numero")
               .HasColumnOrder(2);

            Property(l => l.ClienteFornecedorId)
                .HasColumnName("clienteFornecedor")
                .HasColumnOrder(3);

            HasRequired<ClienteFornecedor>(l => l.ClienteFornecedor)
                .WithMany(l => l.ListaEmpresa)
                .HasForeignKey(l => l.ClienteFornecedorId);

        }

    }
}
