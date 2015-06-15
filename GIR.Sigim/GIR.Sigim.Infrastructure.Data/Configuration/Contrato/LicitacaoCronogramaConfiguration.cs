using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration; 
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Contrato;
using GIR.Sigim.Domain.Entity.Financeiro;

namespace GIR.Sigim.Infrastructure.Data.Configuration.Contrato
{
    public class LicitacaoCronogramaConfiguration : EntityTypeConfiguration<LicitacaoCronograma> 
    {
        public LicitacaoCronogramaConfiguration()
        {
            ToTable("LicitacaoCronograma", "Contrato");

            Property(l => l.Id)
                .HasColumnName("codigo")
                .HasColumnOrder(1);

            Property(l => l.CodigoCentroCusto)
                .HasMaxLength(18)
                .HasColumnName("centroCusto")
                .HasColumnOrder(2);

            HasRequired<CentroCusto>(l => l.CentroCusto)
                .WithMany(c => c.ListaLicitacaoCronograma)
                .HasForeignKey(l => l.CodigoCentroCusto);

            Property(l => l.LicitacaoDescricaoId)
                .HasColumnName("licitacaoDescricao")
                .HasColumnOrder(3);

            HasRequired<LicitacaoDescricao>(l => l.LicitacaoDescricao)
                .WithMany(c => c.ListaLicitacaoCronograma)
                .HasForeignKey(l => l.LicitacaoDescricaoId);

            Property(l => l.DataInicioCartaConvite)
                .HasColumnName("dataInicioCartaConvite")
                .IsRequired()
                .HasColumnOrder(4);

            Property(l => l.DataFimCartaConvite)
                .HasColumnName("dataFimCartaConvite")
                .IsRequired()
                .HasColumnOrder(5);

            Property(l => l.DataInicioQuadroComparativo )
                .HasColumnName("dataInicioQuadroComparativo")
                .IsRequired()
                .HasColumnOrder(6);

            Property(l => l.DataFimQuadroComparativo)
                .HasColumnName("dataFimQuadroComparativo")
                .IsRequired()
                .HasColumnOrder(7);

            Property(l => l.DataInicioAssinatura)
                .HasColumnName("dataInicioAssinatura")
                .IsRequired()
                .HasColumnOrder(8);

            Property(l => l.DataFimAssinatura)
                .HasColumnName("dataFimAssinatura")
                .IsRequired()
                .HasColumnOrder(9);

            Property(l => l.DataInicioServico)
                .HasColumnName("dataInicioServico")
                .IsRequired()
                .HasColumnOrder(10);

            Property(l => l.PrazoFabricacao)
                .HasColumnName("prazoFabricacao")
                .HasColumnOrder(11);

            Property(l => l.DuracaoCartaConvite )
                .HasColumnName("duracaoCartaConvite")
                .IsRequired()
                .HasColumnOrder(12);

            Property(l => l.DuracaoQuadroComparativo)
                .HasColumnName("duracaoQuadroComparativo")
                .IsRequired()
                .HasColumnOrder(13);

            Property(l => l.DuracaoAssinatura)
                .HasColumnName("duracaoAssinatura")
                .IsRequired()
                .HasColumnOrder(14);

            Property(l => l.DataInicioCartaConviteRealizado)
                .HasColumnName("dataInicioCartaConviteRealizado")
                .HasColumnOrder(15);

            Property(l => l.DataFimCartaConviteRealizado)
                .HasColumnName("dataFimCartaConviteRealizado")
                .HasColumnOrder(16);

            Property(l => l.DataInicioQuadroComparativoRealizado)
                .HasColumnName("dataInicioQuadroComparativoRealizado")
                .HasColumnOrder(17);

            Property(l => l.DataFimQuadroComparativoRealizado)
                .HasColumnName("dataFimQuadroComparativoRealizado")
                .HasColumnOrder(18);

            Property(l => l.DataInicioAssinaturaRealizado)
                .HasColumnName("dataInicioAssinaturaRealizado")
                .HasColumnOrder(19);

            Property(l => l.DataFimAssinaturaRealizado)
                .HasColumnName("dataFimAssinaturaRealizado")
                .HasColumnOrder(20);

            Property(l => l.DataInicioServicoRealizado)
                .HasColumnName("dataInicioServicoRealizado")
                .HasColumnOrder(21);

            HasMany<Licitacao>(l => l.ListaLicitacao)
                .WithRequired(c => c.LicitacaoCronograma)
                .HasForeignKey(c => c.LicitacaoCronogramaId);
        }
    }
}
