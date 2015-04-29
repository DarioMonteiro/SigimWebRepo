using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Contrato;

namespace GIR.Sigim.Domain.Entity.Financeiro
{
    public class TituloPagar : AbstractTitulo
    {
        public ContratoRetificacaoProvisao ContratoRetificacaoProvisao { get; set; }
    }
}
