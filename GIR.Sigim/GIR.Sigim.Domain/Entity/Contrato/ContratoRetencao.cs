using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Domain.Entity.Contrato
{
    public class ContratoRetencao : BaseEntity
    {
        public int? ContratoId { get; set; }
        public Contrato Contrato { get; set; }
        public int ContratoRetificacaoId { get; set; }
        public ContratoRetificacao ContratoRetificacao { get; set; }
        public int ContratoRetificacaoItemId { get; set; }
        public ContratoRetificacaoItem ContratoRetificacaoItem { get; set; }
        public int ContratoRetificacaoItemMedicaoId { get; set; }
        public ContratoRetificacaoItemMedicao ContratoRetificacaoItemMedicao { get; set; }
        public decimal? ValorRetencao { get; set; }
        public ICollection<ContratoRetencaoLiberada> ListaContratoRetencaoLiberada { get; set; }

        public ContratoRetencao()
        {
            this.ListaContratoRetencaoLiberada = new HashSet<ContratoRetencaoLiberada>();
        }
    }
}
