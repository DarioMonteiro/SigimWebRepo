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
    public class AbstractRequisicaoMaterialConfiguration<T> : EntityTypeConfiguration<T> where T : AbstractRequisicaoMaterial
    {
        public AbstractRequisicaoMaterialConfiguration()
        {
            Property(l => l.Id)
                .HasColumnName("codigo");

            Property(l => l.Observacao)
                .HasMaxLength(255)
                .HasColumnName("observacao");

            Property(l => l.DataCadastro)
                .HasColumnName("dataCadastro");

            Property(l => l.LoginUsuarioCadastro)
                .HasMaxLength(50)
                .HasColumnName("usuarioCadastro");

            Property(l => l.DataCancelamento)
                .HasColumnName("dataCancela");

            Property(l => l.LoginUsuarioCancelamento)
                .HasMaxLength(50)
                .HasColumnName("usuarioCancela");

            Property(l => l.MotivoCancelamento)
                .HasMaxLength(255)
                .HasColumnName("motivoCancela");
        }
    }
}