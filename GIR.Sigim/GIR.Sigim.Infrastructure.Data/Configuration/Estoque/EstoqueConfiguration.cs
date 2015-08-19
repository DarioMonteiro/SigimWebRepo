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
    public class EstoqueConfiguration : EntityTypeConfiguration<Domain.Entity.Estoque.Estoque>
    {
        public EstoqueConfiguration()
        {
            ToTable("Estoque", "Estoque");

            Property(l => l.Id)
                .HasColumnName("codigo");

            Property(l => l.Descricao)
                .HasMaxLength(100)
                .HasColumnName("descricao");

            Property(l => l.CodigoCentroCusto)
                .HasMaxLength(18)
                .HasColumnName("centroCusto");

            HasOptional(l => l.CentroCusto)
                .WithMany(l => l.ListaEstoque)
                .HasForeignKey(l => l.CodigoCentroCusto);

            //Endereco

            Property(l => l.Situacao)
                .HasColumnType("char")
                .HasMaxLength(1)
                .HasColumnName("situacao");

            Ignore(l => l.Ativo);
        }
    }
}