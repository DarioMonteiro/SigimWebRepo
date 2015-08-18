using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Sigim;

namespace GIR.Sigim.Infrastructure.Data.Configuration.Sigim
{
    public class ContaCorrenteConfiguration : EntityTypeConfiguration<ContaCorrente>
    {
        public ContaCorrenteConfiguration()
        {
            ToTable("ContaCorrente", "Sigim");

            Property(l => l.Id)
                .HasColumnName("codigo")               
                .HasColumnOrder(1);

            Property(l => l.BancoId)                
                .HasColumnName("codigoBC")
                .HasColumnOrder(2);         

            Property(l => l.AgenciaId)
                .HasColumnName("agencia")
                .HasColumnOrder(3);

            HasOptional<Agencia>(l => l.Agencia)
               .WithMany(c => c.ListaContaCorrente)
               .HasForeignKey(l => l.AgenciaId);

            Property(l => l.ContaCodigo)
                .HasMaxLength(15)
                .HasColumnName("conta")
                .HasColumnOrder(4);

            Property(l => l.DVConta)
                .HasMaxLength(10)
                .HasColumnName("DVConta")
                .HasColumnOrder(5);

            Property(l => l.CodigoEmpresa)
                .HasMaxLength(30)
                .HasColumnName("codigoEmpresa")
                .HasColumnOrder(6);

            Property(l => l.NomeCedente)
                .HasMaxLength(50)
                .HasColumnName("nomeCedente")
                .HasColumnOrder(7);

            Property(l => l.CNPJCedente)
                .HasMaxLength(50)
                .HasColumnName("CNPJCedente")
                .HasColumnOrder(8);
            
            Property(l => l.Descricao)
               .HasMaxLength(50)
               .HasColumnName("descricao")
               .HasColumnOrder(9);

            Property(l => l.Complemento)
                .HasMaxLength(50)
                .HasColumnName("complemento")
                .HasColumnOrder(10);

            Property(l => l.Tipo)
                .HasColumnName("tipo")
                .HasColumnType("tinyint")
                .HasColumnOrder(11);

            Property(l => l.Situacao)
                .HasColumnName("situacao")
                .HasColumnType("char")
                .HasMaxLength(1)
                .HasColumnOrder(12);


            
            Property(l => l.BancoLayoutCobranca)
                .HasColumnName("bancoLayoutCobranca")
                .HasColumnOrder(13);
           
            Property(l => l.BancoLayoutPagamento)
                .HasColumnName("BancoLayoutPagamento")
                .HasColumnOrder(14);     

            Property(l => l.CodigoCentroCusto)
               .HasMaxLength(18)
               .HasColumnName("centroCusto")
               .HasColumnOrder(15);

            HasOptional(l => l.CentroCusto)
                .WithMany(l => l.ListaContaCorrente)
                .HasForeignKey(l => l.CodigoCentroCusto);

            Property(l => l.ContaContabil)
                .HasMaxLength(18)
                .HasColumnName("contaContabil")
                .HasColumnOrder(16);

            Property(l => l.Carteira)
                .HasMaxLength(18)
                .HasColumnName("carteira")
                .HasColumnOrder(17);

             Property(l => l.ImprimeBoleto)               
                .HasColumnName("imprimeBoleto")
                .HasColumnOrder(18);

             Property(l => l.ContaCorrentePenhor)
                 .HasColumnName("contaCorrentePenhor")
                 .HasColumnOrder(19);

            Property(l => l.ContaContabilCredCob)
                .HasMaxLength(18)
                .HasColumnName("contaContabilCredCob")
                .HasColumnOrder(30);                       

            Property(l => l.EnderecoCedente)
                .HasMaxLength(18)
                .HasColumnName("enderecoCedente")
                .HasColumnOrder(31);

        
        

        }
    }
}