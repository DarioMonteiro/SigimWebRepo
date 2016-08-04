using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Sigim;

namespace GIR.Sigim.Domain.Entity.Financeiro
{
    public abstract class AbstractTitulo : BaseEntity
    {
        public int ClienteId { get; set; }
        public int? TipoCompromissoId { get; set; }
        public string Identificacao { get; set; }
        public int? TipoDocumentoId { get; set; }
        public string Documento { get; set; }
        public DateTime DataVencimento { get; set; }
        public TipoTitulo TipoTitulo { get; set; }
        public decimal ValorTitulo { get; set; }
    }
}
