using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Application.Filtros;

namespace GIR.Sigim.Application.Service.Sigim
{
    public interface IEstadoCivilAppService
    {
        List<EstadoCivilDTO> ListarTodos();
        List<EstadoCivilDTO> ListarPeloFiltro(BaseFiltro filtro, out int totalRegistros);
        EstadoCivilDTO ObterPeloId(int? id);
        bool Salvar(EstadoCivilDTO dto);
        bool Deletar(int? id);
    }
}