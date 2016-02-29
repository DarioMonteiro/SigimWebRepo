using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Financeiro;
using GIR.Sigim.Domain.Entity.Contrato;
using GIR.Sigim.Domain.Entity.OrdemCompra;

namespace GIR.Sigim.Domain.Entity.Sigim
{
    public class ClienteFornecedor : BaseEntity
    {
        public string Nome { get; set; }
        public string TipoPessoa { get; set; }
        [Obsolete("Esta propriedade será removida em uma versão futura. Caso NÃO esteja codificando em um repositório, utilize a propriedade \"Ativo\"")]
        public string Situacao { get; set; }
        public bool Ativo
        {
            get { return Situacao == "A"; }
            set { Situacao = value ? "A" : "I"; }
        }      
        public string TipoCliente { get; set; }
        public string ClienteAPagar { get; set; }
        public string ClienteAReceber { get; set; }
        public string ClienteOrdemCompra { get; set; }
        public string ClienteContrato { get; set; }
        public string ClienteAluguel { get; set; }
        public string ClienteEmpreitada { get; set; }
        public PessoaJuridica PessoaJuridica { get; set; }
        public PessoaFisica PessoaFisica { get; set; }
        public int? EnderecoComercialId { get; set; }

        public ICollection<CentroCustoEmpresa> ListaCentroCustoEmpresa { get; set; }
        public ICollection<OrdemCompra.ParametrosOrdemCompra> ListaParametrosOrdemCompra { get; set; }
        public ICollection<Contrato.Contrato> ListaContratoContratante { get; set; }
        public ICollection<Contrato.Contrato> ListaContratoContratado { get; set; }
        public ICollection<Contrato.Contrato> ListaContratoInterveniente { get; set; }
        public ICollection<Contrato.Licitacao> ListaLicitacao { get; set; }
        public ICollection<TituloPagar> ListaTituloPagar { get; set; }
        public ICollection<TituloReceber> ListaTituloReceber { get; set; }
        public ICollection<ContratoRetificacaoItemMedicao> ListaContratoRetificacaoItemMedicao { get; set; }
        public ICollection<ImpostoFinanceiro> ListaImpostoFinanceiro { get; set; }
        public ICollection<ParametrosContrato> ListaParametrosContrato { get; set; }
        public ICollection<TaxaAdministracao> ListaTaxaAdministracao { get; set; }
        public ICollection<EntradaMaterial> ListaEntradaMaterial { get; set; }
        public ICollection<EntradaMaterial> ListaEntradaMaterialNota { get; set; }
        public ICollection<EntradaMaterial> ListaEntradaMaterialTransportadora { get; set; }
        public ICollection<AvaliacaoFornecedor> ListaAvaliacaoFornecedor { get; set; }
        public ICollection<Domain.Entity.OrdemCompra.OrdemCompra> ListaOrdemCompra { get; set; }
        public ICollection<EmpresaCentroCusto> ListaEmpresaOrdemCompra { get; set; }

        public ClienteFornecedor()
        {
            this.ListaCentroCustoEmpresa = new HashSet<CentroCustoEmpresa>();
            this.ListaParametrosOrdemCompra = new HashSet<OrdemCompra.ParametrosOrdemCompra>();
            this.ListaContratoContratante = new HashSet<Contrato.Contrato>();
            this.ListaContratoContratado = new HashSet<Contrato.Contrato>();
            this.ListaContratoInterveniente = new HashSet<Contrato.Contrato>();
            this.ListaLicitacao = new HashSet<Contrato.Licitacao>();
            this.ListaTituloPagar = new HashSet<TituloPagar>();
            this.ListaTituloReceber = new HashSet<TituloReceber>();
            this.ListaContratoRetificacaoItemMedicao = new HashSet<ContratoRetificacaoItemMedicao>();
            this.ListaImpostoFinanceiro = new HashSet<ImpostoFinanceiro>();
            this.ListaParametrosContrato = new HashSet<ParametrosContrato>();
            this.ListaTaxaAdministracao = new HashSet<TaxaAdministracao>();
            this.ListaEntradaMaterial = new HashSet<EntradaMaterial>();
            this.ListaEntradaMaterialNota = new HashSet<EntradaMaterial>();
            this.ListaEntradaMaterialTransportadora = new HashSet<EntradaMaterial>();
            this.ListaAvaliacaoFornecedor = new HashSet<AvaliacaoFornecedor>();
            this.ListaOrdemCompra = new HashSet<Domain.Entity.OrdemCompra.OrdemCompra>();
            this.ListaEmpresaOrdemCompra = new HashSet<EmpresaCentroCusto>();
        }
    }
}