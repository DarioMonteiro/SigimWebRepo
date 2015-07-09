using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Financeiro;
using GIR.Sigim.Domain.Entity.OrdemCompra;

namespace GIR.Sigim.Domain.Entity.Sigim
{
    public class AvaliacaoFornecedor : BaseEntity
    {
        public int? ClienteFornecedorId { get; set; }
        public ClienteFornecedor ClienteFornecedor { get; set; }
        public int? AvaliacaoModeloId { get; set; }
        public AvaliacaoModelo AvaliacaoModelo { get; set; }
        public DateTime Data { get; set; }
        public string LoginUsuarioCadastro { get; set; }
        public int MediaMinima { get; set; }
        public decimal MediaObtida { get; set; }
        public string Observacao { get; set; }
        public int? TipoDocumentoId { get; set; }
        public TipoDocumento TipoDocumento { get; set; }
        public string Documento { get; set; }
        public int? EntradaMaterialId { get; set; }
        public EntradaMaterial EntradaMaterial { get; set; }
    }
}