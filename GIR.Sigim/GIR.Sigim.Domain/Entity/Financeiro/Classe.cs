using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Orcamento;
using GIR.Sigim.Domain.Entity.OrdemCompra;
using GIR.Sigim.Domain.Entity.Sigim;
using GIR.Sigim.Domain.Entity.Contrato;
using GIR.Sigim.Domain.Entity.Estoque;

namespace GIR.Sigim.Domain.Entity.Financeiro
{
    public class Classe : BaseEntity
    {
        public string Codigo { get; set; }
        public string Descricao { get; set; }
        public string ContaContabil { get; set; }
        public string CodigoPai { get; set; }
        public virtual Classe ClassePai { get; set; }
        public virtual ICollection<Classe> ListaFilhos { get; set; }
        public ICollection<PreRequisicaoMaterialItem> ListaPreRequisicaoMaterialItem { get; set; }
        public ICollection<RequisicaoMaterialItem> ListaRequisicaoMaterialItem { get; set; }
        public virtual ICollection<OrcamentoComposicao> ListaOrcamentoComposicao { get; set; }
        public ICollection<ContratoRetificacaoItem> ListaContratoRetificacaoItem { get; set; }
        public ICollection<ContratoRetificacaoItemMedicaoNF> ListaContratoRetificacaoItemMedicaoNF { get; set; }
        public ICollection<OrdemCompraItem> ListaOrdemCompraItem { get; set; }
        public ICollection<OrcamentoInsumoRequisitado> ListaOrcamentoInsumoRequisitado { get; set; }
        public ICollection<RateioAutomatico> ListaRateioAutomatico { get; set; }
        public ICollection<TaxaAdministracao> ListaTaxaAdministracao { get; set; }
        public ICollection<EntradaMaterialItem> ListaEntradaMaterialItem { get; set; }
        public ICollection<MovimentoItem> ListaMovimentoItem { get; set; }
        public ICollection<Apropriacao> ListaApropriacao { get; set; }
        public ICollection<ApropriacaoAdiantamento> ListaApropriacaoAdiantamento { get; set; }
        
        public Classe()
        {
            this.ListaFilhos = new HashSet<Classe>();
            this.ListaPreRequisicaoMaterialItem = new HashSet<PreRequisicaoMaterialItem>();
            this.ListaRequisicaoMaterialItem = new HashSet<RequisicaoMaterialItem>();
            this.ListaOrcamentoComposicao = new HashSet<OrcamentoComposicao>();
            this.ListaOrdemCompraItem = new HashSet<OrdemCompraItem>();
            this.ListaOrcamentoInsumoRequisitado = new HashSet<OrcamentoInsumoRequisitado>();
            this.ListaContratoRetificacaoItem = new HashSet<ContratoRetificacaoItem>();
            this.ListaContratoRetificacaoItemMedicaoNF = new HashSet<ContratoRetificacaoItemMedicaoNF>();
            this.ListaRateioAutomatico = new HashSet<RateioAutomatico>();
            this.ListaTaxaAdministracao = new HashSet<TaxaAdministracao>();
            this.ListaEntradaMaterialItem = new HashSet<EntradaMaterialItem>();
            this.ListaMovimentoItem = new HashSet<MovimentoItem>();
            this.ListaApropriacao = new HashSet<Apropriacao>();
            this.ListaApropriacaoAdiantamento = new HashSet<ApropriacaoAdiantamento>();
        }

        
    }
}