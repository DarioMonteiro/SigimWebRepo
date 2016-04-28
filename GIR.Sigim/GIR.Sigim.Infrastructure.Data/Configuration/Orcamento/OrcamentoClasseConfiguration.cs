using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Orcamento;
using GIR.Sigim.Domain.Entity.Financeiro;

namespace GIR.Sigim.Infrastructure.Data.Configuration.Orcamento
{
    public class OrcamentoClasseConfiguration : EntityTypeConfiguration<OrcamentoClasse>
    {
        public OrcamentoClasseConfiguration()
        {
            ToTable("OrcamentoClasse", "Orcamento");

            Property(l => l.Id)
                .HasColumnName("codigo")
                .HasColumnOrder(1);

            Property(l => l.OrcamentoId)
                .IsRequired()
                .HasColumnName("orcamento")
                .HasColumnOrder(2);

            HasRequired<GIR.Sigim.Domain.Entity.Orcamento.Orcamento>(l => l.Orcamento)
                .WithMany(l => l.ListaOrcamentoClasse)
                .HasForeignKey(l => l.OrcamentoId);

            Property(l => l.ClasseCodigo)
                .IsRequired()
                .HasColumnName("classe")
                .HasMaxLength(18)
                .HasColumnOrder(3);

            HasRequired<Classe>(l => l.Classe)
                .WithMany(l => l.ListaOrcamentoClasse)
                .HasForeignKey(l => l.ClasseCodigo);

            Property(l => l.Fechada)
                .HasColumnName("fechada")
                .HasColumnType("bit")
                .HasColumnOrder(4);

            Property(l => l.Controlada)
                .HasColumnName("controlada")
                .HasColumnType("bit")
                .HasColumnOrder(5);
        }
    }
}
