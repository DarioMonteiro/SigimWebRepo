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
    public class VendaConfiguration : EntityTypeConfiguration<Venda>
    {
        public VendaConfiguration()
        {
            ToTable("Venda","Comercial");

            Ignore(l => l.Id);

            HasKey(l => new {l.ContratoId});

            Property(l => l.ContratoId)
                .HasColumnName("contrato")
                .HasColumnOrder(1);

             //HasRequired<ContratoComercial>(l => l.Contrato)
             //   .WithMany(c => c.ListaVendaSerie)
             //   .HasForeignKey(l => l.ContratoId);

             Property(l => l.DataVenda)
                .HasColumnName("dataVenda")
                .IsRequired()
                .HasColumnOrder(2);

             Property(l => l.TabelaVendaId)
                .HasColumnName("tabelaVenda")
                .IsRequired()
                .HasColumnOrder(3);

             HasRequired<TabelaVenda>(l => l.TabelaVenda)
                .WithMany(c => c.ListaVenda)
                .HasForeignKey(l => l.TabelaVendaId);

             Property(l => l.PrecoTabela)
                .HasColumnName("precoTabela")
                .HasPrecision(18,2)
                .IsRequired()
                .HasColumnOrder(4);

             Property(l => l.ValorDesconto)
                .HasColumnName("valorDesconto")
                .HasPrecision(18,2)
                .HasColumnOrder(5);

            Property(l => l.PrecoPraticado)
                .HasColumnName("precoPraticado")
                .HasPrecision(18,2)
                .HasColumnOrder(6);

             Property(l => l.IndiceFinanceiroId)
                .HasColumnName("indiceFinanceiro")
                .IsRequired()
                .HasColumnOrder(7);

             HasRequired<IndiceFinanceiro>(l => l.IndiceFinanceiro)
                .WithMany(c => c.ListaVendaIndiceFinanceiro)
                .HasForeignKey(l => l.IndiceFinanceiroId);

            Property(l => l.CotacaoIndiceFinanceiro)
                .HasColumnName("cotacaoIndiceFinanceiro")
                .HasPrecision(18,5)
                .IsRequired()
                .HasColumnOrder(8);

            Property(l => l.DataBaseIndiceFinanceiro)
                .HasColumnName("dataBaseIndiceFinanceiro")
                .IsRequired()
                .HasColumnOrder(9);

             Property(l => l.DataAssinaturaAgenda)
                .HasColumnName("dataAssinaturaAgenda")
                .HasColumnOrder(10);

            Property(l => l.HoraAssinaturaAgenda)
                .HasColumnName("horaAssinaturaAgenda")
                .HasColumnOrder(11);

            Property(l => l.DataAssinatura)
                .HasColumnName("dataAssinatura")
                .HasColumnOrder(12);

            Property(l => l.HoraAssinatura)
                .HasColumnName("horaAssinatura")
                .HasColumnOrder(13);

            Property(l => l.NumeroCartorio)
                .HasColumnName("numeroCartorio")
                .HasColumnOrder(14);

            Property(l => l.NumeroLivroCartorio)
                .HasColumnName("numeroLivroCartorio")
                .HasColumnOrder(15);

            Property(l => l.NumeroFolhaLivroCartorio)
                .HasColumnName("numeroFolhaLivroCartorio")
                .HasColumnOrder(16);

            Property(l => l.FormaVenda)
                .HasColumnName("formaVenda")
                .HasColumnOrder(17);

            Property(l => l.FormaContrato)
                .HasColumnName("formaContrato")
                .HasColumnOrder(18);

             Property(l => l.ContaCorrenteId)
                .HasColumnName("contaCorrente")
                .HasColumnOrder(19);

             HasRequired<ContaCorrente>(l => l.ContaCorrente)
                .WithMany(c => c.ListaVenda)
                .HasForeignKey(l => l.ContaCorrenteId);

            Property(l => l.DataCadastramento)
                .HasColumnName("dataCadastramento")
                .HasColumnOrder(20);

            Property(l => l.DataQuitacao)
                .HasColumnName("dataQuitacao")
                .HasColumnOrder(21);

            Property(l => l.DataCancelamento)
                .HasColumnName("dataCancelamento")
                .HasColumnOrder(22);

            Property(l => l.Aprovado)
                .HasColumnName("aprovado")
                .HasColumnOrder(23);

            Property(l => l.UsuarioAprovacao)
                .HasColumnName("usuarioAprovacao")
                .HasColumnOrder(24);

             Property(l => l.DataAprovacao)
                .HasColumnName("dataAprovacao")
                .HasColumnOrder(25);

            Property(l => l.PrecoBaseComissao)
                .HasColumnName("precoBaseComissao")
                .HasPrecision(18,2)
                .HasColumnOrder(26);

             Property(l => l.MatrizId)
                .HasColumnName("matriz")
                .HasColumnOrder(27);

            Property(l => l.CorretorMatrizId)
                .HasColumnName("corretorMatriz")
                .HasColumnOrder(28);

            Property(l => l.ValorTotalComissao)
                .HasColumnName("valorTotalComissao")
                .HasPrecision(18,2)
                .HasColumnOrder(29);

        }
    }
}
