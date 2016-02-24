using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Sigim;

namespace GIR.Sigim.Infrastructure.Data.Configuration.Sigim
{
    public class CotacaoValoresConfiguration : EntityTypeConfiguration<CotacaoValores>
    {
        public CotacaoValoresConfiguration()
        {
            ToTable("CotacaoValores", "Sigim");

            Ignore(l => l.Id);

            HasKey(l => new { l.IndiceFinanceiroId, l.Codigo});

            Property(l => l.IndiceFinanceiroId)
                .HasColumnName("indiceFinanceiro")
                .IsRequired()
                .HasColumnOrder(1);

            HasRequired<IndiceFinanceiro>(l => l.IndiceFinanceiro)
                .WithMany(c => c.ListaCotacaoValores)
                .HasForeignKey(l => l.IndiceFinanceiroId);

            Property(l => l.Codigo)
                .HasColumnName("codigo")
                .IsRequired()
                .HasColumnOrder(2);

            Property(l => l.Data)
                .HasColumnName("data")
                .HasColumnOrder(3);

            Property(l => l.Valor)
                .HasColumnName("valor")
                .HasPrecision(18,5)
                .HasColumnOrder(4);

            Property(l => l.Variacao)
                .HasColumnName("variacao")
                .HasPrecision(18, 5)
                .HasColumnOrder(5);

            Property(l => l.MesAno)
                .HasColumnName("mesAno")
                .HasMaxLength(7)
                .HasColumnOrder(6);

            Property(l => l.DataCadastro)
                .HasColumnName("dataCadastro")
                .HasColumnOrder(7);

            Property(l => l.UsuarioCadastro)
                .HasColumnName("usuarioCadastro")
                .HasMaxLength(50)
                .HasColumnOrder(8);

            Property(l => l.DataAlteracao)
                .HasColumnName("dataAlteracao")
                .HasColumnOrder(9);

            Property(l => l.UsuarioAlteracao)
                .HasColumnName("usuarioAlteracao")
                .HasMaxLength(50)
                .HasColumnOrder(9);

        }

    }
}
