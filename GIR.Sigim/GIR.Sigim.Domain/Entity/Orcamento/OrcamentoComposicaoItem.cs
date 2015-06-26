using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Sigim;

namespace GIR.Sigim.Domain.Entity.Orcamento
{
    public class OrcamentoComposicaoItem : BaseEntity
    {
        public int? OrcamentoComposicaoId { get; set; }
        public OrcamentoComposicao OrcamentoComposicao { get; set; }
        public int? MaterialId { get; set; }
        public Material Material { get; set; }
        public decimal? Consumo { get; set; }
        public decimal? PercentualPerda { get; set; }
        public decimal? Preco { get; set; }
        public bool? EhControlado { get; set; }
        public decimal? QuantidadeOrcada
        {
            get
            {
                return (Consumo + (Consumo * PercentualPerda / 100)) * OrcamentoComposicao.Quantidade;
            }
        }

        public decimal? QuantidadeRequisitada
        {
            get
            {
                return Material.ListaOrcamentoInsumoRequisitado.Where(l => l.CodigoCentroCusto == OrcamentoComposicao.Orcamento.Obra.CodigoCentroCusto
                    && l.CodigoClasse == OrcamentoComposicao.codigoClasse
                    && l.ComposicaoId == OrcamentoComposicao.ComposicaoId).Sum(l => l.Quantidade);
            }
        }
    }
}