using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Contrato;

namespace GIR.Sigim.Domain.Entity.Sigim
{
    public class Servico : BaseEntity  
    {
        public string Descricao { get; set; }
        public string SiglaUnidadeMedida { get; set; }
        public UnidadeMedida UnidadeMedida { get; set; }
        public decimal? PrecoUnitario { get; set; }
        public string Situacao { get; set; }

        public ICollection<ContratoRetificacaoItem> ListaContratoRetificacaoItem { get; set; }

        public Servico()
        {
            this.ListaContratoRetificacaoItem = new HashSet<ContratoRetificacaoItem>();  
        }
    }
}
