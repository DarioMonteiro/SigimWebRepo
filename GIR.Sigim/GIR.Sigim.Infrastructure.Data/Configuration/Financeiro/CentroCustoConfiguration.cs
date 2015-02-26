﻿using System;
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
    public class CentroCustoConfiguration : EntityTypeConfiguration<CentroCusto>
    {
        public CentroCustoConfiguration()
        {
            ToTable("CentroCusto", "Financeiro");

            Ignore(l => l.Id);

            HasKey(l => l.Codigo);

            Property(l => l.Codigo)
                .IsRequired()
                .HasMaxLength(18)
                .HasColumnName("codigo");

            Property(l => l.Descricao)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("descricao");

            Property(l => l.CentroContabil)
                .HasMaxLength(20)
                .HasColumnName("centroContabil");

            Property(l => l.AnoMes)
                .HasColumnName("anoMes");

            Property(l => l.TipoTabela)
                .HasColumnName("tipoTabela");

            Property(l => l.Situacao)
                .IsRequired()
                .HasColumnType("char")
                .HasMaxLength(1)
                .HasColumnName("situacao");

            Ignore(l => l.Ativo);

            Property(l => l.CodigoPai)
                .HasMaxLength(18)
                .HasColumnName("pai");

            HasOptional(l => l.CentroCustoPai)
                .WithMany(l => l.ListaFilhos)
                .HasForeignKey(l => l.CodigoPai);

            HasMany(l => l.ListaPreRequisicaoMaterialItem)
                .WithRequired(l => l.CentroCusto);
        }
    }
}