using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Financeiro;
using GIR.Sigim.Application.Filtros.Financeiro;
using GIR.Sigim.Application.DTO.Sigim;

namespace GIR.Sigim.Application.Service.Financeiro
{
    public interface ITipoCompromissoAppService
    {
        List<TipoCompromissoDTO> ListarTipoPagar();
        List<TipoCompromissoDTO> ListarPeloFiltro(TipoCompromissoFiltro filtro, out int totalRegistros);
        TipoCompromissoDTO ObterPeloId(int? id);
        bool Salvar(TipoCompromissoDTO dto);
        bool Deletar(int? id);
        bool EhPermitidoSalvar();
        bool EhPermitidoDeletar();
        bool EhPermitidoImprimir();
        FileDownloadDTO ExportarRelTipoCompromisso(FormatoExportacaoArquivo formato);

    }
}