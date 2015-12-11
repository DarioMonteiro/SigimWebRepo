using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Sigim;

namespace GIR.Sigim.Domain.Entity.Estoque
{
    public class EstoqueMaterial : BaseEntity
    {
        public int? EstoqueId { get; set; }
        public Estoque Estoque { get; set; }
        public int? MaterialId { get; set; }
        public Material Material { get; set; }
        public decimal? Quantidade { get; set; }
        public decimal? Valor { get; set; }
        public decimal? QuantidadeTemporaria { get; set; }

        public EstoqueMaterial()
        {
            this.Quantidade = 0;
            this.Valor = 0;
            this.QuantidadeTemporaria = 0;
        }
    }
}