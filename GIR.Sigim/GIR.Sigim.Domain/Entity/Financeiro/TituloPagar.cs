using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Contrato;
using GIR.Sigim.Domain.Entity.OrdemCompra;
using GIR.Sigim.Domain.Entity.Sigim;

namespace GIR.Sigim.Domain.Entity.Financeiro
{
    public class TituloPagar : AbstractTitulo
    {
        public ClienteFornecedor Cliente { get; set; }
        public TipoCompromisso TipoCompromisso { get; set; }
        public TipoDocumento TipoDocumento { get; set; }
        public ContratoRetificacaoProvisao ContratoRetificacaoProvisao { get; set; }
        public int? TituloPaiId { get; set; }
        public virtual TituloPagar TituloPai { get; set; }
        public short? Parcela { get; set; }
        public decimal? ValorImposto { get; set; }
        public decimal? Desconto { get; set; }
        public Nullable<DateTime> DataLimiteDesconto { get; set; }
        public decimal? Multa { get; set; }
        public bool? EhMultaPercentual { get; set; }
        public decimal? TaxaPermanencia { get; set; }
        public bool? EhTaxaPermanenciaPercentual { get; set; }
        public string MotivoDesconto { get; set; }
        public Nullable<DateTime> DataEmissao { get; set; }
        public Nullable<DateTime> DataPagamento { get; set; }
        public Nullable<DateTime> DataBaixa { get; set; }
        public string LoginUsuarioCadastro { get; set; }
        public Nullable<DateTime> DataCadastro { get; set; }
        public string LoginUsuarioSituacao { get; set; }
        public Nullable<DateTime> DataSituacao { get; set; }
        public string LoginUsuarioApropriacao { get; set; }
        public Nullable<DateTime> DataApropriacao { get; set; }
        public short? FormaPagamento { get; set; }
        public int? CodigoInterface { get; set; }
        public string SistemaOrigem { get; set; }
        public string CBBanco { get; set; }
        public string CBMoeda { get; set; }
        public string CBCampoLivre { get; set; }
        public string CBDV { get; set; }
        public string CBValor { get; set; }
        public string CBDataValor { get; set; }
        public bool? CBBarra { get; set; }
        public decimal? ValorPago { get; set; }
        public int? MotivoCancelamentoId { get; set; }
        public MotivoCancelamento MotivoCancelamento { get; set; }
        public int? MovimentoId { get; set; }
        public int? BancoBaseBordero { get; set; }
        public string AgenciaContaBaseBordero { get; set; }
        public int? ContaCorrenteId { get; set; }
        public ContaCorrente ContaCorrente { get; set; }
        public decimal? Retencao { get; set; }
        public string Observacao { get; set; }
        public string MotivoCancelamentoInterface { get; set; }
        public bool? EhPagamentoAntecipado { get; set; }
        public string CBConcessionaria { get; set; }
        public int? TituloPrincipalAgrupamentoId { get; set; }
        public TituloPagar TituloPrincipalAgrupamento { get; set; }

        public virtual ICollection<TituloPagar> ListaFilhos { get; set; }
        public virtual ICollection<ImpostoPagar> ListaImpostoPagar { get; set; }
        public ICollection<ContratoRetificacaoItemMedicao> ListaContratoRetificacaoItemMedicao { get; set; }
        public ICollection<OrdemCompraFormaPagamento> ListaOrdemCompraFormaPagamento { get; set; }
        public ICollection<EntradaMaterial> ListaEntradaMaterial { get; set; }
        public ICollection<EntradaMaterialFormaPagamento> ListaEntradaMaterialFormaPagamento { get; set; }
        public ICollection<TituloPagarAdiantamento> ListaTituloPagarAdiantamento { get; set; }
        public ICollection<Apropriacao> ListaApropriacao { get; set; }

        public TituloPagar()
        {
            this.ListaFilhos = new HashSet<TituloPagar>();
            this.ListaImpostoPagar = new HashSet<ImpostoPagar>();
            this.ListaContratoRetificacaoItemMedicao = new HashSet<ContratoRetificacaoItemMedicao>();
            this.ListaOrdemCompraFormaPagamento = new HashSet<OrdemCompraFormaPagamento>();
            this.ListaEntradaMaterial = new HashSet<EntradaMaterial>();
            this.ListaEntradaMaterialFormaPagamento = new HashSet<EntradaMaterialFormaPagamento>();
            this.ListaTituloPagarAdiantamento = new HashSet<TituloPagarAdiantamento>();
            this.ListaApropriacao = new HashSet<Apropriacao>();
        }
    }
}