using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Application.DTO.Financeiro;
using GIR.Sigim.Application.Filtros.Financeiro;

namespace GIR.Sigim.Application.Service.Financeiro
{
    public interface IApropriacaoAppService
    {
        List<ItemListaDTO> ListarTipoPesquisaRelatorioApropriacaoPorClasse();
        List<ItemListaDTO> ListarOpcoesRelatorioApropriacaoPorClasse();
        bool EhPermitidoImprimirRelApropriacaoPorClasse();
        List<ApropriacaoClasseCCRelatorioDTO> GerarRelatorioApropriacaoPorClasse(RelApropriacaoPorClasseFiltro filtro, int? usuarioId);
        FileDownloadDTO ExportarRelApropriacaoPorClasse(RelApropriacaoPorClasseFiltro filtro, List<ApropriacaoClasseCCRelatorioDTO> listaApropriacaoPorClasseDTO, FormatoExportacaoArquivo formato);
        List<RelAcompanhamentoFinanceiroDTO> ListarPeloFiltroRelAcompanhamentoFinanceiro(RelAcompanhamentoFinanceiroFiltro filtro, int? usuarioId, out int totalRegistros);
        List<RelAcompanhamentoFinanceiroDTO> ListarPeloFiltroRelAcompanhamentoFinanceiroExecutado(RelAcompanhamentoFinanceiroFiltro filtro, int? usuarioId, out int totalRegistros);
    }
}
