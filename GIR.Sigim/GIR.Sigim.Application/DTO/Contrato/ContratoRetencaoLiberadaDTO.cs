using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Financeiro;

namespace GIR.Sigim.Application.DTO.Contrato
{
    public class ContratoRetencaoLiberadaDTO : BaseDTO
    {
        public int ContratoRetencaoId { get; set; }
        public ContratoRetencaoDTO ContratoRetencao { get; set; }
        public int TipoDocumentoId { get; set; }
        public TipoDocumentoDTO TipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public DateTime DataVencimento { get; set; }
        public decimal ValorLiberado { get; set; }
        public DateTime DataLiberacao { get; set; }
        public string UsuarioLiberacao { get; set; }
        public int? TipoCompromissoId { get; set; }
        public TipoCompromissoDTO TipoCompromisso { get; set; }
        public int? TituloPagarId { get; set; }
        public TituloPagarDTO TituloPagar { get; set; }
        public int? TituloReceberId { get; set; }
        public TituloReceberDTO TituloReceber { get; set; }

    }
}
