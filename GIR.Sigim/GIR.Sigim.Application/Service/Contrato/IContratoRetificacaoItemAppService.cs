﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Contrato; 

namespace GIR.Sigim.Application.Service.Contrato
{
    public interface IContratoRetificacaoItemAppService : IBaseAppService 
    {
        bool EhNaturezaItemPrecoGlobal(ContratoRetificacaoItemDTO dto);
        bool EhNaturezaItemPrecoUnitario(ContratoRetificacaoItemDTO dto);
    }
}
