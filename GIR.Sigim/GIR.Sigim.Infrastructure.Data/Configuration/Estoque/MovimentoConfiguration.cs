using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Estoque;

namespace GIR.Sigim.Infrastructure.Data.Configuration.Financeiro
{
    public class MovimentoConfiguration : EntityTypeConfiguration<Movimento>
    {
        public MovimentoConfiguration()
        {
            ToTable("Movimento", "Estoque");

            Property(l => l.Id)
                .HasColumnName("codigo");

            Property(l => l.EstoqueId)
                .IsRequired()
                .HasColumnName("estoque");

            Property(l => l.ClienteFornecedorId)
                .IsRequired()
                .HasColumnName("clienteFornecedor");

            Property(l => l.MovimentoPaiId)
                .HasColumnName("movimento");

            Property(l => l.TipoMovimento)
                .IsRequired()
                .HasColumnName("tipoMovimento");

            Property(l => l.Data)
                .HasColumnName("dataMovimento");

            Property(l => l.EntradaMaterialId)
                .HasColumnName("entradaMaterial");

            HasOptional(l => l.EntradaMaterial)
                .WithMany(l => l.ListaMovimentoEstoque)
                .HasForeignKey(l => l.EntradaMaterialId);

            //ResponsavelSolicitacao
            //ResponsavelAutorizacao

            Property(l => l.ResponsavelRetirada)
                .HasMaxLength(100)
                .HasColumnName("responsavelRetirada");

            Property(l => l.EhTransferenciaDefinitiva)
                .HasColumnName("transferenciaDefinitiva");

            Property(l => l.Observacao)
                .HasMaxLength(2000)
                .HasColumnName("obs");

            Property(l => l.TipoDocumentoId)
                .HasColumnName("tipoDocumento");

            Property(l => l.Documento)
                .HasMaxLength(50)
                .HasColumnName("documento");

            Property(l => l.Referencia)
                .HasMaxLength(50)
                .HasColumnName("referencia");

            Property(l => l.DataEmissao)
                .HasColumnName("dataEmissao");

            Property(l => l.DataEntrega)
                .HasColumnName("dataEntrega");

            Property(l => l.DataOperacao)
                .HasColumnName("dataOperacao");

            Property(l => l.LoginUsuarioOperacao)
                .IsRequired()
                .HasColumnType("nvarchar")
                .HasMaxLength(128)
                .HasColumnName("usuarioOperacao");

            Property(l => l.EhMovimentoTemporario)
                .HasColumnName("movimentoTemporario");

            Property(l => l.ContratoId)
                .HasColumnName("contrato");

            HasOptional<Domain.Entity.Contrato.Contrato>(l => l.Contrato)
                .WithMany(c => c.ListaMovimento)
                .HasForeignKey(l => l.ContratoId);
        }
    }
}