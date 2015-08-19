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
    public class MovimentoItemConfiguration : EntityTypeConfiguration<MovimentoItem>
    {
        public MovimentoItemConfiguration()
        {
            ToTable("MovimentoItem", "Estoque");

            Property(l => l.Id)
                .HasColumnName("codigo");

            Property(l => l.MovimentoId)
                .HasColumnName("movimento");

            HasRequired(l => l.Movimento)
                .WithMany(l => l.ListaMovimentoItem)
                .HasForeignKey(l => l.MovimentoId);

            Property(l => l.MaterialId)
                .HasColumnName("material");

            HasRequired(l => l.Material)
                .WithMany(l => l.ListaMovimentoItem)
                .HasForeignKey(l => l.MaterialId);

            Property(l => l.CodigoClasse)
                .HasMaxLength(18)
                .HasColumnName("classe");

            HasOptional(l => l.Classe)
                .WithMany(l => l.ListaMovimentoItem)
                .HasForeignKey(l => l.CodigoClasse);

            Property(l => l.Quantidade)
                .HasPrecision(18, 5)
                .HasColumnName("quantidade");

            Property(l => l.QuantidadeSaldo)
                .HasPrecision(18, 5)
                .HasColumnName("quantidadeSaldo");

            Property(l => l.Valor)
                .HasPrecision(18, 5)
                .HasColumnName("valor");

            Property(l => l.Observacao)
                .HasMaxLength(2000)
                .HasColumnName("obs");

            Property(l => l.MovimentoDevolucaoId)
                .HasColumnName("movimentoDevolucao");

            Property(l => l.MovimentoItemDevolucaoId)
                .HasColumnName("movimentoItemDevolucao");
        }
    }
}