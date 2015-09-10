using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Financeiro;
using GIR.Sigim.Domain.Entity.Sigim;

namespace GIR.Sigim.Domain.Entity.OrdemCompra
{
    public class EntradaMaterialItem : BaseEntity
    {
        public int? EntradaMaterialId { get; set; }
        public virtual EntradaMaterial EntradaMaterial { get; set; }
        public int? OrdemCompraItemId { get; set; }
        public OrdemCompraItem OrdemCompraItem { get; set; }
        public string CodigoClasse { get; set; }
        public Classe Classe { get; set; }
        public int Sequencial { get; set; }
        public decimal? Quantidade { get; set; }
        public decimal? ValorUnitario { get; set; }
        public decimal? PercentualIPI { get; set; }
        public decimal? PercentualDesconto { get; set; }
        public decimal? ValorTotal { get; set; }
        public decimal? BaseICMS { get; set; }
        public decimal? PercentualICMS { get; set; }
        public decimal? BaseIPI { get; set; }
        public decimal? BaseICMSST { get; set; }
        public decimal? PercentualICMSST { get; set; }
        public string CodigoComplementoNaturezaOperacao { get; set; }
        public ComplementoNaturezaOperacao ComplementoNaturezaOperacao { get; set; }
        public string CodigoComplementoCST { get; set; }
        public ComplementoCST ComplementoCST { get; set; }
        public string CodigoNaturezaReceita { get; set; }
        public NaturezaReceita NaturezaReceita { get; set; }
    }
}