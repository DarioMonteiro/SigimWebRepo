using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Admin;
using GIR.Sigim.Domain.Entity.Estoque;
using GIR.Sigim.Domain.Entity.Orcamento;
using GIR.Sigim.Domain.Entity.OrdemCompra;

namespace GIR.Sigim.Domain.Entity.Sigim
{
    public class Material : BaseEntity
    {
        public string Descricao { get; set; }
        public string SiglaUnidadeMedida { get; set; }
        public virtual UnidadeMedida UnidadeMedida { get; set; }
        public string CodigoMaterialClasseInsumo { get; set; }
        public MaterialClasseInsumo MaterialClasseInsumo { get; set; }
        public decimal? PrecoUnitario { get; set; }
        //TODO: Criar relação com a classe Usuario
        public string LoginUsuarioCadastro { get; set; }
        public Nullable<DateTime> DataCadastro { get; set; }
        //TODO: Criar relação com a classe Usuario
        public string LoginUsuarioAlteracao { get; set; }
        public Nullable<DateTime> DataAlteracao { get; set; }
        public Nullable<DateTime> DataPreco { get; set; }
        public decimal? QuantidadeMinima { get; set; }
        public bool? EhControladoPorEstoque { get; set; }
        public string ContaContabil { get; set; }
        public TipoTabela TipoTabela { get; set; }
        public int? AnoMes { get; set; }
        public string CodigoExterno { get; set; }
        public string CodigoSituacaoMercadoria { get; set; }
        public SituacaoMercadoria SituacaoMercadoria { get; set; }
        public string CodigoNCM { get; set; }
        public NCM NCM { get; set; }
        public string TipoMaterial { get; set; }
        [Obsolete("Esta propriedade será removida em uma versão futura. Caso NÃO esteja codificando em um repositório, utilize a propriedade \"Ativo\"")]
        public string Situacao { get; set; }
        public bool Ativo
        {
            get { return Situacao == "A"; }
            set { Situacao = value ? "A" : "I"; }
        }
        public ICollection<PreRequisicaoMaterialItem> ListaPreRequisicaoMaterialItem { get; set; }
        public ICollection<RequisicaoMaterialItem> ListaRequisicaoMaterialItem { get; set; }
        public ICollection<OrdemCompraItem> ListaOrdemCompraItem { get; set; }
        public ICollection<OrcamentoInsumoRequisitado> ListaOrcamentoInsumoRequisitado { get; set; }
        public ICollection<OrcamentoComposicaoItem> ListaOrcamentoComposicaoItem { get; set; }
        public ICollection<EstoqueMaterial> ListaEstoqueMaterial { get; set; }
        public ICollection<MovimentoItem> ListaMovimentoItem { get; set; }

        public Material()
        {
            this.ListaPreRequisicaoMaterialItem = new HashSet<PreRequisicaoMaterialItem>();
            this.ListaRequisicaoMaterialItem = new HashSet<RequisicaoMaterialItem>();
            this.ListaOrdemCompraItem = new HashSet<OrdemCompraItem>();
            this.ListaOrcamentoInsumoRequisitado = new HashSet<OrcamentoInsumoRequisitado>();
            this.ListaOrcamentoComposicaoItem = new HashSet<OrcamentoComposicaoItem>();
            this.ListaEstoqueMaterial = new HashSet<EstoqueMaterial>();
            this.ListaMovimentoItem = new HashSet<MovimentoItem>();
        }
    }
}