﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Financeiro;
using GIR.Sigim.Application.DTO;
using GIR.Sigim.Application.Filtros;

namespace GIR.Sigim.Application.Service.Financeiro
{
    public interface ITipoMovimentoAppService
    {
        List<TipoMovimentoDTO> ListarTodos();
        List<TipoMovimentoDTO> ListarPeloFiltro(BaseFiltro filtro, out int totalRegistros);
        TipoMovimentoDTO ObterPeloId(int? id);
        bool Salvar(TipoMovimentoDTO dto);
        bool Deletar(int? id);
    }
}