﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Sigim;

namespace GIR.Sigim.Infrastructure.Data.Configuration.Sigim
{
    public class CSTConfiguration : EntityTypeConfiguration<CST>
    {

        public CSTConfiguration()
        {
            ToTable("CST", "Sigim");

            Ignore(l => l.Id);

            HasKey(l => l.Codigo);

            Property(l => l.Codigo)
                .IsRequired()
                .HasMaxLength(10)
                .HasColumnName("codigo")
                .HasColumnOrder(1);

            Property(l => l.Descricao)
                .HasColumnName("descricao")
                .HasMaxLength(50)
                .HasColumnOrder(2); 

        }
    }
}
