﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIR.Sigim.Domain.Entity.Financeiro
{
    public enum OpcoesRelatorioApropriacaoPorClasse
    {
        [Description("Sintético")]
        Sintetico = 1,

        [Description("Analítico")]
        Analitico = 2,

        [Description("Analítico detalhado")]
        AnaliticoDetalhado = 3,

        [Description("Analítico detalhado fornecedor")]
        AnaliticoDetalhadoFornecedor = 4

    }

}
