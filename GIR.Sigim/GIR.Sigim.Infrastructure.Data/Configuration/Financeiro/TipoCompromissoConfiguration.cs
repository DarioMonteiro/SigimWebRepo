using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Financeiro;
using GIR.Sigim.Domain.Entity.OrdemCompra;
using GIR.Sigim.Domain.Entity.Contrato;

namespace GIR.Sigim.Infrastructure.Data.Configuration.Financeiro
{
    public class TipoCompromissoConfiguration : EntityTypeConfiguration<TipoCompromisso>
    {
        public TipoCompromissoConfiguration()
        {
            ToTable("TipoCompromisso", "Financeiro");

            Property(l => l.Id)
                .HasColumnName("codigo")
                .HasColumnOrder(1);

            Property(l => l.Descricao)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("descricao")
                .HasColumnOrder(2);

            Property(l => l.GeraTitulo)
                .HasColumnName("geraTitulo")
                .HasColumnType("bit") 
                .HasColumnOrder(3);

            Property(l => l.TipoPagar)
                .HasColumnName("tipoPagar")
                .HasColumnType("bit") 
                .HasColumnOrder(4);

            Property(l => l.TipoReceber)
                .HasColumnName("tipoReceber")
                .HasColumnType("bit") 
                .HasColumnOrder(5);

            HasMany<ParametrosOrdemCompra>(l => l.ListaParametrosOrdemCompra)
                .WithOptional(l => l.TipoCompromissoFrete)
                .HasForeignKey(c => c.TipoCompromissoFreteId);

            HasMany<ContratoRetificacao>(l => l.ListaContratoRetificacao)
                .WithOptional(c => c.RetencaoTipoCompromisso)
                .HasForeignKey(c => c.RetencaoTipoCompromissoId);

            HasMany<ContratoRetificacaoItem>(l => l.ListaContratoRetificacaoItem)
                .WithOptional(c => c.RetencaoTipoCompromisso)
                .HasForeignKey(c => c.RetencaoTipoCompromissoId);

            HasMany<TituloPagar>(l => l.ListaTituloPagar)
                .WithOptional(c => c.TipoCompromisso)
                .HasForeignKey(c => c.TipoCompromissoId);

            HasMany<TituloReceber>(l => l.ListaTituloReceber)
                .WithOptional(c => c.TipoCompromisso)
                .HasForeignKey(c => c.TipoCompromissoId);

            HasMany<ImpostoFinanceiro>(l => l.ListaImpostoFinanceiro)
                .WithOptional(c => c.TipoCompromisso)
                .HasForeignKey(c => c.TipoCompromissoId);

        }
    }
}