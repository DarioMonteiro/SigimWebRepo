using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Financeiro;
using GIR.Sigim.Domain.Entity.Sigim;

namespace GIR.Sigim.Domain.Entity.OrdemCompra
{
    public abstract class AbstractRequisicaoMaterialItem : BaseEntity
    {
        public int? MaterialId { get; set; }
        public virtual Material Material { get; set; }
        public string CodigoClasse { get; set; }
        public virtual Classe Classe { get; set; }
        public int Sequencial { get; set; }
        public string Complemento { get; set; }
        public string UnidadeMedida { get; set; }
        public decimal Quantidade { get; set; }
        public decimal QuantidadeAprovada { get; set; }
        public Nullable<DateTime> DataMinima { get; set; }
        public Nullable<DateTime> DataMaxima { get; set; }
    }
}