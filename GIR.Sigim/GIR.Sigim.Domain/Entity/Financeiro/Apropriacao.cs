using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Estoque;

namespace GIR.Sigim.Domain.Entity.Financeiro
{
    public class Apropriacao : BaseEntity
    {
        public string CodigoClasse { get; set; }
        public Classe Classe { get; set; }
        public string CodigoCentroCusto { get; set; }
        public CentroCusto CentroCusto { get; set; }
        public int? TituloPagarId { get; set; }
        public TituloPagar TituloPagar { get; set; }
        //TituloReceber
        public int? MovimentoId { get; set; }
        public Movimento Movimento { get; set; }
        public decimal Valor { get; set; }
        public decimal Percentual { get; set; }
    }
}