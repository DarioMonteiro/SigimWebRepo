using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Orcamento;

namespace GIR.Sigim.Infrastructure.Data.Configuration.Orcamento
{
    public class OrcamentoConfiguration : EntityTypeConfiguration<Domain.Entity.Orcamento.Orcamento>
    {
        public OrcamentoConfiguration()
        {
            ToTable("Orcamento", "Orcamento");

            Property(l => l.Id)
                .HasColumnName("codigo")
                .HasColumnOrder(1);

            //Empresa

            Property(l => l.ObraId)
                .HasColumnName("obra")
                .HasColumnOrder(3);

            HasRequired(l => l.Obra)
                .WithMany(l => l.ListaOrcamento)
                .HasForeignKey(l => l.ObraId);

            Property(l => l.Sequencial)
                .HasColumnName("sequencial")
                .HasColumnOrder(4);

            Property(l => l.Descricao)
                .HasMaxLength(50)
                .HasColumnName("descricao")
                .HasColumnOrder(5);

            Property(l => l.Data)
                .HasColumnName("data")
                .HasColumnOrder(6);

            Property(l => l.Situacao)
                .HasColumnType("char")
                .HasMaxLength(1)
                .HasColumnName("situacao")
                .HasColumnOrder(7);

            Property(l => l.EhControlado)
                .HasColumnName("controlado")
                .HasColumnOrder(8);
        }
    }
}