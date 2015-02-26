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
    public class PreRequisicaoMaterialItemConfiguration : AbstractRequisicaoMaterialItemConfiguration<PreRequisicaoMaterialItem>
    {
        public PreRequisicaoMaterialItemConfiguration()
        {
            ToTable("PreRequisicaoMaterialItem", "OrdemCompra");

            Property(l => l.PreRequisicaoMaterialId)
                .HasColumnName("preRequisicaoMaterial");

            HasRequired(l => l.PreRequisicaoMaterial)
                .WithMany(l => l.ListaItens);

            HasRequired(l => l.Material)
                .WithMany(l => l.ListaPreRequisicaoMaterialItem);

            HasRequired(l => l.Classe)
                .WithMany(l => l.ListaPreRequisicaoMaterialItem)
                .HasForeignKey(l => l.CodigoClasse);

            Property(l => l.CodigoCentroCusto)
                .HasMaxLength(18)
                .HasColumnName("centroCusto")
                .HasColumnOrder(5);

            HasRequired(l => l.CentroCusto)
                .WithMany(l => l.ListaPreRequisicaoMaterialItem)
                .HasForeignKey(l => l.CodigoCentroCusto);

            Property(l => l.Situacao)
                .HasColumnName("situacao")
                .HasColumnOrder(13);

            Property(l => l.DataAprovacao)
                .HasColumnName("dataAprova")
                .HasColumnOrder(14);

            Property(l => l.LoginUsuarioAprovacao)
                .HasMaxLength(50)
                .HasColumnName("usuarioAprova")
                .HasColumnOrder(15);
        }
    }
}