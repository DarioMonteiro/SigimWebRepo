﻿using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration; 
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Sigim;

namespace GIR.Sigim.Infrastructure.Data.Configuration.Sigim
{
    public class PessoaJuridicaConfiguration : EntityTypeConfiguration<PessoaJuridica>
    {
        public PessoaJuridicaConfiguration()
        {
            ToTable("PessoaJuridica", "Sigim");

            Property(l => l.Id)
                .HasColumnName("cliente");

            Property(l => l.Cnpj)
                .HasColumnName("cnpj");

            Property(l => l.NomeFantasia)
                .HasMaxLength(50)
                .HasColumnName("nomeFantasia");

            Property(l => l.InscricaoEstadual)
                .HasMaxLength(20)
                .HasColumnName("inscricaoEstadual");

            HasRequired(l => l.Cliente).WithRequiredDependent(l => l.PessoaJuridica);
        }
    }
}