using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration; 
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Comercial;
using GIR.Sigim.Domain.Entity.Sigim;

namespace GIR.Sigim.Infrastructure.Data.Configuration.Comercial
{
    public class TabelaVendaConfiguration : EntityTypeConfiguration<TabelaVenda>
    {
        public TabelaVendaConfiguration()
        {
            ToTable("TabelaVenda", "Comercial");

        //    public int BlocoId { get; set; }
        //    public String Nome { get; set; }
        //public String Situacao { get; set; }
        //public Nullable<DateTime> DataElaboracao { get; set; }
        //public String Observacao { get; set; }
        //public Decimal PrecoReferencia  { get; set; }
        //public Decimal PercentualCorretora  { get; set; }
        //public Decimal PercentualCorretor  { get; set; }

            Property(l => l.Id)
              .HasColumnName("codigo")
              .HasColumnOrder(1);

            Property(l => l.BlocoId)
               .HasColumnName("bloco")
               .IsRequired()
               .HasColumnOrder(2);

            HasRequired<Bloco>(l => l.Bloco)
                .WithMany(c => c.ListaTabelaVenda)
                .HasForeignKey(l => l.BlocoId);

            Property(l => l.Nome)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("nome")
                .HasColumnOrder(3);

            Property(l => l.DataElaboracao)
                .HasColumnName("dataElaboracao")
                .HasColumnOrder(4);

            Property(l => l.Observacao)
                .HasMaxLength(3000)
                .HasColumnName("observacao")
                .HasColumnOrder(5);

            Property(l => l.PrecoReferencia)
               .HasColumnName("precoReferencia")
               .HasPrecision(18, 2)
               .HasColumnOrder(6);

            Property(l => l.PercentualCorretor )
               .HasColumnName("percentualCorretor")
               .HasPrecision(18, 5)
               .HasColumnOrder(7);

            Property(l => l.PercentualCorretora)
               .HasColumnName("percentualCorretora")
               .HasPrecision(18, 5)
               .HasColumnOrder(8);

        }
    }
}
