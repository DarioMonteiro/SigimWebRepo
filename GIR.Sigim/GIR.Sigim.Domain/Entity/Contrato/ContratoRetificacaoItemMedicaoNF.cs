using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Financeiro;
using GIR.Sigim.Domain.Entity.Sigim;

namespace GIR.Sigim.Domain.Entity.Contrato
{
    public class ContratoRetificacaoItemMedicaoNF : BaseEntity
    {
        public int? ContratoId { get; set; }
        public Contrato Contrato { get; set; }
        public int TipoDocumentoId { get; set; }
        public TipoDocumento TipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public DateTime DataEntrega { get; set; }
        public int MaterialId { get; set; }
        public Material Material { get; set; }
        public string CodigoClasse { get; set; }
        public Classe Classe { get; set; }
        public int Sequencial { get; set; }
        public string ComplementoDescricao { get; set; }
        public decimal? Quantidade { get; set; }
        public decimal? ValorUnitario { get; set; }
        public decimal? BaseIPI { get; set; }
        public decimal? PercentualIpi { get; set; }
        public decimal? BaseIcms { get; set; }
        public decimal? PercentualIcms { get; set; }
        public decimal? BaseIcmsSt { get; set; }
        public decimal? PercentualIcmsSt { get; set; }
        public decimal? PercentualDesconto { get; set; }
        public decimal? ValorTotalSemImposto { get; set; }
        public decimal? ValorTotalItem { get; set; }
        public string ComplementoNaturezaOperacao { get; set; }
        public string ComplementoCST { get; set; }
        public string NaturezaReceita { get; set; }
        public Nullable<DateTime> DataEmissao { get; set; }
    }
}
