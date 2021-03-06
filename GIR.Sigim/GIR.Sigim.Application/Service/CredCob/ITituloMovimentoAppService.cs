﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Specification;
using GIR.Sigim.Domain.Entity.CredCob;
using GIR.Sigim.Application.Filtros.Financeiro;

namespace GIR.Sigim.Application.Service.CredCob
{
    public interface ITituloMovimentoAppService
    {
        Specification<TituloMovimento> MontarSpecificationTituloMovimentoRelApropriacaoPorClasse(RelApropriacaoPorClasseFiltro filtro, int? idUsuario);
    }
}
