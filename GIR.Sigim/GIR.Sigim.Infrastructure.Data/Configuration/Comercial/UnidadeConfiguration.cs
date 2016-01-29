using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration; 
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Comercial;

namespace GIR.Sigim.Infrastructure.Data.Configuration.Comercial
{
    public class UnidadeConfiguration : EntityTypeConfiguration<Unidade>
    {
        public UnidadeConfiguration()
        {
            ToTable("Unidade","Comercial");

            Property(l => l.Id)
                .HasColumnName("codigo")
                .HasColumnOrder(1);

            Property(l => l.Descricao)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("descricao")
                .HasColumnOrder(2);

            Property(l => l.EmpreendimentoId)
                .HasColumnName("empreedimento")
                .HasColumnOrder(3);

            HasOptional<Empreendimento>(l => l.Empreendimento)
                .WithMany(c => c.ListaUnidade)
                .HasForeignKey(l => l.EmpreendimentoId);

            Property(l => l.BlocoId)
                .HasColumnName("bloco")
                .HasColumnOrder(4);

            HasOptional<Bloco>(l => l.Bloco)
                .WithMany(c => c.ListaUnidade)
                .HasForeignKey(l => l.BlocoId);

        }

    }
}
