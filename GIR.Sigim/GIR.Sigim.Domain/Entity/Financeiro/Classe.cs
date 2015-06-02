using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Orcamento;
using GIR.Sigim.Domain.Entity.OrdemCompra;
using GIR.Sigim.Domain.Entity.Sigim;
using GIR.Sigim.Domain.Entity.Contrato;

namespace GIR.Sigim.Domain.Entity.Financeiro
{
    public class Classe : BaseEntity
    {
        public string Codigo { get; set; }
        public string Descricao { get; set; }
        public string ContaContabil { get; set; }
        public string CodigoPai { get; set; }
        public Classe ClassePai { get; set; }
        public virtual ICollection<Classe> ListaFilhos { get; set; }
        public ICollection<PreRequisicaoMaterialItem> ListaPreRequisicaoMaterialItem { get; set; }
        public ICollection<RequisicaoMaterialItem> ListaRequisicaoMaterialItem { get; set; }
        public virtual ICollection<OrcamentoComposicao> ListaOrcamentoComposicao { get; set; }
        public ICollection<ContratoRetificacaoItem> ListaContratoRetificacaoItem { get; set; } 
        public ICollection<OrdemCompraItem> ListaOrdemCompraItem { get; set; }
        public ICollection<OrcamentoInsumoRequisitado> ListaOrcamentoInsumoRequisitado { get; set; }

        public Classe()
        {
            this.ListaFilhos = new HashSet<Classe>();
            this.ListaPreRequisicaoMaterialItem = new HashSet<PreRequisicaoMaterialItem>();
            this.ListaRequisicaoMaterialItem = new HashSet<RequisicaoMaterialItem>();
            this.ListaOrcamentoComposicao = new HashSet<OrcamentoComposicao>();
            this.ListaOrdemCompraItem = new HashSet<OrdemCompraItem>();
            this.ListaOrcamentoInsumoRequisitado = new HashSet<OrcamentoInsumoRequisitado>();
            this.ListaContratoRetificacaoItem = new HashSet<ContratoRetificacaoItem>();  
        }
    }
}