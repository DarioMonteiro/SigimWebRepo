using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Contrato;
using GIR.Sigim.Domain.Entity.OrdemCompra;

namespace GIR.Sigim.Domain.Entity.Sigim
{
    public class CifFob : BaseEntity
    {
        public string Descricao { get; set; }
        public int? CodigoInterno { get; set; }

        public ICollection<ContratoRetificacaoItemMedicao> ListaContratoRetificacaoItemMedicao { get; set; }
        public ICollection<EntradaMaterial> ListaEntradaMaterial { get; set; }

        public CifFob()
        {
            this.ListaContratoRetificacaoItemMedicao = new HashSet<ContratoRetificacaoItemMedicao>();
            this.ListaEntradaMaterial = new HashSet<EntradaMaterial>();
        }
    }
}