using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Sigim;
using GIR.Sigim.Domain.Entity.Orcamento;

namespace GIR.Sigim.Infrastructure.Data.Configuration.Sigim
{
    public class ComposicaoConfiguration : EntityTypeConfiguration<Composicao>
    {
        public ComposicaoConfiguration()
        {
            ToTable("Composicao", "Sigim");

            Property(l => l.Id)
                .HasColumnName("codigo")
                .HasColumnOrder(1);

            Property(l => l.Descricao)
                .HasMaxLength(400)
                .HasColumnName("descricao")
                .HasColumnOrder(2);

            Property(l => l.UnidadeMedidaSigla)
                .HasMaxLength(6)
                .HasColumnName("unidadeMedida")
                .HasColumnOrder(3);

            HasMany<OrcamentoComposicao>(l => l.ListaOrcamentoComposicao)
                .WithOptional(c => c.Composicao)
                .HasForeignKey(c => c.ComposicaoId);

            HasMany<OrcamentoInsumoRequisitado>(l => l.ListaOrcamentoInsumoRequisitado)
                .WithOptional(c => c.Composicao)
                .HasForeignKey(c => c.ComposicaoId);
        }
    }
}