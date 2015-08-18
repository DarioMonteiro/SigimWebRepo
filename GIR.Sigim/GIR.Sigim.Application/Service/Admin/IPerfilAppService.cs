using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Admin;
using GIR.Sigim.Application.Filtros;
using GIR.Sigim.Application.Filtros.Admin;
using GIR.Sigim.Application.DTO.Sigim;

namespace GIR.Sigim.Application.Service.Admin
{
    public interface IPerfilAppService
    {
        List<PerfilDTO> ListarPeloModulo(int moduloId);
        List<PerfilDTO> ListarPeloFiltro(PerfilFiltro filtro, out int totalRegistros);
        PerfilDTO ObterPeloId(int? id);
        bool Salvar(PerfilDTO dto);
        bool Deletar(int? id);
    }
}