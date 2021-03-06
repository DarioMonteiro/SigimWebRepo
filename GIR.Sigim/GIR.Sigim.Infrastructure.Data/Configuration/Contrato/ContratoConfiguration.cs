﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration; 
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Contrato;
using GIR.Sigim.Domain.Entity.Sigim;

namespace GIR.Sigim.Infrastructure.Data.Configuration.Contrato
{
    public class ContratoConfiguration : EntityTypeConfiguration<Domain.Entity.Contrato.Contrato>
    {
        public ContratoConfiguration()
        {
            ToTable("Contrato", "Contrato");

            Property(l => l.Id)
                .HasColumnName("codigo")
                .HasColumnOrder(1);

            Property(l => l.CodigoCentroCusto)
                .IsRequired() 
                .HasMaxLength(18)
                .HasColumnName("centroCusto")
                .HasColumnOrder(2);

            HasRequired<Domain.Entity.Financeiro.CentroCusto>(l => l.CentroCusto)
                .WithMany(c => c.ListaContrato)
                .HasForeignKey(l => l.CodigoCentroCusto);

            Property(l => l.LicitacaoId)
                .HasColumnName("licitacao")
                .HasColumnOrder(3);

            HasOptional<Licitacao>(l => l.Licitacao)
                .WithMany(c => c.ListaContrato)
                .HasForeignKey(l => l.LicitacaoId);

            Property(l => l.ContratanteId)
                .IsRequired()
                .HasColumnName("contratante")
                .HasColumnOrder(4);

            HasRequired<Domain.Entity.Sigim.ClienteFornecedor>(l => l.Contratante)
                .WithMany(c => c.ListaContratoContratante)
                .HasForeignKey(l => l.ContratanteId);

            Property(l => l.ContratadoId)
                .IsRequired() 
                .HasColumnName("contratado")
                .HasColumnOrder(5);

            HasRequired<Domain.Entity.Sigim.ClienteFornecedor>(l => l.Contratado)
                .WithMany(c => c.ListaContratoContratado)
                .HasForeignKey(l => l.ContratadoId);

            Property(l => l.IntervenienteId)
                .HasColumnName("interveniente")
                .HasColumnOrder(6);

            HasOptional<Domain.Entity.Sigim.ClienteFornecedor>(l => l.Interveniente)
                .WithMany(c => c.ListaContratoInterveniente)
                .HasForeignKey(l => l.IntervenienteId);

            Property(l => l.ContratoDescricaoId)
                .IsRequired() 
                .HasColumnName("contratoDescricao")
                .HasColumnOrder(7);

            HasRequired<LicitacaoDescricao>(l => l.ContratoDescricao)
                .WithMany(c => c.ListaContrato)
                .HasForeignKey(l => l.ContratoDescricaoId);

            Property(l => l.Situacao)
                .IsRequired() 
                .HasColumnType("smallint") 
                .HasColumnName("situacao")
                .HasColumnOrder(8);

            Property(l => l.DataAssinatura)
                .HasColumnName("dataAssinatura")
                .HasColumnOrder(9);

            Property(l => l.DocumentoOrigem)
                .HasColumnName("documentoOrigem")
                .HasMaxLength(10) 
                .HasColumnOrder(10);

            Property(l => l.NumeroEmpenho)
                .HasColumnName("numeroEmpenho")
                .HasMaxLength(10) 
                .HasColumnOrder(11);

            Property(l => l.ValorContrato)
                .HasColumnName("valorContrato")
                .HasPrecision(18, 5)
                .HasColumnOrder(12);

            Property(l => l.DataCadastro)
                .HasColumnName("dataCadastro")
                .IsRequired()
                .HasColumnOrder(13);

            Property(l => l.UsuarioCadastro)
                .HasColumnName("usuarioCadastro")
                .HasMaxLength(50) 
                .HasColumnOrder(14);

            Property(l => l.DataCancela)
                .HasColumnName("dataCancela")
                .HasColumnOrder(15);

            Property(l => l.UsuarioCancela)
                .HasColumnName("usuarioCancela")
                .HasMaxLength(50) 
                .HasColumnOrder(16);

            Property(l => l.MotivoCancela)
                .HasColumnName("motivoCancela")
                .HasMaxLength(255) 
                .HasColumnOrder(17);

            Property(l => l.TipoContrato)
                .IsRequired() 
                .HasColumnName("tipoContrato")
                .HasColumnOrder(18);

            HasMany<ContratoRetificacao>(l => l.ListaContratoRetificacao)
                .WithRequired(c => c.Contrato)
                .HasForeignKey(c => c.ContratoId);

            HasMany<ContratoRetificacaoItem>(l => l.ListaContratoRetificacaoItem)
                .WithRequired(c => c.Contrato)
                .HasForeignKey(c => c.ContratoId);

            HasMany<ContratoRetificacaoItemMedicao>(l => l.ListaContratoRetificacaoItemMedicao)
                .WithRequired(c => c.Contrato)
                .HasForeignKey(c => c.ContratoId);

            HasMany<ContratoRetificacaoItemCronograma>(l => l.ListaContratoRetificacaoItemCronograma)
                .WithRequired(c => c.Contrato)
                .HasForeignKey(c => c.ContratoId);

            HasMany<ContratoRetificacaoItemImposto>(l => l.ListaContratoRetificacaoItemImposto)
                .WithRequired(c => c.Contrato)
                .HasForeignKey(c => c.ContratoId);  

            HasMany<ContratoRetificacaoProvisao>(l => l.ListaContratoRetificacaoProvisao)
                .WithRequired(c => c.Contrato)
                .HasForeignKey(c => c.ContratoId);

            HasMany<ContratoRetencao>(l => l.ListaContratoRetencao)
                .WithRequired(c => c.Contrato)
                .HasForeignKey(c => c.ContratoId);

            HasMany<AvaliacaoFornecedor>(l => l.ListaAvaliacaoFornecedor)
                .WithOptional(c => c.Contrato)
                .HasForeignKey(c => c.ContratoId);  


        }


    }
}
