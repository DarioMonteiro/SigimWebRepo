﻿using System;
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
    public interface IModuloAppService
    {
        List<ModuloDTO> ListarTodosWEB();
        List<ModuloDTO> ListarTodos();
        ModuloDTO ObterPeloId(int? id);
        ModuloDTO ObterPeloNome(string nomeModulo);
        bool PossuiModulo(string nomeModulo);
        bool AtualizaBloqueio(int ModuloId, bool bloqueio);
    }
}