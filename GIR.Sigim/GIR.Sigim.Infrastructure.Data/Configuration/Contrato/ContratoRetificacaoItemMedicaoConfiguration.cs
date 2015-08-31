using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Contrato;
using GIR.Sigim.Domain.Entity.Financeiro;
using GIR.Sigim.Domain.Entity.Sigim;

namespace GIR.Sigim.Infrastructure.Data.Configuration.Contrato
{
    public class ContratoRetificacaoItemMedicaoConfiguration : EntityTypeConfiguration<ContratoRetificacaoItemMedicao>
    {
        public ContratoRetificacaoItemMedicaoConfiguration()
        {
            ToTable("ContratoRetificacaoItemMedicao", "Contrato");

            Property(l => l.Id)
                .HasColumnName("codigo")
                .HasColumnOrder(1);

            Property(l => l.ContratoId)
                .IsRequired()
                .HasColumnName("contrato")
                .HasColumnOrder(2);

            HasRequired<Domain.Entity.Contrato.Contrato>(l => l.Contrato)
                .WithMany(c => c.ListaContratoRetificacaoItemMedicao)
                .HasForeignKey(l => l.ContratoId);

            Property(l => l.ContratoRetificacaoId)
                .IsRequired()
                .HasColumnName("contratoRetificacao")
                .HasColumnOrder(3);

            HasRequired<ContratoRetificacao>(l => l.ContratoRetificacao)
                .WithMany(c => c.ListaContratoRetificacaoItemMedicao)
                .HasForeignKey(l => l.ContratoRetificacaoId);

            Property(l => l.ContratoRetificacaoItemId)
                .IsRequired()
                .HasColumnName("contratoRetificacaoItem")
                .HasColumnOrder(4);

            HasRequired<ContratoRetificacaoItem>(l => l.ContratoRetificacaoItem)
                .WithMany(c => c.ListaContratoRetificacaoItemMedicao)
                .HasForeignKey(l => l.ContratoRetificacaoItemId);

            Property(l => l.SequencialItem)
                .IsRequired()
                .HasColumnName("sequencialItem")
                .HasColumnOrder(5);

            Property(l => l.ContratoRetificacaoItemCronogramaId)
                .IsRequired()
                .HasColumnName("contratoRetificacaoItemCronograma")
                .HasColumnOrder(6);

            HasRequired<ContratoRetificacaoItemCronograma>(l => l.ContratoRetificacaoItemCronograma)
                .WithMany(c => c.ListaContratoRetificacaoItemMedicao)
                .HasForeignKey(l => l.ContratoRetificacaoItemCronogramaId);

            Property(l => l.SequencialCronograma)
                .IsRequired()
                .HasColumnName("sequencialCronograma")
                .HasColumnOrder(7);

            Property(l => l.Situacao)
                .IsRequired()
                .HasColumnName("situacao")
                .HasColumnOrder(8);

            Property(l => l.TipoDocumentoId)
                .IsRequired()
                .HasColumnName("tipoDocumento")
                .HasColumnOrder(9);

            HasRequired<TipoDocumento>(l => l.TipoDocumento)
                .WithMany(c => c.ListaContratoRetificacaoItemMedicao)
                .HasForeignKey(l => l.TipoDocumentoId);

            Property(l => l.NumeroDocumento)
                .IsRequired()
                .HasColumnName("numeroDocumento")
                .HasMaxLength(10)
                .HasColumnOrder(10);

            Property(l => l.DataEmissao)
                .IsRequired()
                .HasColumnName("dataEmissao")
                .HasColumnOrder(11);

            Property(l => l.DataVencimento)
                .IsRequired()
                .HasColumnName("dataVencimento")
                .HasColumnOrder(12);

            Property(l => l.Quantidade)
                .IsRequired()
                .HasColumnName("quantidade")
                .HasPrecision(18, 7)
                .HasColumnOrder(13);

            Property(l => l.Valor)
                .IsRequired()
                .HasColumnName("valor")
                .HasPrecision(18,5)
                .HasColumnOrder(14);

            Property(l => l.DataMedicao)
                .IsRequired()
                .HasColumnName("dataMedicao")
                .HasColumnOrder(15);

            Property(l => l.UsuarioMedicao)
                .HasColumnName("usuarioMedicao")
                .HasMaxLength(50)
                .HasColumnOrder(16);

            Property(l => l.MultiFornecedorId)
                .HasColumnName("multiFornecedor")
                .HasColumnOrder(17);

            HasOptional<ClienteFornecedor>(l => l.MultiFornecedor)
                .WithMany(c => c.ListaContratoRetificacaoItemMedicao)
                .HasForeignKey(l => l.MultiFornecedorId);

            Property(l => l.Observacao)
                .HasColumnName("observacao")
                .HasMaxLength(255)
                .HasColumnOrder(18);

            Property(l => l.TituloPagarId)
                .HasColumnName("tituloPagar")
                .HasColumnOrder(19);

            HasOptional<TituloPagar>(l => l.TituloPagar)
                .WithMany(c => c.ListaContratoRetificacaoItemMedicao)
                .HasForeignKey(l => l.TituloPagarId);

            Property(l => l.TituloReceberId)
                .HasColumnName("tituloReceber")
                .HasColumnOrder(20);

            HasOptional<TituloReceber>(l => l.TituloReceber)
                .WithMany(c => c.ListaContratoRetificacaoItemMedicao)
                .HasForeignKey(l => l.TituloReceberId);

            Property(l => l.DataLiberacao)
                .HasColumnName("dataLiberacao")
                .HasColumnOrder(21);

            Property(l => l.UsuarioLiberacao)
                .HasColumnName("usuarioLiberacao")
                .HasMaxLength(50)
                .HasColumnOrder(22);

            Property(l => l.ValorRetido)
                .HasColumnName("valorRetido")
                .HasPrecision(18, 5)
                .HasColumnOrder(23);

            Property(l => l.TipoCompraCodigo)
                .HasColumnName("tipoCompra")
                .HasMaxLength(10)
                .HasColumnOrder(24);

            HasOptional<TipoCompra>(l => l.TipoCompra)
                .WithMany(c => c.ListaContratoRetificacaoItemMedicao)
                .HasForeignKey(l => l.TipoCompraCodigo);

            Property(l => l.CifFobId)
                .HasColumnName("CIFFOB")
                .HasColumnOrder(25);

            HasOptional<CifFob>(l => l.CifFob)
                .WithMany(c => c.ListaContratoRetificacaoItemMedicao)
                .HasForeignKey(l => l.CifFobId);

            Property(l => l.NaturezaOperacaoCodigo)
                .HasColumnName("naturezaOperacao")
                .HasMaxLength(10)
                .HasColumnOrder(26);

            HasOptional<NaturezaOperacao>(l => l.NaturezaOperacao)
                .WithMany(c => c.ListaContratoRetificacaoItemMedicao)
                .HasForeignKey(l => l.NaturezaOperacaoCodigo);

            Property(l => l.SerieNFId)
                .HasColumnName("serieNF")
                .HasColumnOrder(27);

            HasOptional<SerieNF>(l => l.SerieNF)
                .WithMany(c => c.ListaContratoRetificacaoItemMedicao)
                .HasForeignKey(l => l.SerieNFId);

            Property(l => l.CSTCodigo)
                .HasColumnName("CST")
                .HasMaxLength(10)
                .HasColumnOrder(28);

            HasOptional<CST>(l => l.CST)
                .WithMany(c => c.ListaContratoRetificacaoItemMedicao)
                .HasForeignKey(l => l.CSTCodigo);

            Property(l => l.CodigoContribuicaoCodigo)
                .HasColumnName("codigoContribuicao")
                .HasMaxLength(10)
                .HasColumnOrder(29);

            HasOptional<CodigoContribuicao>(l => l.CodigoContribuicao)
                .WithMany(c => c.ListaContratoRetificacaoItemMedicao)
                .HasForeignKey(l => l.CodigoContribuicaoCodigo);

            Property(l => l.CodigoBarras)
                .HasColumnName("codigoBarras")
                .HasMaxLength(50)
                .HasColumnOrder(30);

            Property(l => l.BaseIPI)
                .HasColumnName("baseIPI")
                .HasPrecision(18, 5)
                .HasColumnOrder(31);

            Property(l => l.PercentualIpi)
                .HasColumnName("percentualIPI")
                .HasPrecision(18, 5)
                .HasColumnOrder(32);

            Property(l => l.BaseIcms)
                .HasColumnName("baseICMS")
                .HasPrecision(18, 5)
                .HasColumnOrder(33);

            Property(l => l.PercentualIcms)
                .HasColumnName("percentualICMS")
                .HasPrecision(18, 5)
                .HasColumnOrder(34);

            Property(l => l.BaseIcmsSt)
                .HasColumnName("baseICMSST")
                .HasPrecision(18, 5)
                .HasColumnOrder(35);

            Property(l => l.PercentualIcmsSt)
                .HasColumnName("percentualICMSST")
                .HasPrecision(18, 5)
                .HasColumnOrder(36);

            Property(l => l.Conferido)
                .HasColumnName("conferido")
                .HasColumnType("bit")
                .HasColumnOrder(37);

            Property(l => l.UsuarioConferencia)
                .HasColumnName("usuarioConferencia")
                .HasMaxLength(50)
                .HasColumnOrder(38);

            Property(l => l.DataConferencia)
                .HasColumnName("dataConferencia")
                .HasColumnOrder(39);

            Property(l => l.DataCadastro)
                .HasColumnName("dataCadastro")
                .HasColumnOrder(40);

            Property(l => l.Desconto)
                .HasColumnName("desconto")
                .HasPrecision(18,5)
                .HasColumnOrder(41);

            Property(l => l.MotivoDesconto)
                .HasColumnName("motivoDesconto")
                .HasMaxLength(30)
                .HasColumnOrder(42);

            HasMany<ContratoRetencao>(l => l.ListaContratoRetencao)
                .WithRequired(c => c.ContratoRetificacaoItemMedicao)
                .HasForeignKey(c => c.ContratoRetificacaoItemMedicaoId);  
        }
    }
}
