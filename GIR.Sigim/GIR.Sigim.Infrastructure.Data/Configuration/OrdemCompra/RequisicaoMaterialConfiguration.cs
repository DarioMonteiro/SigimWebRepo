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
    public class RequisicaoMaterialConfiguration : AbstractRequisicaoMaterialConfiguration<RequisicaoMaterial>
    {
        public RequisicaoMaterialConfiguration()
        {
            ToTable("RequisicaoMaterial", "OrdemCompra");

            Property(l => l.Data)
                .HasColumnName("dataRequisicao");

            Property(l => l.CodigoCentroCusto)
                .HasMaxLength(18)
                .HasColumnName("centroCusto");

            HasRequired(l => l.CentroCusto)
                .WithMany(l => l.ListaRequisicaoMaterial)
                .HasForeignKey(l => l.CodigoCentroCusto);

            Property(l => l.Situacao)
                .HasColumnName("situacao");

            Property(l => l.DataAprovacao)
                .HasColumnName("dataAprovado");

            Property(l => l.LoginUsuarioAprovacao)
                .HasMaxLength(50)
                .HasColumnName("usuarioAprovado");

            HasMany(l => l.ListaItens)
                .WithRequired(l => l.RequisicaoMaterial);
        }
    }
}