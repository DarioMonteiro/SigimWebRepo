using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace GIR.Sigim.Application.Enums
{
    public enum OrigemChamada
    {
        [Description("Medição do Contrato")]
        MedicaoContrato = 0,

        [Description("Liberação da Medição")]
        LiberacaoContrato = 1,
    }
}
