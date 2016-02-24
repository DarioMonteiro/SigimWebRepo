using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.CredCob;
using GIR.Sigim.Domain.Entity.Comercial;
using GIR.Sigim.Domain.Entity.Sigim;

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

            Property(l => l.Classe)
                .HasColumnName("classe")
                .HasColumnType("tinyint")
                .IsRequired()
                .HasColumnOrder(3);

            Property(l => l.Situacao)
                .IsRequired()
                .HasColumnType("char")
                .HasMaxLength(1)
                .HasColumnName("situacao")
                .HasColumnOrder(4);

            Property(l => l.Tipo)
                .IsRequired()
                .HasColumnType("char")
                .HasMaxLength(1)
                .HasColumnName("tipo")
                .HasColumnOrder(5);

            Property(l => l.Tipologia)
                .IsRequired()
                .HasColumnType("char")
                .HasMaxLength(1)
                .HasColumnName("tipologia")
                .HasColumnOrder(6);

            Property(l => l.DataCancelamento)
                .HasColumnName("dataCancelamento")
                .HasColumnOrder(7);

            Property(l => l.DataDesdobramento)
                .HasColumnName("dataDesdobramento")
                .HasColumnOrder(8);

            Property(l => l.DataEmissaoCobranca)
                .HasColumnName("dataEmissaoCobranca")
                .HasColumnOrder(9);

            Property(l => l.DataVencimento)
                .IsRequired()
                .HasColumnName("dataVencimento")
                .HasColumnOrder(10);

            Property(l => l.DataPagamento)
                .HasColumnName("dataPagamento")
                .HasColumnOrder(11);

            Property(l => l.MotivoBaixa)
                .HasColumnName("motivoBaixa")
                .HasMaxLength(50)
                .HasColumnOrder(12);

            Property(l => l.NumeroAgrupamentoId)
                .HasColumnName("numeroAgrupamento")
                .HasColumnOrder(13);

            Property(l => l.RescisaoId)
                .HasColumnName("rescisao")
                .HasColumnOrder(14);

            Property(l => l.Periodicidade)
                .HasColumnName("periodicidade")
                .HasColumnType("tinyint")
                .IsRequired()
                .HasColumnOrder(15);

            Property(l => l.QtdIndice)
                .IsRequired()
                .HasPrecision(18, 5)
                .HasColumnName("qtdIndice")
                .HasColumnOrder(16);

            Property(l => l.QtdIndiceAmortizacao)
                .HasPrecision(18, 5)
                .HasColumnName("qtdIndiceAmortizacao")
                .HasColumnOrder(17);

            Property(l => l.QtdIndiceJuros)
                .HasPrecision(18, 5)
                .HasColumnName("qtdIndiceJuros")
                .HasColumnOrder(18);

            Property(l => l.QtdIndiceJurosOriginal)
                .HasPrecision(18, 5)
                .HasColumnName("qtdIndiceJurosOriginal")
                .HasColumnOrder(19);

            Property(l => l.QtdIndiceOriginal)
                .HasPrecision(18, 5)
                .HasColumnName("qtdIndiceOriginal")
                .HasColumnOrder(20);

            Property(l => l.QtdIndiceAmortizacaoOriginal)
                .HasPrecision(18, 5)
                .HasColumnName("qtdIndiceAmortizacaoOriginal")
                .HasColumnOrder(21);

            Property(l => l.ContaCorrenteId)
                .HasColumnName("contaCorrente")
                .HasColumnOrder(22);

            Property(l => l.IndiceId)
                .HasColumnName("indice")
                .HasColumnOrder(23);

            HasOptional<IndiceFinanceiro>(l => l.Indice)
                .WithMany(c => c.ListaTituloCredCob)
                .HasForeignKey(l => l.IndiceId);

            Property(l => l.Serie)
                .HasColumnName("serie")
                .HasColumnOrder(24);

            Property(l => l.NumeroParcela)
                .HasMaxLength(500)
                .HasColumnName("numeroParcela")
                .HasColumnOrder(25);

            Property(l => l.VerbaCobrancaId)
                .HasColumnName("verbaCobranca")
                .HasColumnOrder(26);

            HasOptional<VerbaCobranca>(l => l.VerbaCobranca)
                .WithMany(c => c.ListaTituloCredCob)
                .HasForeignKey(l => l.VerbaCobrancaId);

            Property(l => l.SistemaOrigem)
                .HasColumnType("char")
                .HasMaxLength(2)
                .HasColumnName("sistemaOrigem")
                .HasColumnOrder(27);

            Property(l => l.TipoBaixa)
                .HasColumnType("char")
                .HasMaxLength(1)
                .HasColumnName("tipoBaixa")
                .HasColumnOrder(28);

            Property(l => l.TipoCancelamento)
                .HasColumnType("char")
                .HasMaxLength(1)
                .HasColumnName("tipoCancelamento")
                .HasColumnOrder(29);

            Property(l => l.TituloPrincipal)
                .HasColumnName("tituloPrincipal")
                .HasColumnOrder(30);

            Property(l => l.TituloPrincipalAgrupamento)
                .HasColumnName("tituloPrincipalAgrupamento")
                .HasColumnOrder(31);

            Property(l => l.ValorBaixa)
                .HasPrecision(18, 5)
                .HasColumnName("valorBaixa")
                .HasColumnOrder(32);

            Property(l => l.ValorDesconto)
                .HasPrecision(18, 5)
                .HasColumnName("valorDesconto")
                .HasColumnOrder(33);

            Property(l => l.ValorDescontoAntecipacao)
                .HasPrecision(18, 5)
                .HasColumnName("valorDescontoAntecipacao")
                .HasColumnOrder(34);

            Property(l => l.ValorDiferencaBaixa)
                .HasPrecision(18, 5)
                .HasColumnName("valorDiferencaBaixa")
                .HasColumnOrder(35);

            Property(l => l.ValorIndiceOriginal)
                .HasPrecision(18, 5)
                .HasColumnName("valorIndiceOriginal")
                .HasColumnOrder(36);

            Property(l => l.ValorIndiceBase)
                .IsRequired()
                .HasPrecision(18, 5)
                .HasColumnName("valorIndiceBase")
                .HasColumnOrder(37);

            Property(l => l.ValorIndicePagamento)
                .HasPrecision(18, 5)
                .HasColumnName("valorIndicePagamento")
                .HasColumnOrder(38);

            Property(l => l.ValorCorrecaoAtraso)
                .HasPrecision(18, 5)
                .HasColumnName("valorCorrecaoAtraso")
                .HasColumnOrder(39);

            Property(l => l.ValorCorrecaoProrrata)
                .HasPrecision(18, 5)
                .HasColumnName("valorCorrecaoProrrata")
                .HasColumnOrder(40);

            Property(l => l.ValorEncargos)
                .HasPrecision(18, 5)
                .HasColumnName("valorEncargos")
                .HasColumnOrder(41);

            Property(l => l.ValorMulta)
                .HasPrecision(18, 5)
                .HasColumnName("valorMulta")
                .HasColumnOrder(42);

            Property(l => l.ValorPercentualJuros)
                .HasPrecision(18, 5)
                .HasColumnName("valorPercentualJuros")
                .HasColumnOrder(43);

            Property(l => l.FormaRecebimentoId)
                .HasColumnName("formaRecebimento")
                .HasColumnOrder(44);

            Property(l => l.QtdDiasAtraso)
                .HasColumnName("qtdDiasAtraso")
                .HasColumnType("smallint")
                .HasColumnOrder(45);

            Property(l => l.ValorPresente)
                .HasPrecision(18, 5)
                .HasColumnName("valorPresente")
                .HasColumnOrder(46);

            Property(l => l.NumeroAgrupamentoRenegociacaoId)
                .HasColumnName("numeroAgrupamentoRenegociacao")
                .HasColumnOrder(47);

            Property(l => l.NumeroBoleto)
                .HasMaxLength(50)
                .HasColumnName("numeroBoleto")
                .HasColumnOrder(48);

            Property(l => l.IndiceAtrasoCorrecaoId)
                .HasColumnName("indiceAtrasoCorrecao")
                .HasColumnOrder(49);

        }
    }
}
