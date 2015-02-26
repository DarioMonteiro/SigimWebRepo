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
    public class PreRequisicaoMaterialConfiguration : AbstractRequisicaoMaterialConfiguration<PreRequisicaoMaterial>
    {
        public PreRequisicaoMaterialConfiguration()
        {
            ToTable("PreRequisicaoMaterial", "OrdemCompra");

            Property(l => l.Data)
                .HasColumnName("dataPreRequisicao");

            Property(l => l.Situacao)
                .HasColumnName("situacao");

            HasMany(l => l.ListaItens)
                .WithRequired(l => l.PreRequisicaoMaterial);
        }
    }
}