using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Financeiro;
using GIR.Sigim.Domain.Entity.Sigim;

namespace GIR.Sigim.Infrastructure.Data.Configuration.Financeiro
{
    public class MovimentoFinanceiroConfiguration : EntityTypeConfiguration<MovimentoFinanceiro>
    {
        public MovimentoFinanceiroConfiguration()
        {
            ToTable("Movimento", "Financeiro");

            Property(l => l.Id)
                .HasColumnName("codigo")
                .HasColumnOrder(1);

            Property(l => l.TipoMovimentoId)
                .IsRequired()
                .HasColumnName("tipoMovimento")
                .HasColumnOrder(2);

            HasRequired<TipoMovimento>(l => l.TipoMovimento)
                .WithMany(c => c.ListaMovimentoFinanceiro)
                .HasForeignKey(l => l.TipoMovimentoId);

            Property(l => l.DataMovimento)
                .HasColumnName("dataMovimento")
                .HasColumnOrder(3);

            Property(l => l.ContaCorrenteId)
                .HasColumnName("contaCorrente")
                .HasColumnOrder(4);

            HasOptional<ContaCorrente>(l => l.ContaCorrente)
                .WithMany(c => c.ListaMovimentoFinanceiro)
                .HasForeignKey(l => l.ContaCorrenteId);

            Property(l => l.CaixaId)
                .HasColumnName("caixa")
                .HasColumnOrder(5);

            HasOptional<Caixa>(l => l.Caixa)
                .WithMany(c => c.ListaMovimentoFinanceiro)
                .HasForeignKey(l => l.CaixaId);

            Property(l => l.Referencia)
                .HasColumnName("referencia")
                .HasMaxLength(100)
                .HasColumnOrder(6);

            Property(l => l.Documento)
                .HasColumnName("documento")
                .HasMaxLength(50)
                .HasColumnOrder(7);

            Property(l => l.Valor)
                .HasColumnName("valor")
                .HasPrecision(18, 5)
                .HasColumnOrder(8);

            Property(l => l.Situacao)
                .HasColumnName("situacao")
                .HasColumnType("char")
                .HasMaxLength(1)
                .HasColumnOrder(9);

            Property(l => l.UsuarioLancamento)
                .HasColumnName("usuarioLancamento")
                .HasMaxLength(128)
                .HasColumnOrder(10);

            Property(l => l.DataLancamento)
                .HasColumnName("dataLancamento")
                .HasColumnOrder(11);

            Property(l => l.UsuarioConferencia)
                .HasColumnName("usuarioConferencia")
                .HasMaxLength(128)
                .HasColumnOrder(12);

            Property(l => l.DataConferencia)
                .HasColumnName("dataConferencia")
                .HasColumnOrder(13);

            Property(l => l.UsuarioApropriacao)
                .HasColumnName("usuarioApropriacao")
                .HasMaxLength(128)
                .HasColumnOrder(14);

            Property(l => l.DataApropriacao)
                .HasColumnName("dataApropriacao")
                .HasColumnOrder(15);

            Property(l => l.MovimentoPaiId)
                .HasColumnName("movimentoPai")
                .HasColumnOrder(16);

            HasOptional<MovimentoFinanceiro>(l => l.MovimentoPai)
                .WithMany(l => l.ListaFilhos)
                .HasForeignKey(l => l.MovimentoPaiId);

            Property(l => l.MovimentoOposto)
                .HasColumnName("movimentoOposto")
                .HasColumnOrder(17);

            Property(l => l.BorderoTransferencia)
                .HasColumnName("borderoTransferencia")
                .HasColumnOrder(18);
        }
    }
}
