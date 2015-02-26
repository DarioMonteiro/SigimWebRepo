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
    public class ParametrosOrcamentoConfiguration : EntityTypeConfiguration<ParametrosOrcamento>
    {
        public ParametrosOrcamentoConfiguration()
        {
            ToTable("Parametros", "Orcamento");

            Property(l => l.Id)
                .HasColumnName("codigo")
                .HasColumnOrder(1);

            Property(l => l.MascaraClasseInsumo)
                .HasMaxLength(18)
                .HasColumnName("mascaraClasseInsumo")
                .HasColumnOrder(2);

            Property(l => l.EmpresaNomeRaiz)
                .HasMaxLength(50)
                .HasColumnName("empresaNomeRaiz")
                .HasColumnOrder(3);

            Property(l => l.EmpresaResponsavel)
                .HasMaxLength(50)
                .HasColumnName("empresaResponsavel")
                .HasColumnOrder(4);

            Property(l => l.IconeRelatorio)
                .HasColumnType("image")
                .HasColumnName("iconeRelatorio")
                .HasColumnOrder(5);
        }
    }
}