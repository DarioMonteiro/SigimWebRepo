﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Admin;
using GIR.Sigim.Domain.Entity.Financeiro;

namespace GIR.Sigim.Domain.Repository.Financeiro
{
    public interface ICentroCustoEmpresaRepository : IRepository<CentroCustoEmpresa>
    {
        CentroCustoEmpresa ObterPeloCodigo(string codigo, params Expression<Func<Classe, object>>[] includes);
    }
}