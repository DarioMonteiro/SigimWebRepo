using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Sac;
using GIR.Sigim.Application.Filtros.Sac;

namespace GIR.Sigim.Application.Service.Sac
{
    public interface ISetorAppService : IBaseAppService 
    {
        List<SetorDTO> ListarPeloFiltro(SetorFiltro filtro, out int totalRegistros);
        SetorDTO ObterPeloId(int? id);
        bool Salvar(SetorDTO dto);
        bool Deletar(int? id);
    }
}