using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Contrato;

namespace GIR.Sigim.Infrastructure.Data.Configuration.Contrato
{
    public class LicitacaoConfiguration : EntityTypeConfiguration<Licitacao>
    {
        public LicitacaoConfiguration()
        {
            ToTable("Licitacao","Contrato");

            Property(l => l.Id)
                .HasColumnName("codigo")
                .HasColumnOrder(1);

            Property(l => l.CodigoCentroCusto)
                .HasMaxLength(18)
                .HasColumnName("centroCusto")
                .HasColumnOrder(2);

            HasRequired(l => l.CentroCusto)
                .WithMany(l => l.ListaLicitacao)
                .HasForeignKey(l => l.CodigoCentroCusto);

            Property(l => l.LicitacaoCronogramaId)
                .HasColumnName("licitacaoCronograma")
                .HasColumnOrder(3);

            HasRequired(l => l.LicitacaoCronograma)
                .WithMany(l => l.ListaLicitacao)
                .HasForeignKey(l => l.LicitacaoCronogramaId);

            Property(l => l.DataLicitacao)
                .HasColumnName("dataLicitacao")
                .HasColumnOrder(4);

            Property(l => l.Situacao)
                .HasColumnName("situacao")
                .HasColumnOrder(5);

             Property(l => l.Observacao)
                .HasColumnName("observacao")
                .HasMaxLength(255) 
                .HasColumnOrder(6);

            Property(l => l.ClienteFornecedorId)
                .HasColumnName("clienteFornecedor")
                .HasColumnOrder(7);

            HasOptional(l => l.ClienteFornecedor)
                .WithMany(l => l.ListaLicitacao)
                .HasForeignKey(l => l.ClienteFornecedorId);

            Property(l => l.DataLimiteEmail)
                .HasColumnName("dataLimiteEmail")
                .HasColumnOrder(8);

            Property(l => l.ReferenciaDigital)
                .HasColumnName("referenciaDigital")
                .HasMaxLength(255) 
                .HasColumnOrder(9);

            Property(l => l.DataCadastro)
                .HasColumnName("dataCadastro")
                .HasColumnOrder(10);

            Property(l => l.UsuarioCadastro)
                .HasColumnName("usuarioCadastro")
                .HasMaxLength(50) 
                .HasColumnOrder(11);

            Property(l => l.DataCancela)
                .HasColumnName("dataCancela")
                .HasColumnOrder(12);

            Property(l => l.UsuarioCancela)
                .HasColumnName("usuarioCancela")
                .HasMaxLength(50) 
                .HasColumnOrder(13);

            Property(l => l.MotivoCancela)
                .HasColumnName("motivoCancela")
                .HasMaxLength(255) 
                .HasColumnOrder(14);
        }


    }
}




