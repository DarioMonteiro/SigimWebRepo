using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Orcamento;
using GIR.Sigim.Application.Filtros.Financeiro;
using GIR.Sigim.Domain.Specification;
using GIR.Sigim.Application.Filtros.Orcamento;

namespace GIR.Sigim.Application.Service.Orcamento
{
    public interface IOrcamentoAppService
    {
        OrcamentoDTO ObterPeloId(int? id);
        OrcamentoDTO ObterUltimoOrcamentoPeloCentroCusto(string codigoCentroCusto);
        OrcamentoDTO ObterUltimoOrcamentoPeloCentroCustoClasseOrcamento(string codigoCentroCusto);
        bool EhPermitidoImprimirRelOrcamento();
        List<OrcamentoDTO> PesquisarOrcamentosPeloFiltro(OrcamentoPesquisaFiltro filtro, out int totalRegistros);
    }
}