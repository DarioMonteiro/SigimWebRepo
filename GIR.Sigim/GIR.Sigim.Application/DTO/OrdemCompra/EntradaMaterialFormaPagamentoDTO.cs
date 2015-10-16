using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.Adapter;
using GIR.Sigim.Application.DTO.Financeiro;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Domain.Entity.OrdemCompra;

namespace GIR.Sigim.Application.DTO.OrdemCompra
{
    public class EntradaMaterialFormaPagamentoDTO : BaseDTO
    {
        public int? OrdemCompraFormaPagamentoId { get; set; }
        public OrdemCompraFormaPagamentoDTO OrdemCompraFormaPagamento { get; set; }
        public Nullable<DateTime> Data { get; set; }
        public decimal Valor { get; set; }
        public TipoCompromissoDTO TipoCompromisso { get; set; }
        public TituloPagarDTO TituloPagar { get; set; }
        public string EhLiberadoDescricao { get; set; }

        public EntradaMaterialFormaPagamentoDTO()
        {
            this.OrdemCompraFormaPagamento = new OrdemCompraFormaPagamentoDTO();
            this.TipoCompromisso = new TipoCompromissoDTO();
            this.TituloPagar = new TituloPagarDTO();
        }
    }
}