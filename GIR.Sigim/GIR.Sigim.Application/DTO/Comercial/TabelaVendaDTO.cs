using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Comercial;

namespace GIR.Sigim.Application.DTO.Comercial
{
    public class TabelaVendaDTO : BaseDTO
    {
        public int BlocoId { get; set; }
        public BlocoDTO Bloco { get; set; }
        public String Nome { get; set; }
        public String Situacao { get; set; }
        public Nullable<DateTime> DataElaboracao { get; set; }
        public String Observacao { get; set; }
        public Decimal PrecoReferencia { get; set; }
        public Decimal PercentualCorretora { get; set; }
        public Decimal PercentualCorretor { get; set; }

    }
}
