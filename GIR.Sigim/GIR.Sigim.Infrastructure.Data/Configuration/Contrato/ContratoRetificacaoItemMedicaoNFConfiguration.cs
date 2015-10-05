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
    public class ContratoRetificacaoItemMedicaoNFConfiguration : EntityTypeConfiguration<ContratoRetificacaoItemMedicaoNF>
    {
        public ContratoRetificacaoItemMedicaoNFConfiguration()
        {
            ToTable("ContratoRetificacaoItemMedicaoNF", "Contrato");

            Property(l => l.Id)
                .HasColumnName("codigo")
                .HasColumnOrder(1);

            Property(l => l.ContratoId)
                .IsRequired()
                .HasColumnName("contrato")
                .HasColumnOrder(2);

            HasRequired<Domain.Entity.Contrato.Contrato>(l => l.Contrato)
                .WithMany(c => c.ListaContratoRetificacaoItemMedicaoNF)
                .HasForeignKey(l => l.ContratoId);

            Property(l => l.TipoDocumentoId)
                .IsRequired()
                .HasColumnName("tipoDocumento")
                .HasColumnOrder(3);

            HasRequired<TipoDocumento>(l => l.TipoDocumento)
                .WithMany(c => c.ListaContratoRetificacaoItemMedicaoNF)
                .HasForeignKey(l => l.TipoDocumentoId);

            Property(l => l.NumeroDocumento)
                .IsRequired()
                .HasColumnName("numeroDocumento")
                .HasMaxLength(10)
                .HasColumnOrder(4);

            Property(l => l.DataEntrega)
                .IsRequired()
                .HasColumnName("dataEntrega")
                .HasColumnOrder(5);

            Property(l => l.MaterialId)
                .IsRequired()
                .HasColumnName("material")
                .HasColumnOrder(6);

            HasRequired<Material>(l => l.Material)
                .WithMany(c => c.ListaContratoRetificacaoItemMedicaoNF)
                .HasForeignKey(l => l.MaterialId);

            Property(l => l.CodigoClasse)
                .HasColumnName("classe")
                .IsRequired()
                .HasColumnOrder(7);

            HasRequired<Domain.Entity.Financeiro.Classe>(l => l.Classe)
                .WithMany(c => c.ListaContratoRetificacaoItemMedicaoNF)
                .HasForeignKey(l => l.CodigoClasse);

            Property(l => l.Sequencial)
                .IsRequired()
                .HasColumnName("sequencial")
                .HasColumnOrder(8);

            Property(l => l.ComplementoDescricao)
                .HasColumnName("complementoDescricao")
                .HasMaxLength(80)
                .HasColumnOrder(9);

            Property(l => l.Quantidade)
                .IsRequired()
                .HasColumnName("quantidade")
                .HasPrecision(18, 5)
                .HasColumnOrder(10);

            Property(l => l.ValorUnitario)
                .IsRequired()
                .HasColumnName("valorUnitario")
                .HasPrecision(18, 5)
                .HasColumnOrder(11);

            Property(l => l.BaseIPI)
                .HasColumnName("baseIPI")
                .HasPrecision(18, 5)
                .HasColumnOrder(12);

            Property(l => l.PercentualIpi)
                .HasColumnName("percentualIPI")
                .HasPrecision(18, 5)
                .HasColumnOrder(13);

            Property(l => l.BaseIcms)
                .HasColumnName("baseICMS")
                .HasPrecision(18, 5)
                .HasColumnOrder(14);

            Property(l => l.PercentualIcms)
                .HasColumnName("percentualICMS")
                .HasPrecision(18, 5)
                .HasColumnOrder(15);

            Property(l => l.BaseIcmsSt)
                .HasColumnName("baseICMSST")
                .HasPrecision(18, 5)
                .HasColumnOrder(16);

            Property(l => l.PercentualIcmsSt)
                .HasColumnName("percentualICMSST")
                .HasPrecision(18, 5)
                .HasColumnOrder(17);

            Property(l => l.PercentualDesconto)
                .HasColumnName("percentualDesconto")
                .HasPrecision(18, 5)
                .HasColumnOrder(18);

            Property(l => l.ValorTotalSemImposto)
                .HasColumnName("valorTotalSemImposto")
                .HasPrecision(18, 5)
                .HasColumnOrder(19);

            Property(l => l.ValorTotalItem)
                .HasColumnName("valorTotalItem")
                .HasPrecision(18, 5)
                .HasColumnOrder(20);

            Property(l => l.ComplementoNaturezaOperacao)
                .HasColumnName("complementoNaturezaOperacao")
                .HasMaxLength(10)
                .HasColumnOrder(21);

            Property(l => l.ComplementoCST)
                .HasColumnName("complementoCST")
                .HasMaxLength(10)
                .HasColumnOrder(22);

            Property(l => l.NaturezaReceita)
                .HasColumnName("naturezaReceita")
                .HasMaxLength(10)
                .HasColumnOrder(23);

            Property(l => l.DataEmissao)
                .HasColumnName("dataEmissao")
                .HasColumnOrder(24);
        }
    }
}
