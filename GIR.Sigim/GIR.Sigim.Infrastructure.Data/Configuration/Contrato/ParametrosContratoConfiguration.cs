using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Contrato;

namespace GIR.Sigim.Infrastructure.Data.Configuration.Contrato
{
    public class ParametrosContratoConfiguration : EntityTypeConfiguration<ParametrosContrato>
    {
        public ParametrosContratoConfiguration()
        {
            ToTable("Parametros", "Contrato");

            Property(l => l.Id)
                .HasColumnName("codigo")
                .HasColumnOrder(1);

            Property(l => l.ClienteId)
                .HasColumnName("cliente")
                .HasColumnOrder(2);

            HasOptional<Domain.Entity.Sigim.ClienteFornecedor>(l => l.Cliente)
                .WithMany(l => l.ListaParametrosContrato)
                .HasForeignKey(l => l.ClienteId);

            Property(l => l.MascaraClasseInsumo)
                .HasMaxLength(18)
                .HasColumnName("mascaraClasseInsumo")
                .HasColumnOrder(4);

            Property(l => l.IconeRelatorio)
                .HasColumnType("image")
                .HasColumnName("iconeRelatorio")
                .HasColumnOrder(5);

            Property(l => l.GeraTituloAguardando)
                .HasColumnName("geraTituloAguardando")
                .HasColumnType("bit")
                .HasColumnOrder(7);

            Property(l => l.DiasMedicao)
                .HasColumnName("diasMedicao")
                .HasColumnOrder(23);

            Property(l => l.DiasPagamento)
                .HasColumnName("diasPagamento")
                .HasColumnOrder(24);

            Property(l => l.DadosSped)
                .HasColumnName("dadosSped")
                .HasColumnType("bit")
                .HasColumnOrder(25);

        }
    }
}