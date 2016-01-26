using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Application.Filtros.Financeiro;

namespace GIR.Sigim.Application.Service.Financeiro
{
    public interface IApropriacaoAppService
    {
        List<ItemListaDTO> ListarTipoPesquisaRelatorioApropriacaoPorClasse();
        List<ItemListaDTO> ListarOpcoesRelatorioApropriacaoPorClasse();
        bool EhPermitidoImprimirRelApropriacaoPorClasse();
        FileDownloadDTO ExportarRelApropriacaoPorClasse(RelApropriacaoPorClasseFiltro filtro, int? usuarioId, FormatoExportacaoArquivo formato);
    }
}
