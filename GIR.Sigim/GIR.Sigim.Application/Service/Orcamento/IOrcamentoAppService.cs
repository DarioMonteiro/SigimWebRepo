﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Orcamento;

namespace GIR.Sigim.Application.Service.Orcamento
{
    public interface IOrcamentoAppService
    {
        OrcamentoDTO ObterUltimoOrcamentoPeloCentroCusto(string codigoCentroCusto);
        OrcamentoDTO ObterUltimoOrcamentoPeloCentroCustoClasseOrcamento(string codigoCentroCusto);
    }
}