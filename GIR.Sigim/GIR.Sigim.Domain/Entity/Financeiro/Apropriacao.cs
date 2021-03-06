﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public int? TituloReceberId { get; set; }
        public TituloReceber TituloReceber { get; set; }
        public int? MovimentoId { get; set; }
        public MovimentoFinanceiro Movimento { get; set; }
        public decimal Valor { get; set; }
        public decimal Percentual { get; set; }
    }
}