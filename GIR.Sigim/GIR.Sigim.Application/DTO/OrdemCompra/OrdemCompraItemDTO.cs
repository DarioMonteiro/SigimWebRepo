using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.Adapter;
using GIR.Sigim.Domain.Entity.OrdemCompra;

namespace GIR.Sigim.Application.DTO.OrdemCompra
{
    public class OrdemCompraItemDTO : BaseDTO
    {
        public int? OrdemCompraId { get; set; }
        public int? RequisicaoMaterialItemId { get; set; }
        public int? CotacaoItemId { get; set; }
        public int? MaterialId { get; set; }
        public string CodigoClasse { get; set; }
        public int Sequencial { get; set; }
        public string Complemento { get; set; }
        public decimal? Quantidade { get; set; }
        public decimal? QuantidadeEntregue { get; set; }
        public decimal? ValorUnitario { get; set; }
        public decimal? PercentualIPI { get; set; }
        public decimal? PercentualDesconto { get; set; }
        public decimal? ValorTotalComImposto { get; set; }
        public decimal? ValorTotalItem { get; set; }
    }
}