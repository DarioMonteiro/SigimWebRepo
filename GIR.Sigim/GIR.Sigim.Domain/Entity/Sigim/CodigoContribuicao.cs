using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Contrato;

namespace GIR.Sigim.Domain.Entity.Sigim
{
    public class CodigoContribuicao : BaseEntity
    {
        public string Codigo { get; set; }
        public string Descricao { get; set; }

        public ICollection<ContratoRetificacaoItemMedicao> ListaContratoRetificacaoItemMedicao { get; set; }

        public CodigoContribuicao()
        {
            this.ListaContratoRetificacaoItemMedicao = new HashSet<ContratoRetificacaoItemMedicao>();
        }
    }
}
