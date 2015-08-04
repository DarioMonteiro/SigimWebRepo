using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.OrdemCompra;
using GIR.Sigim.Domain.Entity.Contrato; 

namespace GIR.Sigim.Domain.Entity.Financeiro
{
    public class TipoMovimento : BaseEntity
    {
        public string Descricao { get; set; }
        public int? HistoricoContabilId { get; set; }
        public HistoricoContabil HistoricoContabil { get; set; }
        public string Operacao { get; set; }
        public string Tipo { get; set; }
    }
}