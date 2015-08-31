using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Financeiro;

namespace GIR.Sigim.Domain.Entity.Contrato
{
    public class ContratoRetencaoLiberada : BaseEntity
    {
        public int ContratoRetencaoId { get; set; }
        public ContratoRetencao ContratoRetencao { get; set; }
        public int TipoDocumentoId { get; set; }
        public TipoDocumento TipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public DateTime DataVencimento { get; set; }
        public decimal ValorLiberado { get; set; }
        public DateTime DataLiberacao { get; set; }
        public string UsuarioLiberacao { get; set; }
        public int? TipoCompromissoId { get; set; }
        public TipoCompromisso TipoCompromisso { get; set; }
        public int? TituloPagarId { get; set; }
        public TituloPagar TituloPagar { get; set; }
        public int? TituloReceberId { get; set; }
        public TituloReceber TituloReceber { get; set; }
    }
}
