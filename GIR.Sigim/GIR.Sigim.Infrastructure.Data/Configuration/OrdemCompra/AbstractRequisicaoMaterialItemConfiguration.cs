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
    public class AbstractRequisicaoMaterialItemConfiguration<T> : EntityTypeConfiguration<T> where T : AbstractRequisicaoMaterialItem
    {
        public AbstractRequisicaoMaterialItemConfiguration()
        {
            Property(l => l.Id)
                .HasColumnName("codigo");

            Property(l => l.MaterialId)
                .HasColumnName("material");

            Property(l => l.CodigoClasse)
                .HasMaxLength(18)
                .HasColumnName("classe");

            Property(l => l.Sequencial)
                .HasColumnName("sequencial");

            Property(l => l.Complemento)
                .HasMaxLength(80)
                .HasColumnName("complementoDescricao");

            Property(l => l.UnidadeMedida)
                .IsRequired()
                .HasMaxLength(6)
                .HasColumnName("unidadeMedida");

            Property(l => l.Quantidade)
                .HasPrecision(18, 5)
                .HasColumnName("quantidade");

            Property(l => l.QuantidadeAprovada)
                .HasPrecision(18, 5)
                .HasColumnName("quantidadeAprovada");

            Property(l => l.DataMinima)
                .HasColumnName("dataMinima");

            Property(l => l.DataMaxima)
                .HasColumnName("dataMaxima");
        }
    }
}