using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Comercial;

namespace GIR.Sigim.Domain.Entity.CredCob
{
    public class TituloCredCob : BaseEntity
    {
        public int ContratoId { get; set; }
        public ContratoComercial Contrato { get; set; }
        public string Situacao { get; set; }
        public DateTime DataVencimento { get; set; }
        public Nullable<DateTime> DataPagamento { get; set; }
        public Decimal QtdIndice { get; set; }
        public int VerbaCobrancaId { get; set; }
        public VerbaCobranca VerbaCobranca { get; set; }
        public Nullable<Decimal> ValorBaixa { get; set; }
        public Decimal ValorIndiceBase { get; set; }

    }
}
