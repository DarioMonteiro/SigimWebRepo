using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Financeiro;

namespace GIR.Sigim.Infrastructure.Data.Configuration.Financeiro
{
    public class TipoCompromissoConfiguration : EntityTypeConfiguration<TipoCompromisso>
    {
        public TipoCompromissoConfiguration()
        {
            ToTable("TipoCompromisso", "Financeiro");

            Property(l => l.Id)
                .HasColumnName("codigo")
                .HasColumnOrder(1);

            Property(l => l.Descricao)
                .IsRequired()
                .HasMaxLength(25)
                .HasColumnName("descricao")
                .HasColumnOrder(2);

            Property(l => l.GeraTitulo)
                .HasColumnName("geraTitulo")
                .HasColumnOrder(3);

            Property(l => l.TipoPagar)
                .HasColumnName("tipoPagar")
                .HasColumnOrder(4);

            Property(l => l.TipoReceber)
                .HasColumnName("tipoReceber")
                .HasColumnOrder(5);

            HasMany(l => l.ListaParametrosOrdemCompra)
                .WithOptional(l => l.TipoCompromissoFrete);
        }
    }
}