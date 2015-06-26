using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Sigim;
using GIR.Sigim.Domain.Entity.Contrato;

namespace GIR.Sigim.Infrastructure.Data.Configuration.Sigim
{
    public class NaturezaOperacaoConfiguration : EntityTypeConfiguration<NaturezaOperacao>
    {
        public NaturezaOperacaoConfiguration()
        {
            ToTable("NaturezaOperacao", "Sigim");

            Ignore(l => l.Id);

            HasKey(l => l.Codigo);

            Property(l => l.Codigo)
                .IsRequired()
                .HasMaxLength(10)
                .HasColumnName("codigo")
                .HasColumnOrder(1);

            Property(l => l.Descricao)
                .IsRequired()
                .HasColumnName("descricao")
                .HasMaxLength(50)
                .HasColumnOrder(2);

            HasMany<ContratoRetificacaoItemMedicao>(l => l.ListaContratoRetificacaoItemMedicao)
                .WithOptional(c => c.NaturezaOperacao)
                .HasForeignKey(c => c.NaturezaOperacaoCodigo);

        }
    }
}
