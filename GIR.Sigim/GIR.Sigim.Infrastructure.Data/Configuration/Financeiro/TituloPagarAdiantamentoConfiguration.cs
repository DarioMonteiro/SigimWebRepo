using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Financeiro;

namespace GIR.Sigim.Infrastructure.Data.Configuration.Financeiro
{
    class TituloPagarAdiantamentoConfiguration : EntityTypeConfiguration<TituloPagarAdiantamento>
    {
        public TituloPagarAdiantamentoConfiguration()
        {
            ToTable("TituloPagarAdiantamento", "Financeiro");

            Property(l => l.Id)
                .HasColumnName("codigo");

            Property(l => l.ClienteId)
                .HasColumnName("cliente");

            Property(l => l.Identificacao)
                .HasMaxLength(70)
                .HasColumnName("identificacao");

            Property(l => l.TipoDocumentoId)
                .HasColumnName("tipoDocumento");

            Property(l => l.Documento)
                .HasMaxLength(10)
                .HasColumnName("documento");

            Property(l => l.DataEmissaoDocumento)
                .HasColumnName("dataEmissaoDocumento");

            Property(l => l.ValorAdiantamento)
                .HasPrecision(18, 5)
                .HasColumnName("valorAdiantamento");

            Property(l => l.LoginUsuarioCadastro)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("operadorCadastro");

            Property(l => l.DataCadastro)
                .HasColumnName("dataCadastro");

            Property(l => l.TituloPagarId)
                .HasColumnName("tituloPagarAdiantamento");

            Property(l => l.EntradaMaterialFormaPagamentoId)
                .HasColumnName("entradaMaterialFormaPagamento");
        }
    }
}