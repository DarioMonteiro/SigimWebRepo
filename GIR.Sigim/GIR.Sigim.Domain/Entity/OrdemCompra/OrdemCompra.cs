﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Domain.Entity.OrdemCompra
{
    public class OrdemCompra : BaseEntity
    {
        public DateTime Data { get; set; }
        public SituacaoOrdemCompra Situacao { get; set; }
        public virtual ICollection<OrdemCompraItem> ListaItens { get; set; }

        public OrdemCompra()
        {
            this.ListaItens = new HashSet<OrdemCompraItem>();
        }
    }
}