using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Contrato;
using GIR.Sigim.Domain.Entity.Financeiro;

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

            HasRequired<Domain.Entity.Contrato.Contrato>(l => l.Contrato)
                .WithMany(c => c.ListaContratoRetificacaoProvisao)
                .HasForeignKey(l => l.ContratoId);

            Property(l => l.ContratoRetificacaoId)
                .IsRequired()
                .HasColumnName("contratoRetificacao")
                .HasColumnOrder(3);

            HasRequired<ContratoRetificacao>(l => l.ContratoRetificacao)
                .WithMany(c => c.ListaContratoRetificacaoProvisao)
                .HasForeignKey(l => l.ContratoRetificacaoId);

            Property(l => l.ContratoRetificacaoItemId)
                .HasColumnName("contratoRetificacaoItem")
                .HasColumnOrder(4);

            HasOptional<ContratoRetificacaoItem>(l => l.ContratoRetificacaoItem)
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
            HasOptional<ContratoRetificacaoItemCronograma>(l => l.ContratoRetificacaoItemCronograma)
                .WithMany(c => c.ListaContratoRetificacaoProvisao)
                .HasForeignKey(l => l.ContratoRetificacaoItemCronogramaId); 
               //.WithRequired(c => c.ContratoRetificacaoProvisao);

            Property(l => l.SequencialCronograma)
                .HasColumnName("sequencialCronograma")
                .HasColumnOrder(7);

            Property(l => l.TituloPagarId)
                .HasColumnName("tituloPagar")
                .HasColumnOrder(8);

            HasOptional<TituloPagar>(l => l.TituloPagar)
                .WithRequired(c => c.ContratoRetificacaoProvisao);

            Property(l => l.TituloReceberId)
                .HasColumnName("tituloReceber")
                .HasColumnOrder(9);

            HasOptional<TituloReceber>(l => l.TituloReceber)
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

            Property(l => l.PagamentoAntecipado)
                .HasColumnName("pagamentoAntecipado")
                .HasColumnType("bit")
                .HasColumnOrder(12);

            Property(l => l.ValorAdiantadoDescontado)
                .HasColumnName("valorAdiantadoDescontado")
                .HasPrecision(18, 5)
                .HasColumnOrder(13);

            Property(l => l.DataAntecipacao)
                .HasColumnName("dataAntecipacao")
                .HasColumnOrder(14);


            Property(l => l.UsuarioAntecipacao)
                .HasColumnName("usuarioAntecipacao")
                .HasMaxLength(50) 
                .HasColumnOrder(15);

            Property(l => l.DocumentoAntecipacao)
                .HasColumnName("documentoAntecipacao")
                .HasMaxLength(100) 
                .HasColumnOrder(16);



        }

    }
}
