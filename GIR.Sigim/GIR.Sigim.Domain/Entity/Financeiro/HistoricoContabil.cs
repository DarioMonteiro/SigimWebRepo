using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Contrato;
using GIR.Sigim.Domain.Entity.OrdemCompra;
using GIR.Sigim.Domain.Entity.Sigim;

namespace GIR.Sigim.Domain.Entity.Financeiro
{
    public class HistoricoContabil : BaseEntity
    {
        public string Descricao { get; set; }
        public int? Tipo { get; set; }
        public ICollection<TipoMovimento> ListaTipoMovimento { get; set; }

        public HistoricoContabil()
        {
            this.ListaTipoMovimento = new HashSet<TipoMovimento>();
        }
    }
}