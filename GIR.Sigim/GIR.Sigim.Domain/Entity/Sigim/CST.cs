using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Contrato;
using GIR.Sigim.Domain.Entity.OrdemCompra;

namespace GIR.Sigim.Domain.Entity.Sigim
{
    public class CST : BaseEntity
    {
        public string Codigo { get; set; }
        public string Descricao { get; set; }

        public ICollection<ContratoRetificacaoItemMedicao> ListaContratoRetificacaoItemMedicao { get; set; }
        public ICollection<EntradaMaterial> ListaEntradaMaterial { get; set; }

        public CST()
        {
            this.ListaContratoRetificacaoItemMedicao = new HashSet<ContratoRetificacaoItemMedicao>();
            this.ListaEntradaMaterial = new HashSet<EntradaMaterial>();
        }
    }
}