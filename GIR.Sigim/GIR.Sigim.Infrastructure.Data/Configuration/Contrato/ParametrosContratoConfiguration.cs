﻿using System;
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

            Property(l => l.MascaraClasseInsumo)
                .HasMaxLength(18)
                .HasColumnName("mascaraClasseInsumo")
                .HasColumnOrder(4);

            Property(l => l.IconeRelatorio)
                .HasColumnType("image")
                .HasColumnName("iconeRelatorio")
                .HasColumnOrder(5);

            Property(l => l.DiasMedicao)
                .HasColumnName("DiasMedicao")
                .HasColumnOrder(23);

            Property(l => l.DiasPagamento)
                .HasColumnName("DiasPagamento")
                .HasColumnOrder(24);
        }
    }
}