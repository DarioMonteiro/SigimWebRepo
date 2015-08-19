using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Financeiro;
using GIR.Sigim.Domain.Entity.Sigim;

namespace GIR.Sigim.Infrastructure.Data.Configuration.Financeiro
{
    public class TaxaAdministracaoConfiguration : EntityTypeConfiguration<TaxaAdministracao>
    {
        public TaxaAdministracaoConfiguration()
        {
            ToTable("TaxaAdministracao", "Financeiro");

            Property(l => l.Id)
                .HasColumnName("codigo")
                .HasColumnOrder(1);

            Property(l => l.CentroCustoId)
                .HasColumnName("centroCusto")
                .HasColumnOrder(2);

             HasOptional<CentroCusto>(l => l.CentroCusto)
                 .WithMany(l => l.ListaTaxaAdministracao)
                 .HasForeignKey(l => l.CentroCustoId);

             Property(l => l.ClienteId)
                .HasColumnName("cliente")
                .HasColumnOrder(3);

             HasRequired<ClienteFornecedor>(l => l.Cliente)
                 .WithMany(l => l.ListaTaxaAdministracao)
                 .HasForeignKey(l => l.ClienteId);

             Property(l => l.ClasseId)
                 .HasColumnName("classe")
                 .HasColumnOrder(4);

             HasOptional<Classe>(l => l.Classe)
                 .WithMany(l => l.ListaTaxaAdministracao)
                 .HasForeignKey(l => l.ClasseId);

             Property(l => l.Percentual)
                .HasColumnName("percentual")
                .HasColumnOrder(5);
        }
    }
}
