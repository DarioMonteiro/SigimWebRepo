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
    public class CotacaoItemDTO : BaseDTO
    {
        public int? CotacaoId { get; set; }
        public int? RequisicaoMaterialItemId { get; set; }
        public int Sequencial { get; set; }
        public decimal Quantidade { get; set; }
        public int? OrdemCompraItemUltimoPreco { get; set; }
        public decimal? ValorUltimoPreco { get; set; }
        public Nullable<DateTime> DataUltimoPreco { get; set; }
        public decimal? ValorOrcado { get; set; }
        public string LoginUsuarioEleicao { get; set; }
        public Nullable<DateTime> DataEleicao { get; set; }
    }
}