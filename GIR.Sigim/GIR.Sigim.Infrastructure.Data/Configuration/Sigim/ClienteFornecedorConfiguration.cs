using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Sigim;
using GIR.Sigim.Domain.Entity.OrdemCompra;
using GIR.Sigim.Domain.Entity.Financeiro;
using GIR.Sigim.Domain.Entity.Contrato;

namespace GIR.Sigim.Infrastructure.Data.Configuration.Sigim
{
    public class ClienteFornecedorConfiguration : EntityTypeConfiguration<ClienteFornecedor>
    {
        public ClienteFornecedorConfiguration()
        {
            ToTable("ClienteFornecedor", "Sigim");

            Property(l => l.Id)
                .HasColumnName("codigo")
                .HasColumnOrder(1);

            Property(l => l.Nome)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("nome")
                .HasColumnOrder(2);

            Property(l => l.TipoPessoa)
                .IsRequired()
                .HasColumnType("char")
                .HasMaxLength(1)
                .HasColumnName("tipoPessoa")
                .HasColumnOrder(5);
            
            Property(l => l.Situacao)
                .IsRequired()
                .HasColumnType("char")
                .HasMaxLength(1)
                .HasColumnName("situacao")
                .HasColumnOrder(6);

            Property(l => l.TipoCliente)
                .HasColumnType("char")
                .HasMaxLength(1)
                .HasColumnName("tipoCliente")
                .HasColumnOrder(7);

            Property(l => l.ClienteAPagar)
                .HasColumnType("char")
                .HasMaxLength(1)
                .HasColumnName("clienteAPagar")
                .HasColumnOrder(11);

            Property(l => l.ClienteAReceber)
                .HasColumnType("char")
                .HasMaxLength(1)
                .HasColumnName("clienteAReceber")
                .HasColumnOrder(12);

            Property(l => l.ClienteOrdemCompra)
                .HasColumnType("char")
                .HasMaxLength(1)
                .HasColumnName("clienteOrdemCompra")
                .HasColumnOrder(13);

            Property(l => l.ClienteContrato)
                .HasColumnType("char")
                .HasMaxLength(1)
                .HasColumnName("clienteContrato")
                .HasColumnOrder(14);

            Property(l => l.EnderecoResidencialId)
                .HasColumnName("enderecoResidencial")
                .HasColumnOrder(15);

            HasOptional<Endereco>(l => l.EnderecoResidencial)
                .WithMany(c => c.ListaClienteFornecedorEndResidencial)
                .HasForeignKey(l => l.EnderecoResidencialId);

            Property(l => l.EnderecoComercialId)
                .HasColumnName("enderecoComercial")
                .HasColumnOrder(16);

            HasOptional<Endereco>(l => l.EnderecoComercial)
                .WithMany(c => c.ListaClienteFornecedorEndComercial)
                .HasForeignKey(l => l.EnderecoComercialId);

            Property(l => l.EnderecoOutroId)
                .HasColumnName("enderecoOutro")
                .HasColumnOrder(17);

            HasOptional<Endereco>(l => l.EnderecoOutro)
                .WithMany(c => c.ListaClienteFornecedorEndOutro)
                .HasForeignKey(l => l.EnderecoComercialId);

            Property(l => l.Correspondencia)
                .HasColumnName("correspondencia")
                .HasColumnOrder(18);

            Property(l => l.ClienteAluguel)
                .HasColumnType("char")
                .HasMaxLength(1)
                .HasColumnName("clienteAluguel")
                .HasColumnOrder(21);

            Property(l => l.ClienteEmpreitada)
                .HasColumnType("char")
                .HasMaxLength(1)
                .HasColumnName("clienteEmpreitada")
                .HasColumnOrder(24);


            Ignore(l => l.Ativo);

            HasMany<ParametrosOrdemCompra>(l => l.ListaParametrosOrdemCompra)
                .WithOptional(c => c.Cliente)
                .HasForeignKey (c => c.ClienteId);

            HasMany<CentroCustoEmpresa>(l => l.ListaCentroCustoEmpresa)
                .WithRequired(c => c.Cliente)
                .HasForeignKey(c => c.ClienteId);

            HasMany<Domain.Entity.Contrato.Contrato>(l => l.ListaContratoContratante)
                .WithRequired(c => c.Contratante)
                .HasForeignKey(c => c.ContratanteId);

            HasMany<Domain.Entity.Contrato.Contrato>(l => l.ListaContratoContratado)
                .WithRequired(c => c.Contratado)
                .HasForeignKey(c => c.ContratadoId);

            HasMany<Domain.Entity.Contrato.Contrato>(l => l.ListaContratoInterveniente)
                .WithOptional(c => c.Interveniente)
                .HasForeignKey(c => c.IntervenienteId);

            HasMany<Licitacao>(l => l.ListaLicitacao)
                .WithOptional(c => c.ClienteFornecedor)
                .HasForeignKey(c => c.ClienteFornecedorId);

            HasMany<TituloPagar>(l => l.ListaTituloPagar)
                .WithRequired(c => c.Cliente)
                .HasForeignKey(c => c.ClienteId);

            HasMany<TituloReceber>(l => l.ListaTituloReceber)
                .WithRequired(c => c.Cliente)
                .HasForeignKey(c => c.ClienteId);

            HasMany<ContratoRetificacaoItemMedicao>(l => l.ListaContratoRetificacaoItemMedicao)
                .WithOptional(c => c.MultiFornecedor)
                .HasForeignKey(c => c.MultiFornecedorId);

            HasMany<ImpostoFinanceiro>(l => l.ListaImpostoFinanceiro)
                .WithOptional(c => c.Cliente)
                .HasForeignKey(c => c.ClienteId);

            HasMany<ParametrosContrato>(l => l.ListaParametrosContrato)
                .WithOptional(c => c.Cliente)
                .HasForeignKey(c => c.ClienteId);

            HasMany<Domain.Entity.OrdemCompra.OrdemCompra>(l => l.ListaOrdemCompra)
                .WithRequired(c => c.ClienteFornecedor)
                .HasForeignKey(c => c.ClienteFornecedorId);
        }
    }
}