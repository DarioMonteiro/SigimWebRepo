﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Comercial;

namespace GIR.Sigim.Domain.Repository.Comercial
{
    public interface IBlocoRepository : IRepository<Bloco>
    {
        List<Bloco> ListarPeloEmpreendimento(int empreendimentoId);
    }
}