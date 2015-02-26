using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Admin;
using GIR.Sigim.Domain.Entity.OrdemCompra;

namespace GIR.Sigim.Infrastructure.Data.Configuration.OrdemCompra
{
    public class ParametrosUsuarioConfiguration : EntityTypeConfiguration<ParametrosUsuario>
    {
        public ParametrosUsuarioConfiguration()
        {
            ToTable("ParametrosUsuario", "OrdemCompra");

            Property(l => l.Id)
                .HasColumnName("usuario");

            Property(l => l.CodigoCentroCusto)
                .HasColumnName("centroCusto");

            Property(l => l.Email)
                .HasMaxLength(50)
                .HasColumnName("usuarioEmail");

            Property(l => l.Senha)
                .HasMaxLength(50)
                .HasColumnName("usuarioEmailSenha");

            HasOptional(l => l.CentroCusto)
                .WithMany(l => l.ListaParametrosUsuario)
                .HasForeignKey(l => l.CodigoCentroCusto);
        }
    }
}