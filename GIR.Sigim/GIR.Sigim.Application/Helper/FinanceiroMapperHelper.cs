﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GIR.Sigim.Application.DTO.Financeiro;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Application.Enums;
using GIR.Sigim.Domain.Entity.Financeiro;

namespace GIR.Sigim.Application.Helper
{
    public class FinanceiroMapperHelper
    {
        public static void Initialise()
        {
            Mapper.CreateMap<Caixa, CaixaDTO>();
            Mapper.CreateMap<CaixaDTO, Caixa>();

            Mapper.CreateMap<CentroCusto, CentroCustoDTO>();
            Mapper.CreateMap<CentroCustoDTO, CentroCusto>();

            Mapper.CreateMap<Classe, ClasseDTO>();
            Mapper.CreateMap<ClasseDTO, Classe>();

            Mapper.CreateMap<ImpostoFinanceiro, ImpostoFinanceiroDTO>();
            Mapper.CreateMap<ImpostoFinanceiroDTO, ImpostoFinanceiro>();

            Mapper.CreateMap<ImpostoPagar, ImpostoPagarDTO>();
            Mapper.CreateMap<ImpostoPagarDTO, ImpostoPagar>();

            Mapper.CreateMap<MotivoCancelamento, MotivoCancelamentoDTO>();
            Mapper.CreateMap<MotivoCancelamentoDTO, MotivoCancelamento>();

            Mapper.CreateMap<ParametrosFinanceiro, ParametrosFinanceiroDTO>();
            Mapper.CreateMap<ParametrosFinanceiroDTO, ParametrosFinanceiro>();

            Mapper.CreateMap<ParametrosUsuarioFinanceiro, ParametrosUsuarioFinanceiroDTO>();
            Mapper.CreateMap<ParametrosUsuarioFinanceiroDTO, ParametrosUsuarioFinanceiro>();

            Mapper.CreateMap<RateioAutomatico, RateioAutomaticoDTO>();
            Mapper.CreateMap<RateioAutomaticoDTO, RateioAutomatico>();

            Mapper.CreateMap<TaxaAdministracao, TaxaAdministracaoDTO>();
            Mapper.CreateMap<TaxaAdministracaoDTO, TaxaAdministracao>();

            Mapper.CreateMap<TipoCompromisso, TipoCompromissoDTO>();
            Mapper.CreateMap<TipoCompromissoDTO, TipoCompromisso>();

            Mapper.CreateMap<TipoDocumento, TipoDocumentoDTO>();
            Mapper.CreateMap<TipoDocumentoDTO, TipoDocumento>();

            Mapper.CreateMap<TipoRateio, TipoRateioDTO>();
            Mapper.CreateMap<TipoRateioDTO, TipoRateio>();

            Mapper.CreateMap<TituloPagar, TituloPagarDTO>();
            Mapper.CreateMap<TituloPagarDTO, TituloPagar>();

            Mapper.CreateMap<TituloReceber, TituloReceberDTO>();
            Mapper.CreateMap<TituloReceberDTO, TituloReceber>();

        }
    }
}