﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Sac;
using System.Linq.Expressions;

namespace GIR.Sigim.Domain.Repository.Sac
{
    public interface IParametrosSacRepository : IRepository<ParametrosSac>
    {
        ParametrosSac Obter();
    }
}