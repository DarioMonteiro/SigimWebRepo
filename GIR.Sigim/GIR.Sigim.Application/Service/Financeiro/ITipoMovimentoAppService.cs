using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Financeiro;
using GIR.Sigim.Application.DTO;
using GIR.Sigim.Application.Filtros;
using GIR.Sigim.Application.DTO.Sigim;

namespace GIR.Sigim.Application.Service.Financeiro
{
    public interface ITipoMovimentoAppService
    {
        List<TipoMovimentoDTO> ListarTodos();
        List<TipoMovimentoDTO> ListarNaoAutomatico(BaseFiltro filtro, out int totalRegistros);
        TipoMovimentoDTO ObterPeloId(int? id);
        bool Salvar(TipoMovimentoDTO dto);
        bool Deletar(int? id);
        bool EhPermitidoSalvar();
        bool EhPermitidoDeletar();
        bool EhPermitidoImprimir();
        FileDownloadDTO ExportarRelTipoMovimento(FormatoExportacaoArquivo formato);
    }
}
