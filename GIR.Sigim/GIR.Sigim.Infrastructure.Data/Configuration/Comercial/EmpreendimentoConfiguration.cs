using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using GIR.Sigim.Domain.Entity.Comercial;

namespace GIR.Sigim.Infrastructure.Data.Configuration.Comercial
{
    public class EmpreendimentoConfiguration : EntityTypeConfiguration<Empreendimento>
    {
        public EmpreendimentoConfiguration()
        {
            ToTable("Empreendimento","Comercial");

            Property(l => l.Id)
                .HasColumnName("codigo")
                .HasColumnOrder(1);

            Property(l => l.Nome)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("nome")
                .HasColumnOrder(2);

            Property(l => l.EnderecoId)
                .HasColumnName("endereco")
                .IsRequired()
                .HasColumnOrder(3);

            Property(l => l.IncorporadorId )
                .HasColumnName("incorporador")
                .IsRequired()
                .HasColumnOrder(4);

            HasRequired<Incorporador>(l => l.Incorporador)
               .WithMany(c => c.ListaEmpreendimento)
               .HasForeignKey(l => l.IncorporadorId);

        }
    }
}
