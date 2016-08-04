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
    public class ObraConfiguration : EntityTypeConfiguration<Obra>
    {
        public ObraConfiguration()
        {
            ToTable("Obra", "Orcamento");

            Property(l => l.Id)
                .HasColumnName("codigo")
                .HasColumnOrder(1);

            Property(l => l.Numero)
                .HasMaxLength(18)
                .HasColumnName("numero")
                .HasColumnOrder(2);

            Property(l => l.EmpresaId)
                .HasColumnName("empresa")
                .HasColumnOrder(3);

            HasOptional<Empresa>(l => l.Empresa)
                .WithMany(l => l.ListaObra)
                .HasForeignKey(l => l.EmpresaId);

            Property(l => l.Descricao)
                .HasMaxLength(50)
                .HasColumnName("descricao")
                .HasColumnOrder(4);

            Property(l => l.CodigoCentroCusto)
                .HasColumnName("centroCusto")
                .HasColumnOrder(5);

            HasOptional(l => l.CentroCusto)
                .WithMany(l => l.ListaObra)
                .HasForeignKey(l => l.CodigoCentroCusto);

            Property(l => l.OrcamentoSimplificado)
                .HasColumnName("orcamentoSimplificado")
                .HasColumnType("bit")
                .HasColumnOrder(6);

            Property(l => l.BDIPercentual)
                .HasColumnName("BDIPercentual")
                .HasPrecision(18,5)
                .HasColumnOrder(9);

            Property(l => l.AreaConstrucaoAreaReal)
                .HasColumnName("areaConstrucaoAreaReal")
                .HasPrecision(18, 5)
                .HasColumnOrder(26);

            Property(l => l.AreaConstrucaoAreaEquivalente)
                .HasColumnName("areaConstrucaoAreaEquivalente")
                .HasPrecision(18, 5)
                .HasColumnOrder(28);

            HasMany<Domain.Entity.Orcamento.Orcamento>(l => l.ListaOrcamento)
                .WithRequired(l => l.Obra)
                .HasForeignKey(c => c.ObraId);


        }
    }
}