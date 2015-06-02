using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Financeiro;
using GIR.Sigim.Domain.Entity.OrdemCompra;
using GIR.Sigim.Domain.Entity.Sigim;

namespace GIR.Sigim.Domain.Entity.Orcamento
{
    public class OrcamentoInsumoRequisitado : BaseEntity
    {
        public string CodigoCentroCusto { get; set; }
        public CentroCusto CentroCusto { get; set; }
        public string CodigoClasse { get; set; }
        public Classe Classe { get; set; }
        public int? ComposicaoId { get; set; }
        public Composicao Composicao { get; set; }
        public int? MaterialId { get; set; }
        public Material Material { get; set; }
        public int? RequisicaoMaterialItemId { get; set; }
        public RequisicaoMaterialItem RequisicaoMaterialItem { get; set; }
        public decimal? Quantidade { get; set; }
    }
}