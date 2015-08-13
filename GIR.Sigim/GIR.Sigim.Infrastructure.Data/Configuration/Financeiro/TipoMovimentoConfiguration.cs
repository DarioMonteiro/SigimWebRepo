using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;   
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Financeiro;
using GIR.Sigim.Domain.Entity.Contrato;

namespace GIR.Sigim.Infrastructure.Data.Configuration.Financeiro
{
    public class TipoMovimentoConfiguration : EntityTypeConfiguration<TipoMovimento>
    {
        public TipoMovimentoConfiguration()
        {
            ToTable("TipoMovimento", "Financeiro");

            Property(l => l.Id)
                .HasColumnName("codigo")
                .HasColumnOrder(1);    

            Property(l => l.Descricao)
                .HasColumnName("descricao")
                .IsRequired()
                .HasMaxLength(50) 
                .HasColumnOrder(2);

            Property(l => l.Operacao)
                .HasColumnName("operacao")
                .IsRequired()
                .HasColumnType("char")
                .HasMaxLength(1)
                .HasColumnOrder(3);

            Property(l => l.Automatico)
                .HasColumnName("automatico")
                .HasColumnType("bit")
                .HasColumnOrder(4);

            Property(l => l.HistoricoContabilId )
                .HasColumnName("historicoContabil")
                .HasColumnOrder(5);

            HasOptional<HistoricoContabil>(l => l.HistoricoContabil)
                .WithMany(l => l.ListaTipoMovimento)
                .HasForeignKey(l => l.HistoricoContabilId);

            Property(l => l.Tipo)
                .HasColumnName("tipo")
                .HasColumnType("char")
                .HasMaxLength(1)
                .IsRequired()
                .HasColumnOrder(6);

            Property(l => l.FormaPagamento)
                .HasColumnName("formaPagamento")
                .HasColumnType("tinyint")
                .HasColumnOrder(7);
        }
    }
}
