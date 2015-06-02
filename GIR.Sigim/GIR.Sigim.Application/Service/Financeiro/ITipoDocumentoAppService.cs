using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Financeiro;
using GIR.Sigim.Application.DTO;
using GIR.Sigim.Application.Filtros;

namespace GIR.Sigim.Application.Service.Financeiro
{
    public interface ITipoDocumentoAppService
    {
        List<TipoDocumentoDTO> ListarTodos();
        List<TipoDocumentoDTO> ListarPeloFiltro(BaseFiltro filtro, out int totalRegistros);
        TipoDocumentoDTO ObterPeloId(int? id);
        bool Salvar(TipoDocumentoDTO dto);
        bool Deletar(int? id);
    }
}
