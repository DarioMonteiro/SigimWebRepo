using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Comercial;
using GIR.Sigim.Domain.Entity.Sigim;

namespace GIR.Sigim.Infrastructure.Data.Configuration.Comercial
{
    public class VendaSerieConfiguration : EntityTypeConfiguration<VendaSerie>
    {
        public VendaSerieConfiguration()
        {
            ToTable("VendaSerie", "Comercial");

            Ignore(l => l.Id);

            HasKey(l => new { l.ContratoId, l.NumeroSerie });

            Property(l => l.ContratoId)
                .HasColumnName("contrato")
                .IsRequired()
                .HasColumnOrder(1);

            HasRequired<ContratoComercial>(l => l.Contrato)
                .WithMany(c => c.ListaVendaSerie)
                .HasForeignKey(l => l.ContratoId);

            Property(l => l.NumeroSerie)
                .HasColumnName("numeroSerie")
                .IsRequired()
                .HasColumnOrder(2);

            Property(l => l.NomeSerie)
                .HasColumnName("nomeSerie")
                .HasColumnType("tinyint")
                .IsRequired()
                .HasColumnOrder(3);

            Property(l => l.CapitalSerie)
                .HasColumnName("capitalSerie")
                .HasPrecision(18,2)
                .IsRequired()
                .HasColumnOrder(4);

            Property(l => l.TipoCapital)
                .HasColumnName("tipoCapital")
                .HasMaxLength(1)
                .IsRequired()
                .HasColumnOrder(5);

            Property(l => l.FormaFinanciamento)
                .HasColumnName("formaFinanciamento")
                .HasMaxLength(1)
                .IsRequired()
                .HasColumnOrder(6);

            Property(l => l.Periodicidade)
                .HasColumnName("periodicidade")
                .HasMaxLength(2)
                .HasColumnOrder(7);

            Property(l => l.PercentualJurosSerie)
                .HasColumnName("percentualJurosSerie")
                .HasPrecision(18,5)
                .IsRequired()
                .HasColumnOrder(8);

            Property(l => l.NumeroParcelas)
                .HasColumnName("numeroParcelas")
                .HasColumnType("smallint")
                .IsRequired()
                .HasColumnOrder(9);

            Property(l => l.DataPrimeiroVencimento)
                .HasColumnName("dataPrimeiroVencimento")
                .IsRequired()
                .HasColumnOrder(10);

            Property(l => l.ValorParcela)
                .HasColumnName("valorParcela")
                .HasPrecision(18, 2)
                .IsRequired()
                .HasColumnOrder(11);

            Property(l => l.IndiceCorrecaoId)
                .HasColumnName("indiceCorrecao")
                .HasColumnOrder(12);

            HasOptional<IndiceFinanceiro>(l => l.IndiceCorrecao)
                .WithMany(c => c.ListaVendaSerieIndiceCorrecao)
                .HasForeignKey(l => l.IndiceCorrecaoId);

            Property(l => l.DataBaseIndiceCorrecao)
                .HasColumnName("dataBaseIndiceCorrecao")
                .HasColumnOrder(13);

            Property(l => l.CotacaoIndiceCorrecao)
                .HasColumnName("cotacaoIndiceCorrecao")
                .HasPrecision(18,5)
                .HasColumnOrder(14);

            Property(l => l.IndiceAtrasoCorrecaoId)
                .HasColumnName("indiceAtrasoCorrecao")
                .HasColumnOrder(15);

            HasOptional<IndiceFinanceiro>(l => l.IndiceAtrasoCorrecao)
                .WithMany(c => c.ListaVendaSerieIndiceAtrasoCorrecao)
                .HasForeignKey(l => l.IndiceAtrasoCorrecaoId);

            Property(l => l.CobrancaResiduo)
                .HasColumnName("cobrancaResiduo")
                .HasMaxLength(1)
                .HasColumnOrder(16);

            Property(l => l.DataBaseAniversarioCobrancaResiduo)
                .HasColumnName("dataBaseAniversarioCobrancaResiduo")
                .HasColumnOrder(17);

            Property(l => l.DefasagemDia)
                .HasColumnName("defasagemDia")
                .HasColumnOrder(18);

            Property(l => l.DefasagemMes)
                .HasColumnName("defasagemMes")
                .HasColumnOrder(19);

            Property(l => l.DataBaseTabelaVenda)
                .HasColumnName("dataBaseTabelaVenda")
                .HasColumnOrder(20);

            Property(l => l.PercentualJurosDefasagem)
                .HasColumnName("percentualJurosDefasagem")
                .HasPrecision(18, 5)
                .HasColumnOrder(21);

            Property(l => l.TipoJurosDefasagem)
                .HasColumnName("tipoJurosDefasagem")
                .HasMaxLength(1)
                .HasColumnOrder(22);

            Property(l => l.IndiceReajusteId)
                .HasColumnName("indiceReajuste")
                .HasColumnOrder(23);

            HasOptional<IndiceFinanceiro>(l => l.IndiceReajuste)
                .WithMany(c => c.ListaVendaSerieIndiceReajuste)
                .HasForeignKey(l => l.IndiceReajusteId);

            Property(l => l.DataBaseIndiceReajuste)
                .HasColumnName("dataBaseIndiceReajuste")
                .HasColumnOrder(24);

            Property(l => l.CotacaoIndiceReajuste)
                .HasColumnName("cotacaoIndiceReajuste")
                .HasPrecision(18, 5)
                .HasColumnOrder(25);

            Property(l => l.DataBaseJuros)
                .HasColumnName("dataBaseJuros")
                .HasColumnOrder(26);

            Property(l => l.DataProximoAniversario)
                .HasColumnName("dataProximoAniversario")
                .HasColumnOrder(27);

            Property(l => l.DataUltimoAniversario)
                .HasColumnName("dataUltimoAniversario")
                .HasColumnOrder(28);

            Property(l => l.RenegociacaoId)
                .HasColumnName("renegociacao")
                .HasColumnOrder(29);

            HasOptional<Renegociacao>(l => l.Renegociacao)
                .WithMany(c => c.ListaVendaSerie)
                .HasForeignKey(l => l.RenegociacaoId);

            Property(l => l.DataCancelamentoRenegociacao)
                .HasColumnName("dataCancelamentoRenegociacao")
                .HasColumnOrder(30);

            Property(l => l.DefasagemMesIndiceCorrecao)
                .HasColumnName("defasagemMesIndiceCorrecao")
                .HasColumnType("smallint")
                .HasColumnOrder(31);
        }

    }
}
