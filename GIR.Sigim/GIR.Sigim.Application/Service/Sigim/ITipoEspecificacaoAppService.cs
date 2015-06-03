using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Application.Filtros;

namespace GIR.Sigim.Application.Service.Sigim
{
    public interface ITipoEspecificacaoAppService
    {
        List<TipoEspecificacaoDTO> ListarTodos();
        List<TipoEspecificacaoDTO> ListarPeloFiltro(BaseFiltro filtro, out int totalRegistros);
        TipoEspecificacaoDTO ObterPeloId(int? id);
        bool Salvar(TipoEspecificacaoDTO dto);
        bool Deletar(int? id);
    }
}