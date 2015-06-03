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
    public class OrcamentoInsumoRequisitadoConfiguration : EntityTypeConfiguration<OrcamentoInsumoRequisitado>
    {
        public OrcamentoInsumoRequisitadoConfiguration()
        {
            ToTable("OrcamentoInsumoRequisitado", "Orcamento");

            Property(l => l.Id)
                .HasColumnName("codigo");

            Property(l => l.CodigoCentroCusto)
                .HasColumnName("centroCusto");

            HasRequired(l => l.CentroCusto)
                .WithMany(l => l.ListaOrcamentoInsumoRequisitado)
                .HasForeignKey(l => l.CodigoCentroCusto);

            Property(l => l.CodigoClasse)
                .HasColumnName("classe");

            HasRequired(l => l.Classe)
                .WithMany(l => l.ListaOrcamentoInsumoRequisitado)
                .HasForeignKey(l => l.CodigoClasse);

            Property(l => l.ComposicaoId)
                .HasColumnName("composicao");

            HasOptional(l => l.Composicao)
                .WithMany(l => l.ListaOrcamentoInsumoRequisitado)
                .HasForeignKey(l => l.ComposicaoId);

            Property(l => l.MaterialId)
                .HasColumnName("material");

            HasOptional(l => l.Material)
                .WithMany(l => l.ListaOrcamentoInsumoRequisitado)
                .HasForeignKey(l => l.MaterialId);

            Property(l => l.RequisicaoMaterialItemId)
                .HasColumnName("requisicaoMaterialItem");

            HasOptional(l => l.RequisicaoMaterialItem)
                .WithMany(l => l.ListaOrcamentoInsumoRequisitado)
                .HasForeignKey(l => l.RequisicaoMaterialItemId);

            Property(l => l.Quantidade)
                .HasPrecision(18, 5)
                .HasColumnName("quantidade");
        }
    }
}