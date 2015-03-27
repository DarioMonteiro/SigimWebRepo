using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Financeiro;

namespace GIR.Sigim.Application.DTO.Orcamento
{
    public class ObraDTO : BaseDTO
    {
        public string Numero { get; set; }
        public string Descricao { get; set; }
        public CentroCustoDTO CentroCusto { get; set; }
    }
}