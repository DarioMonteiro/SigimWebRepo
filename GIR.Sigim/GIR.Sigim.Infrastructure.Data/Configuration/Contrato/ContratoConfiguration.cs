using System;
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
    public class ContratoConfiguration : EntityTypeConfiguration<Domain.Entity.Contrato.Contrato>
    {
        public ContratoConfiguration()
        {
            ToTable("Contrato", "Contrato");

            Property(l => l.Id)
                .HasColumnName("codigo")
                .HasColumnOrder(1);

            Property(l => l.CodigoCentroCusto)
                .HasMaxLength(18)
                .HasColumnName("centroCusto")
                .HasColumnOrder(2);

            HasRequired(l => l.CentroCusto)
                .WithMany(l => l.ListaContrato)
                .HasForeignKey(l => l.CodigoCentroCusto);

            Property(l => l.LicitacaoId)
                .HasColumnName("licitacao")
                .HasColumnOrder(3);

            HasRequired(l => l.Licitacao)
                .WithMany(l => l.ListaContrato)
                .HasForeignKey(l => l.LicitacaoId);

            Property(l => l.ContratanteId)
                .HasColumnName("contratante")
                .HasColumnOrder(4);

            HasRequired(l => l.Contratante)
                .WithMany(l => l.ListaContratoContratante)
                .HasForeignKey(l => l.ContratanteId);

            Property(l => l.ContratadoId)
                .HasColumnName("contratado")
                .HasColumnOrder(5);

            HasRequired(l => l.Contratado)
                .WithMany(l => l.ListaContratoContratado)
                .HasForeignKey(l => l.ContratadoId);

            Property(l => l.IntervenienteId)
                .HasColumnName("interveniente")
                .HasColumnOrder(6);

            HasRequired(l => l.Interveniente)
                .WithMany(l => l.ListaContratoInterveniente)
                .HasForeignKey(l => l.IntervenienteId);


            Property(l => l.ContratoDescricaoId)
                .HasColumnName("contratoDescricao")
                .HasColumnOrder(7);

            HasRequired(l => l.ContratoDescricao)
                .WithMany(l => l.ListaContrato)
                .HasForeignKey(l => l.ContratoDescricaoId);

            Property(l => l.Situacao)
                .HasColumnName("situacao")
                .HasColumnOrder(8);

            Property(l => l.DataAssinatura)
                .HasColumnName("dataAssinatura")
                .HasColumnOrder(9);

            Property(l => l.DocumentoOrigem)
                .HasColumnName("documentoOrigem")
                .HasColumnOrder(10);

            Property(l => l.NumeroEmpenho)
                .HasColumnName("numeroEmpenho")
                .HasColumnOrder(11);

            Property(l => l.ValorContrato)
                .HasColumnName("valorContrato")
                .HasColumnOrder(12);

            Property(l => l.DataCadastro)
                .HasColumnName("dataCadastro")
                .HasColumnOrder(13);

            Property(l => l.UsuarioCadastro)
                .HasColumnName("usuarioCadastro")
                .HasColumnOrder(14);

            Property(l => l.DataCancela)
                .HasColumnName("dataCancela")
                .HasColumnOrder(15);

            Property(l => l.UsuarioCancela)
                .HasColumnName("usuarioCancela")
                .HasColumnOrder(16);

            Property(l => l.MotivoCancela)
                .HasColumnName("motivoCancela")
                .HasColumnOrder(17);

            Property(l => l.TipoContrato)
                .HasColumnName("tipoContrato")
                .HasColumnOrder(18);

        }


    }
}
