using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Financeiro;

namespace GIR.Sigim.Infrastructure.Data.Configuration.Financeiro
{
    public class ParametrosFinanceiroConfiguration : EntityTypeConfiguration<ParametrosFinanceiro>
    {
        public ParametrosFinanceiroConfiguration()
        {
            ToTable("Parametros", "Financeiro");

            Property(l => l.Id)
                .HasColumnName("codigo")
                .HasColumnOrder(1);

            Property(l => l.ClienteId)
                .HasColumnName("cliente")
                .HasColumnOrder(2);

           Property(l => l.Responsavel)
                .HasMaxLength(30)
                .HasColumnName("responsavel")
                .HasColumnOrder(3);

           Property(l => l.PracaPagamento )
               .HasMaxLength(20)
               .HasColumnName("pracaPagamento")
               .HasColumnOrder(4);

           Property(l => l.Licenca)
              .HasMaxLength(30)
              .HasColumnName("licenca")
              .HasColumnOrder(5);

            Property(l => l.CentroCusto)
                .HasMaxLength(18)
                .HasColumnName("centroCusto")
                .HasColumnOrder(6);

            Property(l => l.Classe)
                .HasMaxLength(18)
                .HasColumnName("classe")
                .HasColumnOrder(7);

            Property(l => l.SituacaoDefaultPagar)
                .HasColumnName("situacaoDefaultPagar")
                .HasColumnOrder(8);

            Property(l => l.SituacaoDefaultReceber)
                .HasColumnName("situacaoDefaultReceber")
                .HasColumnOrder(9);

            Property(l => l.GeraTituloImposto)
                .HasColumnName("geraTituloImposto")
                .HasColumnOrder(10);

            Property(l => l.LeitoraCodigoBarras)
                .HasColumnName("leitoraCodigoBarras")
                .HasColumnOrder(11);

            Property(l => l.ToleranciaRecebimento)
                .HasColumnName("toleranciaRecebimento")
                .HasColumnOrder(12);

            Property(l => l.IconeRelatorio)
                .HasColumnType("image")
                .HasColumnName("iconeRelatorio")
                .HasColumnOrder(13);

            Property(l => l.InterfaceBloqueioTotal)
                .HasColumnName("interfaceBloqueioTotal")
                .HasColumnOrder(14);

            Property(l => l.InterfaceBloqueioParcial)
                .HasColumnName("interfaceBloqueioParcial")
                .HasColumnOrder(15);

            Property(l => l.InterfacePermiteApropriacao)
                .HasColumnName("interfacePermiteApropriacao")
                .HasColumnOrder(16);

            Property(l => l.InterfaceContabil)
                .HasColumnName("interfaceContabil")
                .HasColumnOrder(17);

            Property(l => l.PercentualApropriacao)
                .HasColumnName("percentualApropriacao")
                .HasColumnOrder(18);

            Property(l => l.ConfereNFOC)
                .HasColumnName("confereNFOC")
                .HasColumnOrder(19);

            Property(l => l.ConfereNFCT)
                .HasColumnName("confereNFCT")
                .HasColumnOrder(20);

            Property(l => l.ImpostoAutomatico)
                .HasColumnName("impostoAutomatico")
                .HasColumnOrder(21);

            Property(l => l.ValorLiquidoApropriado)
                .HasColumnName("valorLiquidoApropriado")
                .HasColumnOrder(22);

            Property(l => l.ContaCorrenteCentroCusto)
                .HasColumnName("contaCorrenteCentroCusto")
                .HasColumnOrder(23);

            Property(l => l.BloqueioSituacaoLiberado)
                 .HasMaxLength(255)
                 .HasColumnName("bloqueioSituacaoLiberado")
                 .HasColumnOrder(24);

            Ignore(l => l.Interface);

        }
    }
}
