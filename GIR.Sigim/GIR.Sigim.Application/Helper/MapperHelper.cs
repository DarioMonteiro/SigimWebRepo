using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GIR.Sigim.Application.Adapter;
using GIR.Sigim.Application.DTO.Admin;
using GIR.Sigim.Application.DTO.Contrato;
using GIR.Sigim.Application.DTO.Financeiro;
using GIR.Sigim.Application.DTO.Orcamento;
using GIR.Sigim.Application.DTO.OrdemCompra;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Application.DTO.Sac;
using GIR.Sigim.Domain.Entity.Admin;
using GIR.Sigim.Domain.Entity.Contrato;
using GIR.Sigim.Domain.Entity.Financeiro;
using GIR.Sigim.Domain.Entity.Orcamento;
using GIR.Sigim.Domain.Entity.OrdemCompra;
using GIR.Sigim.Domain.Entity.Sigim;
using GIR.Sigim.Domain.Entity.Sac;
using GIR.Sigim.Infrastructure.Crosscutting.Adapter;
using GIR.Sigim.Application.Enums;

namespace GIR.Sigim.Application.Helper
{
    public class MapperHelper
    {
        public static void Initialise()
        {
            AdminMapperHelper.Initialise();
            ContratoMapperHelper.Initialise();
            FinanceiroMapperHelper.Initialise();
            OrcamentoMapperHelper.Initialise();
            OrdemCompraMapperHelper.Initialise();
            SigimMapperHelper.Initialise();
            SacMapperHelper.Initialise();
        }
    }
}