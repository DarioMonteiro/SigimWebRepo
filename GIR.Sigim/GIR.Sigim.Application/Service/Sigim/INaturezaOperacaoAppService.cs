﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Sigim;

namespace GIR.Sigim.Application.Service.Sigim
{
    public interface INaturezaOperacaoAppService
    {
        List<NaturezaOperacaoDTO> ListarTodos();
    }
}