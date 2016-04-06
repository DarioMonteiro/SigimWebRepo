﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.GirCliente;

namespace GIR.Sigim.Application.Service.Sigim
{
    public interface IAcessoAppService
    {
        ClienteAcessoChaveAcesso ObterInfoAcesso(string textCripto);
        bool ValidaSistemaBloqueado(string NomeModulo);
    }
}
