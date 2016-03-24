using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using GIR.Sigim.Domain.Entity.Comercial;

namespace GIR.Sigim.Infrastructure.Data.Configuration.Comercial
{
    public class IncorporadorConfiguration : EntityTypeConfiguration<Incorporador>
    {
        public IncorporadorConfiguration()
        {
            ToTable("Incorporador", "Comercial");

            Property(l => l.Id)
                .HasColumnName("codigo")
                .HasColumnOrder(1);

            Property(l => l.RazaoSocial)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("razaoSocial")
                .HasColumnOrder(2);

            Property(l => l.TipoPessoa)
                .HasMaxLength(1)
                .HasColumnName("tipoPessoa")
                .HasColumnOrder(3);

            Property(l => l.Cnpj)
                .HasMaxLength(20)
                .HasColumnName("cnpj")
                .HasColumnOrder(4);

            Property(l => l.InscricaoMunicipal )
                .HasMaxLength(20)
                .HasColumnName("inscricaoMunicipal")
                .HasColumnOrder(5);

            Property(l => l.InscricaoEstadual)
                .HasMaxLength(20)
                .HasColumnName("inscricaoEstadual")
                .HasColumnOrder(6);

            Property(l => l.CodigoSUFRAMA)
               .HasMaxLength(50)
               .HasColumnName("codigoSUFRAMA")
               .HasColumnOrder(7);
                       

        }
    }
}
