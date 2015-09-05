using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Application.DTO.Financeiro;
using GIR.Sigim.Application.DTO.OrdemCompra;
using GIR.Sigim.Application.DTO.Contrato;

namespace GIR.Sigim.Application.DTO.Sigim
{
    public class AvaliacaoFornecedorDTO :BaseDTO
    {
        public int? ClienteFornecedorId { get; set; }
        public ClienteFornecedorDTO ClienteFornecedor { get; set; }
        public int? AvaliacaoModeloId { get; set; }
        public DateTime Data { get; set; }
        public string LoginUsuarioCadastro { get; set; }
        public int MediaMinima { get; set; }
        public decimal MediaObtida { get; set; }
        public string Observacao { get; set; }
        public int? TipoDocumentoId { get; set; }
        public TipoDocumentoDTO TipoDocumento { get; set; }
        public string Documento { get; set; }
        public int? EntradaMaterialId { get; set; }
        public EntradaMaterialDTO EntradaMaterial { get; set; }
        public int? ContratoId { get; set; }
        public ContratoDTO Contrato { get; set; }

    }
}
