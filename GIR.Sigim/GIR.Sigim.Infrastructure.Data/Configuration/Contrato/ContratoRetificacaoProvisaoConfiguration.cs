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
    public class ContratoRetificacaoProvisaoConfiguration : EntityTypeConfiguration<ContratoRetificacaoProvisao>
    {
        public ContratoRetificacaoProvisaoConfiguration()
        {
            ToTable("ContratoRetificacaoProvisao", "Contrato");

            Property(l => l.Id)
                .HasColumnName("codigo")
                .HasColumnOrder(1);

            Property(l => l.ContratoId)
                .IsRequired() 
                .HasColumnName("contrato")
                .HasColumnOrder(2);

            Property(l => l.ContratoRetificacaoId)
                .IsRequired()
                .HasColumnName("contratoRetificacao")
                .HasColumnOrder(3);

            HasRequired<Domain.Entity.Contrato.ContratoRetificacao>(l => l.ContratoRetificacao)
                .WithMany(c => c.ListaContratoRetificacaoProvisao)
                .HasForeignKey(l => l.ContratoRetificacaoId);

            Property(l => l.ContratoRetificacaoItemId)
                .HasColumnName("contratoRetificacaoItem")
                .HasColumnOrder(4);

            HasOptional<Domain.Entity.Contrato.ContratoRetificacaoItem>(l => l.ContratoRetificacaoItem)
                .WithMany(c => c.ListaContratoRetificacaoProvisao)
                .HasForeignKey(l => l.ContratoRetificacaoItemId);

            Property(l => l.SequencialItem)
                .HasColumnName("sequencialItem")
                .HasColumnOrder(5);

            Property(l => l.ContratoRetificacaoItemCronogramaId)
                .HasColumnName("contratoRetificacaoItemCronograma")
                .HasColumnOrder(6);

            //TODO: O RELACIONAMENTO DEVERIA SER 1 PARA 1 , ONDE A CHAVE PRIMARIA DE CONTRATORETIFICACAOITEMCRONOGRAMA
            //      DEVERIA SER FOREING KEY PARA A CHAVE PRIMARIA DE CONTRATORETIFICACAOPROVISAO, MAS DEVIDO
            //      A MODELAGEM DA TABELA ELA É 1 PARA MUITOS
            HasOptional<Domain.Entity.Contrato.ContratoRetificacaoItemCronograma>(l => l.ContratoRetificacaoItemCronograma)
                .WithMany(c => c.ListaContratoRetificacaoProvisao)
                .HasForeignKey(l => l.ContratoRetificacaoItemCronogramaId); 
               //.WithRequired(c => c.ContratoRetificacaoProvisao);

            Property(l => l.SequencialCronograma)
                .HasColumnName("sequencialCronograma")
                .HasColumnOrder(7);

            Property(l => l.TituloPagarId)
                .HasColumnName("tituloPagar")
                .HasColumnOrder(8);

            HasOptional<Domain.Entity.Financeiro.TituloPagar>(l => l.TituloPagar)
                .WithRequired(c => c.ContratoRetificacaoProvisao);

            Property(l => l.TituloReceberId)
                .HasColumnName("tituloReceber")
                .HasColumnOrder(9);

            HasOptional<Domain.Entity.Financeiro.TituloReceber>(l => l.TituloReceber)
                .WithRequired(c => c.ContratoRetificacaoProvisao);

            Property(l => l.Valor)
                .HasColumnName("valor")
                .IsRequired()
                .HasPrecision(18, 5)
                .HasColumnOrder(10);

            Property(l => l.Quantidade)
                .HasColumnName("quantidade")
                .IsRequired()
                .HasPrecision(18, 7)
                .HasColumnOrder(11);

        }

    }
}
