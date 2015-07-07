﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Application.Filtros.Sigim;

namespace GIR.Sigim.Application.Service.Sigim
{
    public interface IAgenciaAppService
    {
        List<AgenciaDTO> ListarPeloFiltro(AgenciaFiltro filtro, int? idUsuario, out int totalRegistros);
        AgenciaDTO ObterPeloId(int? Id);
        bool Salvar(AgenciaDTO dto);
        bool Deletar(int? id);

    }
}