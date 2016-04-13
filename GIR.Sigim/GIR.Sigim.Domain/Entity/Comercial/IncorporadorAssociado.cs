using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Domain.Entity.Comercial
{
    public class IncorporadorAssociado : BaseEntity
    {
        public int IncorporadorId { get; set; }
        public Incorporador Incorporador { get; set; }
        public int BlocoId { get; set; }
        public Bloco Bloco { get; set; }
        public int UnidadeId { get; set; }
        public Unidade Unidade { get; set; }
        public DateTime DataVigencia { get; set; }
        public Nullable<Decimal> PercentualCusto { get; set; }
        public Nullable<Decimal> PercentualReceita { get; set; }
        public string EfetuaRateioBaixaTitulo { get; set; }
    }
}
