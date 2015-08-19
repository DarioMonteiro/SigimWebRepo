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
    public class AgenciaConfiguration : EntityTypeConfiguration<Agencia>
    {
        public AgenciaConfiguration()
        {
            ToTable("Agencia", "Sigim");

            Property(l => l.Id)
                .HasColumnName("codigo")               
                .HasColumnOrder(1);

            Property(l => l.BancoId)                
                .HasColumnName("codigoBC")
                .HasColumnOrder(2);

            HasOptional<Banco>(l => l.Banco)
               .WithMany(c => c.ListaAgencia)
               .HasForeignKey(l => l.BancoId);


            Property(l => l.AgenciaCodigo)
                .HasMaxLength(10)
                .HasColumnName("agencia")
                .HasColumnOrder(3);

            Property(l => l.DVAgencia)
                .HasMaxLength(10)
                .HasColumnName("DVAgencia")
                .HasColumnOrder(4);

            Property(l => l.Nome)
                .HasMaxLength(50)
                .HasColumnName("nome")
                .HasColumnOrder(5);

            Property(l => l.NomeContato)
                .HasMaxLength(50)
                .HasColumnName("nomeContato")
                .HasColumnOrder(6);

            Property(l => l.TelefoneContato)
                .HasMaxLength(50)
                .HasColumnName("telefoneContato")
                .HasColumnOrder(7);

            Property(l => l.TipoLogradouro)
                .HasMaxLength(50)
                .HasColumnName("tipoLogradouro")
                .HasColumnOrder(8);

            Property(l => l.Logradouro)
                .HasMaxLength(50)
                .HasColumnName("logradouro")
                .HasColumnOrder(9);

            Property(l => l.Cidade)
                .HasMaxLength(50)
                .HasColumnName("cidade")
                .HasColumnOrder(10);

            Property(l => l.Numero)
                .HasMaxLength(20)
                .HasColumnName("numero")
                .HasColumnOrder(11);

            Property(l => l.Complemento)
                .HasMaxLength(50)
                .HasColumnName("complemento")
                .HasColumnOrder(12);

            HasMany<ContaCorrente>(l => l.ListaContaCorrente)
                     .WithOptional(c => c.Agencia)
                     .HasForeignKey(c => c.AgenciaId); 


        }
    }
}