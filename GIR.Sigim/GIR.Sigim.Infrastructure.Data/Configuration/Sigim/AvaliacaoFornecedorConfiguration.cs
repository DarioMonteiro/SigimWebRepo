using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Sigim;
using GIR.Sigim.Domain.Entity.Contrato;

namespace GIR.Sigim.Infrastructure.Data.Configuration.Sigim
{
    public class AvaliacaoFornecedorConfiguration : EntityTypeConfiguration<AvaliacaoFornecedor>
    {
        public AvaliacaoFornecedorConfiguration()
        {
            ToTable("AvaliacaoFornecedor", "Sigim");

            Property(l => l.Id)
                .HasColumnName("codigo");

            Property(l => l.ClienteFornecedorId)
                .HasColumnName("clienteFornecedor");

            HasRequired(l => l.ClienteFornecedor)
                .WithMany(l => l.ListaAvaliacaoFornecedor)
                .HasForeignKey(l => l.ClienteFornecedorId);

            Property(l => l.AvaliacaoModeloId)
                .HasColumnName("avaliacaoModelo");

            HasRequired(l => l.AvaliacaoModelo)
                .WithMany(l => l.ListaAvaliacaoFornecedor)
                .HasForeignKey(l => l.AvaliacaoModeloId);

            Property(l => l.Data)
                .HasColumnName("data");

            Property(l => l.LoginUsuarioCadastro)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("usuario");

            Property(l => l.MediaMinima)
                .HasColumnName("mediaMinima");

            Property(l => l.MediaObtida)
                .HasPrecision(18, 5)
                .HasColumnName("mediaObtida");

            Property(l => l.Observacao)
                .HasMaxLength(500)
                .HasColumnName("observacao");

            Property(l => l.TipoDocumentoId)
                .HasColumnName("tipoDocumento");

            HasOptional(l => l.TipoDocumento)
                .WithMany(l => l.ListaAvaliacaoFornecedor)
                .HasForeignKey(l => l.TipoDocumentoId);

            Property(l => l.Documento)
                .HasMaxLength(10)
                .HasColumnName("documento");

            Property(l => l.EntradaMaterialId)
                .HasColumnName("entradaMaterial");

            HasOptional(l => l.EntradaMaterial)
                .WithMany(l => l.ListaAvaliacaoFornecedor)
                .HasForeignKey(l => l.EntradaMaterialId);
        }
    }
}