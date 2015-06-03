using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Application.Filtros;

namespace GIR.Sigim.Application.Service.Sigim
{
    public interface IRelacionamentoAppService
    {
        List<RelacionamentoDTO> ListarTodos();
        List<RelacionamentoDTO> ListarPeloFiltro(BaseFiltro filtro, out int totalRegistros);
        RelacionamentoDTO ObterPeloId(int? id);
        bool Salvar(RelacionamentoDTO dto);
        bool Deletar(int? id);
    }
}