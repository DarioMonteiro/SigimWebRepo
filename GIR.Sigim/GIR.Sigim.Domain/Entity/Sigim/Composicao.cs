﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Orcamento;

namespace GIR.Sigim.Domain.Entity.Sigim
{
    public class Composicao : BaseEntity
    {
        public string Descricao { get; set; }
        public string UnidadeMedidaSigla { get; set; }
        public ICollection<OrcamentoComposicao> ListaOrcamentoComposicao { get; set; }
        public ICollection<OrcamentoInsumoRequisitado> ListaOrcamentoInsumoRequisitado { get; set; }

        public Composicao()
        {
            this.ListaOrcamentoComposicao = new HashSet<OrcamentoComposicao>();
            this.ListaOrcamentoInsumoRequisitado = new HashSet<OrcamentoInsumoRequisitado>();
        }
    }
}