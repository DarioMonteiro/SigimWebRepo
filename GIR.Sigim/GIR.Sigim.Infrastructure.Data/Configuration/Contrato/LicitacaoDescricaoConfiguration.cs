﻿using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Contrato;

namespace GIR.Sigim.Infrastructure.Data.Configuration.Contrato
{
    
    public class LicitacaoDescricaoConfiguration : EntityTypeConfiguration<LicitacaoDescricao>
    {
        public LicitacaoDescricaoConfiguration()
        {
            ToTable("LicitacaoDescricao","Contrato");

            Property(l => l.Id)
                .HasColumnName("codigo")
                .HasColumnOrder(1);           

            Property(l => l.Descricao)
               .HasColumnName("descricao")
               .IsRequired()
               .HasMaxLength(100) 
               .HasColumnOrder(2);

            HasMany<Domain.Entity.Contrato.Contrato>(l => l.ListaContrato)
            .WithRequired(c => c.ContratoDescricao)
            .HasForeignKey(c => c.ContratoDescricaoId);

            HasMany<LicitacaoCronograma>(l => l.ListaLicitacaoCronograma)
            .WithRequired(c => c.LicitacaoDescricao)
            .HasForeignKey(c => c.LicitacaoDescricaoId);

        }
    }
}
