﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Sigim;

namespace GIR.Sigim.Domain.Repository.Sigim
{
    public interface IUnidadeMedidaRepository : IRepository<UnidadeMedida> 
    {
        UnidadeMedida ObterPeloCodigo(string sigla, params Expression<Func<UnidadeMedida, object>>[] includes);
    }
}
