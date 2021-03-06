﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GIR.Sigim.Application.Adapter;
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
            Mapper.CreateMap<Apropriacao, ApropriacaoDTO>();
            Mapper.CreateMap<ApropriacaoDTO, Apropriacao>();

            Mapper.CreateMap<ApropriacaoClasseCCRelatorio, ApropriacaoClasseCCRelatorioDTO>();
            Mapper.CreateMap<ApropriacaoClasseCCRelatorioDTO, ApropriacaoClasseCCRelatorio>();

            Mapper.CreateMap<Caixa, CaixaDTO>();
            Mapper.CreateMap<CaixaDTO, Caixa>();

            Mapper.CreateMap<CentroCusto, CentroCustoDTO>();
            Mapper.CreateMap<CentroCustoDTO, CentroCusto>();

            Mapper.CreateMap<Classe, ClasseDTO>();
            Mapper.CreateMap<ClasseDTO, Classe>();

            Mapper.CreateMap<HistoricoContabil, HistoricoContabilDTO>();
            Mapper.CreateMap<HistoricoContabilDTO, HistoricoContabil>();

            Mapper.CreateMap<ImpostoFinanceiro, ImpostoFinanceiroDTO>()
                .ForMember(d => d.PeriodicidadeDescricao, m => m.MapFrom(s => s.Periodicidade.HasValue ? s.Periodicidade.ObterDescricao() : string.Empty))
                .ForMember(d => d.FimDeSemanaDescricao, m => m.MapFrom(s => s.FimDeSemana.HasValue ? s.FimDeSemana.ObterDescricao() : string.Empty))
                .ForMember(d => d.FatoGeradorDescricao, m => m.MapFrom(s => s.FatoGerador.HasValue ? s.FatoGerador.ObterDescricao() : string.Empty));
            Mapper.CreateMap<ImpostoFinanceiroDTO, ImpostoFinanceiro>();

            Mapper.CreateMap<ImpostoPagar, ImpostoPagarDTO>();
            Mapper.CreateMap<ImpostoPagarDTO, ImpostoPagar>();

            Mapper.CreateMap<ImpostoReceber, ImpostoReceberDTO>();
            Mapper.CreateMap<ImpostoReceberDTO, ImpostoReceber>();

            Mapper.CreateMap<MotivoCancelamento, MotivoCancelamentoDTO>();
            Mapper.CreateMap<MotivoCancelamentoDTO, MotivoCancelamento>();

            Mapper.CreateMap<MovimentoFinanceiro, MovimentoFinanceiroDTO>();
            Mapper.CreateMap<MovimentoFinanceiroDTO, MovimentoFinanceiro>();

            Mapper.CreateMap<ParametrosFinanceiro, ParametrosFinanceiroDTO>();
            Mapper.CreateMap<ParametrosFinanceiroDTO, ParametrosFinanceiro>()
                .ForMember(d => d.IconeRelatorio, m => m.ResolveUsing(s => s.IconeRelatorio == null ? null : s.IconeRelatorio));

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

            Mapper.CreateMap<TipoMovimento, TipoMovimentoDTO>();
            Mapper.CreateMap<TipoMovimentoDTO, TipoMovimento>();

            Mapper.CreateMap<TipoRateio, TipoRateioDTO>();
            Mapper.CreateMap<TipoRateioDTO, TipoRateio>();

            Mapper.CreateMap<TituloPagar, TituloPagarDTO>()
                .ForMember(d => d.FormaPagamento, m => m.MapFrom(s => s.FormaPagamento.HasValue ? (FormaPagamento)s.FormaPagamento.Value : (FormaPagamento?)null));
            Mapper.CreateMap<TituloPagarDTO, TituloPagar>();


            Mapper.CreateMap<TituloReceber, TituloReceberDTO>();
            Mapper.CreateMap<TituloReceberDTO, TituloReceber>();

        }
    }
}