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
    public class CotacaoConfiguration : EntityTypeConfiguration<Cotacao>
    {
        public CotacaoConfiguration()
        {
            ToTable("Cotacao", "OrdemCompra");

            Property(l => l.Id)
                .HasColumnName("codigo")
                .HasColumnOrder(1);

            Property(l => l.Data)
                .HasColumnName("dataCotacao")
                .HasColumnOrder(2);

            //TODO: Implementar Situação
            //.HasColumnOrder(3);

            Property(l => l.Observacao)
                .HasMaxLength(255)
                .HasColumnName("observacao")
                .HasColumnOrder(4);

            Property(l => l.DataCadastro)
                .HasColumnName("dataCadastro")
                .HasColumnOrder(5);

            Property(l => l.LoginUsuarioCadastro)
                .HasMaxLength(50)
                .HasColumnName("usuarioCadastro")
                .HasColumnOrder(6);

            Property(l => l.DataCancelamento)
                .HasColumnName("dataCancela")
                .HasColumnOrder(7);

            Property(l => l.LoginUsuarioCancelamento)
                .HasMaxLength(50)
                .HasColumnName("usuarioCancela")
                .HasColumnOrder(8);

            Property(l => l.MotivoCancelamento)
                .HasMaxLength(255)
                .HasColumnName("motivoCancela")
                .HasColumnOrder(9);

            HasMany(l => l.ListaItens)
                .WithRequired(l => l.Cotacao);
        }
    }
}