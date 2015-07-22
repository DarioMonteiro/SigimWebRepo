using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Financeiro;
using GIR.Sigim.Domain.Entity.OrdemCompra;
using GIR.Sigim.Domain.Entity.Sigim;

namespace GIR.Sigim.Domain.Entity.Estoque
{
    public class MovimentoItem : BaseEntity
    {
        public int? MovimentoId { get; set; }
        public Movimento Movimento { get; set; }
        public int? MaterialId { get; set; }
        public Material Material { get; set; }
        public string CodigoClasse { get; set; }
        public Classe Classe { get; set; }
        public decimal? Quantidade { get; set; }
        public decimal? QuantidadeSaldo { get; set; }
        public decimal? Valor { get; set; }
        public string Observacao { get; set; }
        public int? MovimentoDevolucaoId { get; set; }
        public Movimento MovimentoDevolucao { get; set; }
        public int? MovimentoItemDevolucaoId { get; set; }
        public MovimentoItem MovimentoItemDevolucao { get; set; }

        public MovimentoItem()
        {

        }
    }
}