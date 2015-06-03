using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Application.Filtros;

namespace GIR.Sigim.Application.Service.Sigim
{
    public interface IParentescoAppService
    {
        List<ParentescoDTO> ListarTodos();
        List<ParentescoDTO> ListarPeloFiltro(BaseFiltro filtro, out int totalRegistros);
        ParentescoDTO ObterPeloId(int? id);
        bool Salvar(ParentescoDTO dto);
        bool Deletar(int? id);
    }
}