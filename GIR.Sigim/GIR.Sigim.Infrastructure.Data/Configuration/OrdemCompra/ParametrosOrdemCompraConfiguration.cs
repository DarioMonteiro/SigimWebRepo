using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.OrdemCompra;

namespace GIR.Sigim.Infrastructure.Data.Configuration.OrdemCompra
{
    public class ParametrosOrdemCompraConfiguration : EntityTypeConfiguration<ParametrosOrdemCompra>
    {
        public ParametrosOrdemCompraConfiguration()
        {
            ToTable("Parametros", "OrdemCompra");

            Property(l => l.Id)
                .HasColumnName("codigo")
                .HasColumnOrder(1);

            Property(l => l.ClienteId)
                .HasColumnName("cliente")
                .HasColumnOrder(2);

            HasOptional(l => l.Cliente)
                .WithMany(l => l.ListaParametrosOrdemCompra);

            Property(l => l.Responsavel)
                .HasMaxLength(30)
                .HasColumnName("responsavel")
                .HasColumnOrder(3);

            Property(l => l.MascaraClasseInsumo)
                .HasMaxLength(18)
                .HasColumnName("mascaraClasseInsumo")
                .HasColumnOrder(4);

            Property(l => l.IconeRelatorio)
                .HasColumnType("image")
                .HasColumnName("iconeRelatorio")
                .HasColumnOrder(5);

            Property(l => l.AssuntoContatoId)
                .HasColumnName("assuntoContatoEmail")
                .HasColumnOrder(6);

            HasOptional(l => l.AssuntoContato)
                .WithMany(l => l.ListaParametrosOrdemCompra);

            Property(l => l.GeraTituloAguardando)
                .HasColumnName("geraTituloAguardando")
                .HasColumnOrder(7);

            Property(l => l.GeraProvisionamentoNaCotacao)
                .HasColumnName("geraProvisionamentoNaCotacao")
                .HasColumnOrder(8);

            Property(l => l.DiasDataMinima)
                .HasColumnName("diasDataMinima")
                .HasColumnOrder(9);

            Property(l => l.DiasPrazo)
                .HasColumnName("diasPrazo")
                .HasColumnOrder(10);

            Property(l => l.EhPreRequisicaoMaterial)
                .HasColumnName("preRequisicaoMaterial")
                .HasColumnOrder(11);

            Property(l => l.TipoCompromissoFreteId)
                .HasColumnName("tipoCompromissoFrete")
                .HasColumnOrder(12);

            HasOptional(l => l.TipoCompromissoFrete)
                .WithMany(l => l.ListaParametrosOrdemCompra);

            Property(l => l.SmtpServidorSaidaEmail)
                .HasMaxLength(50)
                .HasColumnName("smtpServidorSaidaEmail")
                .HasColumnOrder(13);

            Property(l => l.SmtpPortaSaidaEmail)
                .HasColumnName("smtpPortaSaidaEmail")
                .HasColumnOrder(14);

            Property(l => l.EhRequisicaoObrigatoria)
                .HasColumnName("requisicaoObrigatoria")
                .HasColumnOrder(15);

            Property(l => l.EhInterfaceOrcamento)
                .HasColumnName("interfaceOrcamento")
                .HasColumnOrder(16);

            Property(l => l.HabilitaSSL)
                .HasColumnName("habilitaSSL")
                .HasColumnOrder(17);

            Property(l => l.InibeFormaPagamento)
                .HasColumnName("inibeFormaPagamento")
                .HasColumnOrder(18);

            Property(l => l.EhInterfaceContabil)
                .HasColumnName("interfaceContabil")
                .HasColumnOrder(19);

            Property(l => l.InterfaceCotacao.Modelo)
                .HasColumnName("modeloInterfaceCotacao")
                .HasColumnOrder(20);

            Property(l => l.InterfaceCotacao.CodigoCliente)
                .HasColumnName("clienteInterfaceCotacao")
                .HasColumnOrder(21);

            Property(l => l.InterfaceCotacao.CodigoLogin)
                .HasColumnName("loginInterfaceCotacao")
                .HasColumnOrder(22);
            
            Property(l => l.InterfaceCotacao.CodigoRegiao)
                .HasColumnName("regiaoInterfaceCotacao")
                .HasColumnOrder(23);
            
            Property(l => l.InterfaceCotacao.CodigoComprador)
                .HasColumnName("compradorInterfaceCotacao")
                .HasColumnOrder(24);
            
            Property(l => l.InterfaceCotacao.CodigoTelefone)
                .HasColumnName("telefoneInterfaceCotacao")
                .HasColumnOrder(25);
            
            Property(l => l.InterfaceCotacao.CodigoCentroCusto)
                .HasColumnName("centroCustoInterfaceCotacao")
                .HasColumnOrder(26);
            
            Property(l => l.InterfaceCotacao.CodigoTipo)
                .HasColumnName("tipoInterfaceCotacao")
                .HasColumnOrder(27);

            Property(l => l.DiasEntradaMaterial)
                .HasColumnName("diasEntradaMaterial")
                .HasColumnOrder(28);

            Property(l => l.ConfereNF)
                .HasColumnName("confereNF")
                .HasColumnOrder(29);

            Property(l => l.GravaCotacaoWeb)
                .HasColumnName("gravaCotacaoWeb")
                .HasColumnOrder(30);

            Property(l => l.LayoutSPEDId)
                .HasColumnName("layoutSPED")
                .HasColumnOrder(31);

            HasOptional(l => l.LayoutSPED)
                .WithMany(l => l.ListaParametrosOrdemCompra);

            Property(l => l.DiasPagamento)
                .HasColumnName("diasPagamento")
                .HasColumnOrder(32);
            
            Property(l => l.EhObrigatorioDadosSPED)
                .HasColumnName("dadosSPED")
                .HasColumnOrder(33);

            Property(l => l.EhGeraRequisicaoMaterialRequisitada)
                .HasColumnName("geraRequisicaoMaterialRequisitada")
                .HasColumnType("bit")
                .HasColumnOrder(34);

        }
    }
}