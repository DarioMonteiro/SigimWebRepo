using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Orcamento;

namespace GIR.Sigim.Infrastructure.Data.Configuration.Orcamento
{
    public class OrcamentoComposicaoItemConfiguration : EntityTypeConfiguration<OrcamentoComposicaoItem>
    {
        public OrcamentoComposicaoItemConfiguration()
        {
            ToTable("OrcamentoComposicaoItem", "Orcamento");

            Property(l => l.Id)
                .HasColumnName("codigo")
                .HasColumnOrder(1);

            Property(l => l.OrcamentoComposicaoId)
                .HasColumnName("orcamentoComposicao")
                .HasColumnOrder(2);

            HasRequired(l => l.OrcamentoComposicao)
                .WithMany(l => l.ListaOrcamentoComposicaoItem)
                .HasForeignKey(l => l.OrcamentoComposicaoId);

            Property(l => l.MaterialId)
                .HasColumnName("material")
                .HasColumnOrder(3);

            HasOptional(l => l.Material)
                .WithMany(l => l.ListaOrcamentoComposicaoItem)
                .HasForeignKey(l => l.MaterialId);

            Property(l => l.Consumo)
                .HasPrecision(18, 5)
                .HasColumnName("consumo")
                .HasColumnOrder(4);

            Property(l => l.PercentualPerda)
                .HasPrecision(18, 5)
                .HasColumnName("percentualPerda")
                .HasColumnOrder(5);

            Property(l => l.Preco)
                .HasPrecision(18, 5)
                .HasColumnName("preco")
                .HasColumnOrder(6);

            Property(l => l.EhControlado)
                .HasColumnName("controlado")
                .HasColumnOrder(7);
        }
    }
}