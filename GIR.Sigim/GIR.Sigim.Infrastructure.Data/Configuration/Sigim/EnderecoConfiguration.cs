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
    public class EnderecoConfiguration : EntityTypeConfiguration<Endereco>
    {
        public EnderecoConfiguration()
        {
            ToTable("Endereco", "Sigim");

            Property(l => l.Id)
                .HasColumnName("codigo")
                .HasColumnOrder(1);

            Property(l => l.TipoLogradouro)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("tipoLogradouro")
                .HasColumnOrder(2);

            Property(l => l.Logradouro)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("logradouro")
                .HasColumnOrder(3);

            Property(l => l.Cidade)
                .HasMaxLength(50)
                .HasColumnName("cidade")
                .HasColumnOrder(4);

            Property(l => l.Numero)
                .HasMaxLength(50)
                .HasColumnName("numero")
                .HasColumnOrder(5);

            Property(l => l.Complemento)
                .HasMaxLength(50)
                .HasColumnName("complemento")
                .HasColumnOrder(6);

            Property(l => l.UnidadeFederacaoSigla)
                .HasMaxLength(2)
                .HasColumnName("unidadeFederacao")
                .HasColumnOrder(7);

            HasOptional<UnidadeFederacao>(l => l.UnidadeFederacao)
                .WithMany(l => l.ListaEndereco)
                .HasForeignKey(l => l.UnidadeFederacaoSigla);

            Property(l => l.Cep)
                .HasMaxLength(10)
                .HasColumnName("cep")
                .HasColumnOrder(8);

            Property(l => l.Bairro)
                .HasMaxLength(50)
                .HasColumnName("bairro")
                .HasColumnOrder(9);

            Property(l => l.Telefone)
                .HasMaxLength(50)
                .HasColumnName("telefone")
                .HasColumnOrder(10);

            Property(l => l.TelefoneCelular)
                .HasMaxLength(50)
                .HasColumnName("telefoneCelular")
                .HasColumnOrder(11);

            Property(l => l.Email)
                .HasMaxLength(50)
                .HasColumnName("email")
                .HasColumnOrder(12);

            Property(l => l.Fax)
                .HasMaxLength(50)
                .HasColumnName("fax")
                .HasColumnOrder(13);

            Property(l => l.MunicipioId)
                .HasColumnName("municipio")
                .HasColumnOrder(14);

        }

    }
}
