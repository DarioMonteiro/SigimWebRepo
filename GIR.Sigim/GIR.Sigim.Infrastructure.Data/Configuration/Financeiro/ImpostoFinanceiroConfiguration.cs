﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Financeiro;
using GIR.Sigim.Domain.Entity.Sigim;

namespace GIR.Sigim.Infrastructure.Data.Configuration.Financeiro
{
    public class ImpostoFinanceiroConfiguration : EntityTypeConfiguration<ImpostoFinanceiro>
    {
        public ImpostoFinanceiroConfiguration()
        {
            ToTable("ImpostoFinanceiro", "Financeiro");

            Property(l => l.Id)
                .HasColumnName("codigo")
                .HasColumnOrder(1);

            Property(l => l.Sigla)
                .HasColumnName("sigla")
                .IsRequired()
                .HasMaxLength(10)
                .HasColumnOrder(2);

            Property(l => l.Descricao)
                .HasColumnName("descricao")
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnOrder(3);

            Property(l => l.Aliquota)
                .IsRequired()
                .HasColumnName("aliquota")
                .HasPrecision(18, 5)
                .HasColumnOrder(4);

            Property(l => l.Retido)
                .HasColumnName("retido")
                .HasColumnType("bit")
                .HasColumnOrder(5);

            Property(l => l.Indireto)
                .HasColumnName("indireto")
                .HasColumnType("bit")
                .HasColumnOrder(6);

            Property(l => l.PagamentoEletronico)
                .HasColumnName("pagamentoEletronico")
                .HasColumnType("bit")
                .HasColumnOrder(7);

            Property(l => l.TipoCompromissoId)
                .HasColumnName("tipoCompromisso")
                .HasColumnOrder(8);

            HasOptional<TipoCompromisso>(l => l.TipoCompromisso)
                .WithMany(c => c.ListaImpostoFinanceiro)
                .HasForeignKey(l => l.TipoCompromissoId);

            Property(l => l.ClienteId)
                .HasColumnName("cliente")
                .HasColumnOrder(9);

            HasOptional<ClienteFornecedor>(l => l.Cliente)
                .WithMany(c => c.ListaImpostoFinanceiro)
                .HasForeignKey(l => l.ClienteId);

            Property(l => l.ContaContabil)
                .HasColumnName("contaContabil")
                .HasMaxLength(20)
                .HasColumnOrder(10);

            Property(l => l.Periodicidade)
                .HasColumnName("periodicidade")
                .HasColumnType("char")
                .HasMaxLength(2)
                .HasColumnOrder(11);

            Property(l => l.FimDeSemana)
                .HasColumnName("fimDeSemana")
                .HasColumnType("char")
                .HasMaxLength(2)
                .HasColumnOrder(12);

            Property(l => l.FatoGerador)
                .HasColumnName("fatoGerador")
                .HasColumnType("char")
                .HasMaxLength(2)
                .HasColumnOrder(13);

            Property(l => l.DiaVencimento)
                .HasColumnName("DiaVencimento")
                .HasColumnType("smallint")
                .HasColumnOrder(14);

        }
    }
}
