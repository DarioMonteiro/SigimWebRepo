using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Estoque;

namespace GIR.Sigim.Infrastructure.Data.Configuration.Financeiro
{
    public class MovimentoConfiguration : EntityTypeConfiguration<Movimento>
    {
        public MovimentoConfiguration()
        {
            ToTable("Movimento", "Estoque");

            Property(l => l.Id)
                .HasColumnName("codigo");

            Property(l => l.EntradaMaterialId)
                .HasColumnName("entradaMaterial");

            HasOptional(l => l.EntradaMaterial)
                .WithMany(l => l.ListaMovimentoEstoque)
                .HasForeignKey(l => l.EntradaMaterialId);
        }
    }
}