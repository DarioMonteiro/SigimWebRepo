﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Financeiro;

namespace GIR.Sigim.Domain.Entity.Orcamento
{
    public class Obra : BaseEntity
    {
        public string Numero { get; set; }
        public string Descricao { get; set; }
        public string CodigoCentroCusto { get; set; }
        public CentroCusto CentroCusto { get; set; }
        public ICollection<Orcamento> ListaOrcamento { get; set; }

        public Obra()
        {
            this.ListaOrcamento = new HashSet<Orcamento>();
        }
    }
}