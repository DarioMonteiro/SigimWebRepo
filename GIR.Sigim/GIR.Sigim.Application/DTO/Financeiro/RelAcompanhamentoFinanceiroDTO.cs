using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Application.DTO.Financeiro
{
    public class RelAcompanhamentoFinanceiroDTO
    {
        public string CodigoClasse { get; set; }
        public string DescricaoClasse { get; set; }
        public decimal OrcamentoInicial { get; set; }
        public decimal OrcamentoAtual { get; set; }
        public decimal DespesaPeriodo { get; set; }
        public decimal DespesaAcumulada { get; set; }
        public decimal ComprometidoPendente { get; set; }
        public decimal ComprometidoFuturo { get; set; }
        public decimal ResultadoAcrescimo { get; set; }
        public decimal ResultadoSaldo { get; set; }
        public string DescricaoClasseFechada { get; set; }
        public decimal Conclusao { get; set; }
    }
}
