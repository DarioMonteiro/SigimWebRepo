using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.OrdemCompra;
using GIR.Sigim.Domain.Entity.Orcamento;

namespace GIR.Sigim.Infrastructure.Data.Configuration.OrdemCompra
{
    public class RequisicaoMaterialItemConfiguration : AbstractRequisicaoMaterialItemConfiguration<RequisicaoMaterialItem>
    {
        public RequisicaoMaterialItemConfiguration()
        {
            ToTable("RequisicaoMaterialItem", "OrdemCompra");

            Property(l => l.RequisicaoMaterialId)
                .HasColumnName("RequisicaoMaterial");

            HasRequired(l => l.RequisicaoMaterial)
                .WithMany(l => l.ListaItens);

            HasRequired(l => l.Material)
                .WithMany(l => l.ListaRequisicaoMaterialItem);

            HasRequired(l => l.Classe)
                .WithMany(l => l.ListaRequisicaoMaterialItem)
                .HasForeignKey(l => l.CodigoClasse);

            Property(l => l.Situacao)
                .HasColumnName("situacao");

            Property(l => l.PreRequisicaoMaterialItemId)
                .HasColumnName("preRequisicaoMaterialItem");

            HasOptional(l => l.PreRequisicaoMaterialItem)
                .WithMany(l => l.ListaRequisicaoMaterialItem);

            HasMany<CotacaoItem>(l => l.ListaCotacaoItem)
                .WithRequired(l => l.RequisicaoMaterialItem)
                .HasForeignKey(c => c.RequisicaoMaterialItemId);

            HasMany<OrdemCompraItem>(l => l.ListaOrdemCompraItem)
                .WithOptional(l => l.RequisicaoMaterialItem)
                .HasForeignKey(c => c.RequisicaoMaterialItemId);

            HasMany<OrcamentoInsumoRequisitado>(l => l.ListaOrcamentoInsumoRequisitado)
                .WithOptional(l => l.RequisicaoMaterialItem)
                .HasForeignKey(c => c.RequisicaoMaterialItemId);

        }
    }
}