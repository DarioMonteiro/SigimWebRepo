using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Financeiro;
using GIR.Sigim.Domain.Entity.Sigim;

namespace GIR.Sigim.Domain.Entity.OrdemCompra
{
    public class ParametrosOrdemCompra : BaseEntity, IParametros
    {
        public int? ClienteId { get; set; }
        public ClienteFornecedor Cliente { get; set; }
        public string Responsavel { get; set; }
        public string MascaraClasseInsumo { get; set; }
        public byte[] IconeRelatorio { get; set; }
        public int? AssuntoContatoId { get; set; }
        public AssuntoContato AssuntoContato { get; set; }
        public bool? GeraTituloAguardando { get; set; }
        public bool? GeraProvisionamentoNaCotacao { get; set; }
        public short? DiasDataMinima { get; set; }
        public short? DiasPrazo { get; set; }
        public bool? EhPreRequisicaoMaterial { get; set; }
        public int? TipoCompromissoFreteId { get; set; }
        public TipoCompromisso TipoCompromissoFrete { get; set; }
        public string SmtpServidorSaidaEmail { get; set; }
        public int? SmtpPortaSaidaEmail { get; set; }
        public bool? EhRequisicaoObrigatoria { get; set; }
        public bool? EhInterfaceOrcamento { get; set; }
        public bool? HabilitaSSL { get; set; }
        public bool? InibeFormaPagamento { get; set; }
        public bool? EhInterfaceContabil { get; set; }
        public InterfaceCotacao InterfaceCotacao { get; set; }
        
        private int? diasEntradaMaterial;
        public int? DiasEntradaMaterial
        {
            get { return diasEntradaMaterial.HasValue ? diasEntradaMaterial : 0; }
            set { diasEntradaMaterial = value; }
        }

        public bool? ConfereNF { get; set; }
        public bool? GravaCotacaoWeb { get; set; }
        public int? LayoutSPEDId { get; set; }
        public BancoLayout LayoutSPED { get; set; }

        private int? diasPagamento;
        public int? DiasPagamento
        {
            get { return diasPagamento.HasValue ? diasPagamento : 0; }
            set { diasPagamento = value; }
        }

        public bool? EhObrigatorioDadosSPED { get; set; }
        public bool? EhGeraRequisicaoMaterialRequisitada { get; set; }

        public ParametrosOrdemCompra()
        {
            this.InterfaceCotacao = new InterfaceCotacao();
        }
    }
}