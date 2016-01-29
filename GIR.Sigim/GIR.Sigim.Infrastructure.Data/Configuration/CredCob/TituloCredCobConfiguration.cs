using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.CredCob;
using GIR.Sigim.Domain.Entity.Comercial;

namespace GIR.Sigim.Infrastructure.Data.Configuration.CredCob
{
    public class TituloCredCobConfiguration : EntityTypeConfiguration<TituloCredCob>
    {
        public TituloCredCobConfiguration()
        {
            ToTable("Titulo", "CredCob");

            Property(l => l.Id)
                .HasColumnName("codigo")
                .HasColumnOrder(1);

            Property(l => l.ContratoId)
                .HasColumnName("contrato")
                .HasColumnOrder(2);

            HasRequired<ContratoComercial>(l => l.Contrato)
                .WithMany(c => c.ListaTituloCredCob)
                .HasForeignKey(l => l.ContratoId);

            Property(l => l.Situacao)
                .IsRequired()
                .HasColumnType("char")
                .HasMaxLength(1)
                .HasColumnName("situacao")
                .HasColumnOrder(4);

            Property(l => l.DataVencimento)
                .IsRequired()
                .HasColumnName("dataVencimento")
                .HasColumnOrder(10);

            Property(l => l.DataPagamento)
                .HasColumnName("dataPagamento")
                .HasColumnOrder(11);

            Property(l => l.QtdIndice)
                .HasPrecision(18, 5)
                .HasColumnName("qtdIndice")
                .HasColumnOrder(16);

            Property(l => l.ValorBaixa)
                .HasPrecision(18, 5)
                .HasColumnName("valorBaixa")
                .HasColumnOrder(32);

            Property(l => l.ValorIndiceBase)
                .HasPrecision(18, 5)
                .HasColumnName("valorIndiceBase")
                .HasColumnOrder(37);

        }
    }
}
