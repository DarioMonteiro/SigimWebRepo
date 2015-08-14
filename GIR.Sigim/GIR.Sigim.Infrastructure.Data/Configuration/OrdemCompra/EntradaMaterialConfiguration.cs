using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.OrdemCompra;

namespace GIR.Sigim.Infrastructure.Data.Configuration.OrdemCompra
{
    public class EntradaMaterialConfiguration : EntityTypeConfiguration<EntradaMaterial>
    {
        public EntradaMaterialConfiguration()
        {
            ToTable("EntradaMaterial", "OrdemCompra");

            Property(l => l.Id)
                .HasColumnName("codigo");

            Property(l => l.CodigoCentroCusto)
                .HasMaxLength(18)
                .HasColumnName("centroCusto");

            HasRequired(l => l.CentroCusto)
                .WithMany(l => l.ListaEntradaMaterial)
                .HasForeignKey(l => l.CodigoCentroCusto);

            Property(l => l.ClienteFornecedorId)
                .HasColumnName("clienteFornecedor");

            HasRequired(l => l.ClienteFornecedor)
                .WithMany(l => l.ListaEntradaMaterial)
                .HasForeignKey(l => l.ClienteFornecedorId);

            Property(l => l.FornecedorNotaId)
                .HasColumnName("fornecedorNota");

            HasRequired(l => l.FornecedorNota)
                .WithMany(l => l.ListaEntradaMaterialNota)
                .HasForeignKey(l => l.FornecedorNotaId);

            Property(l => l.Data)
                .HasColumnName("dataEntradaMaterial");

            Property(l => l.Situacao)
                .HasColumnName("situacao");

            Property(l => l.TipoNotaFiscalId)
                .HasColumnName("tipoNotaFiscal");

            HasRequired(l => l.TipoNotaFiscal)
                .WithMany(l => l.ListaEntradaMaterialNotaFiscal)
                .HasForeignKey(l => l.TipoNotaFiscalId);

            Property(l => l.NumeroNotaFiscal)
                .HasMaxLength(10)
                .HasColumnName("numeroNotaFiscal");

            Property(l => l.DataEmissaoNota)
                .HasColumnName("dataEmissaoNota");

            Property(l => l.DataEntregaNota)
                .HasColumnName("dataEntregaNota");

            Property(l => l.Observacao)
                .HasMaxLength(255)
                .HasColumnName("observacao");

            Property(l => l.PercentualDesconto)
                .HasPrecision(18, 5)
                .HasColumnName("percentualDesconto");

            Property(l => l.Desconto)
                .HasPrecision(18, 2)
                .HasColumnName("desconto");

            Property(l => l.PercentualISS)
                .HasPrecision(18, 5)
                .HasColumnName("percentualISS");

            Property(l => l.ISS)
                .HasPrecision(18, 2)
                .HasColumnName("iss");

            Property(l => l.FreteIncluso)
                .HasPrecision(18, 2)
                .HasColumnName("freteIncluso");

            Property(l => l.DataCadastro)
                .HasColumnName("dataCadastro");

            Property(l => l.LoginUsuarioCadastro)
                .HasMaxLength(50)
                .HasColumnName("usuarioCadastro");

            Property(l => l.DataLiberacao)
                .HasColumnName("dataLibera");

            Property(l => l.LoginUsuarioLiberacao)
                .HasMaxLength(50)
                .HasColumnName("usuarioLibera");

            Property(l => l.DataCancelamento)
                .HasColumnName("dataCancela");

            Property(l => l.LoginUsuarioCancelamento)
                .HasMaxLength(50)
                .HasColumnName("usuarioCancela");

            Property(l => l.MotivoCancelamento)
                .HasMaxLength(255)
                .HasColumnName("motivoCancela");

            Property(l => l.TransportadoraId)
                .HasColumnName("transportadora");

            HasRequired(l => l.Transportadora)
                .WithMany(l => l.ListaEntradaMaterialTransportadora)
                .HasForeignKey(l => l.TransportadoraId);

            Property(l => l.DataFrete)
                .HasColumnName("dataFrete");

            Property(l => l.ValorFrete)
                .HasPrecision(18, 2)
                .HasColumnName("valorFrete");
            
            Property(l => l.TipoNotaFreteId)
                .HasColumnName("tipoNotaFrete");

            HasRequired(l => l.TipoNotaFrete)
                .WithMany(l => l.ListaEntradaMaterialNotaFrete)
                .HasForeignKey(l => l.TipoNotaFreteId);

            Property(l => l.NumeroNotaFrete)
                .HasMaxLength(50)
                .HasColumnName("numeroNotaFrete");

            Property(l => l.OrdemCompraFreteId)
                .HasColumnName("ordemCompraFrete");

            HasOptional(l => l.OrdemCompraFrete)
                .WithMany(l => l.ListaEntradaMaterialFrete)
                .HasForeignKey(l => l.OrdemCompraFreteId);

            Property(l => l.TituloFreteId)
                .HasColumnName("tituloFrete");

            HasRequired(l => l.TituloFrete)
                .WithMany(l => l.ListaEntradaMaterial)
                .HasForeignKey(l => l.TituloFreteId);

            Property(l => l.CodigoTipoCompra)
                .HasMaxLength(10)
                .HasColumnName("tipoCompra");

            HasRequired(l => l.TipoCompra)
                .WithMany(l => l.ListaEntradaMaterial)
                .HasForeignKey(l => l.CodigoTipoCompra);

            Property(l => l.CifFobId)
                .HasColumnName("CIFFOB");

            HasRequired(l => l.CifFob)
                .WithMany(l => l.ListaEntradaMaterial)
                .HasForeignKey(l => l.CifFobId);

            Property(l => l.CodigoNaturezaOperacao)
                .HasMaxLength(10)
                .HasColumnName("naturezaOperacao");

            HasRequired(l => l.NaturezaOperacao)
                .WithMany(l => l.ListaEntradaMaterial)
                .HasForeignKey(l => l.CodigoNaturezaOperacao);

            Property(l => l.SerieNFId)
                .HasColumnName("serieNF");

            HasRequired(l => l.SerieNF)
                .WithMany(l => l.ListaEntradaMaterial)
                .HasForeignKey(l => l.SerieNFId);

            Property(l => l.CodigoCST)
                .HasMaxLength(10)
                .HasColumnName("CST");

            HasRequired(l => l.CST)
                .WithMany(l => l.ListaEntradaMaterial)
                .HasForeignKey(l => l.CodigoCST);

            Property(l => l.CodigoContribuicaoId)
                .HasMaxLength(10)
                .HasColumnName("codigoContribuicao");

            HasRequired(l => l.CodigoContribuicao)
                .WithMany(l => l.ListaEntradaMaterial)
                .HasForeignKey(l => l.CodigoContribuicaoId);

            Property(l => l.CodigoBarras)
                .HasMaxLength(50)
                .HasColumnName("codigoBarras");

            Property(l => l.EhConferido)
                .HasColumnName("conferido");

            Property(l => l.DataConferencia)
                .HasColumnName("dataConferencia");

            Property(l => l.LoginUsuarioConferencia)
                .HasMaxLength(50)
                .HasColumnName("usuarioConferencia");
        }
    }
}