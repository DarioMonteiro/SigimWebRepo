using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Financeiro;

namespace GIR.Sigim.Application.DTO.OrdemCompra
{
    public class OrdemCompraFormaPagamentoDTO : BaseDTO
    {
        //public OrdemCompraDTO OrdemCompra { get; set; }
        public int? OrdemCompraId { get; set; }
        public Nullable<DateTime> Data { get; set; }
        public decimal Valor { get; set; }
        public TipoCompromissoDTO TipoCompromisso { get; set; }
        public TituloPagarDTO TituloPagar { get; set; }
        public bool EhPagamentoAntecipado { get; set; }
        public string EhPagamentoAntecipadoDescricao { get; set; }
        public bool EhUtilizada { get; set; }
        public bool EhAssociada { get; set; }

        public OrdemCompraFormaPagamentoDTO()
        {
            //this.OrdemCompra = new OrdemCompraDTO();
            this.TipoCompromisso = new TipoCompromissoDTO();
            this.TituloPagar = new TituloPagarDTO();
        }
    }
}