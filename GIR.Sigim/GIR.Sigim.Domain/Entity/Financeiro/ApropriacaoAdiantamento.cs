using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Domain.Entity.Financeiro
{
    public class ApropriacaoAdiantamento : BaseEntity
    {
        public string CodigoClasse { get; set; }
        public Classe Classe { get; set; }
        public string CodigoCentroCusto { get; set; }
        public CentroCusto CentroCusto { get; set; }
        public int? TituloPagarAdiantamentoId { get; set; }
        public TituloPagarAdiantamento TituloPagarAdiantamento { get; set; }
        public decimal Valor { get; set; }
        public decimal Percentual { get; set; }
    }
}