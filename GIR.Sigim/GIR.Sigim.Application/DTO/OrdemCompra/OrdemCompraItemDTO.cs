using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.Adapter;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Application.DTO.Financeiro;

namespace GIR.Sigim.Application.DTO.OrdemCompra
{
    public class OrdemCompraItemDTO : BaseDTO
    {
        public int? OrdemCompraId { get; set; }
        public DateTime DataOrdemCompra { get; set; }
        public int? RequisicaoMaterialItemId { get; set; }
        public int? CotacaoItemId { get; set; }
        public int? MaterialId { get; set; }
        public MaterialDTO Material { get; set; }
        public string CodigoClasse { get; set; }
        public ClasseDTO Classe { get; set; }
        public int Sequencial { get; set; }
        public string Complemento { get; set; }
        public decimal? Quantidade { get; set; }
        public decimal? QuantidadeEntregue { get; set; }
        public decimal? ValorUnitario { get; set; }
        public decimal? PercentualIPI { get; set; }
        public decimal? PercentualDesconto { get; set; }
        public decimal? ValorTotalComImposto { get; set; }
        public decimal? ValorTotalItem { get; set; }
        public decimal? Saldo { get; set; }
        public decimal? PercentualDescontoOrdemCompra { get; set; }
    }
}