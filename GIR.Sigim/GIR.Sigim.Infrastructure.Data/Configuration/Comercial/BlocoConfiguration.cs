using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration; 
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Comercial;

namespace GIR.Sigim.Infrastructure.Data.Configuration.Comercial
{
    public class BlocoConfiguration : EntityTypeConfiguration<Bloco>
    {
        public BlocoConfiguration()
        {
            ToTable("Bloco","Comercial");

            Property(l => l.Id)
                .HasColumnName("codigo")
                .HasColumnOrder(1);

            Property(l => l.EmpreendimentoId)
                .HasColumnName("empreendimento")
                .HasColumnOrder(2);

            HasRequired<Empreendimento>(l => l.Empreendimento)
                .WithMany(c => c.ListaBloco)
                .HasForeignKey(l => l.EmpreendimentoId);

            Property(l => l.Nome)
                .HasColumnName("nome")
                .HasMaxLength(50)
                .HasColumnOrder(3);

            Property(l => l.CodigoCentroCusto)
                .HasMaxLength(18)
                .HasColumnName("centroCusto")
                .HasColumnOrder(13);

            HasOptional<Domain.Entity.Financeiro.CentroCusto>(l => l.CentroCusto)
                .WithMany(c => c.ListaBloco)
                .HasForeignKey(l => l.CodigoCentroCusto);

        }

    }
}
