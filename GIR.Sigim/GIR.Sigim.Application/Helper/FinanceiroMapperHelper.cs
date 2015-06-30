using System;
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
            Mapper.CreateMap<CentroCusto, CentroCustoDTO>();
            Mapper.CreateMap<CentroCustoDTO, CentroCusto>();

            Mapper.CreateMap<Classe, ClasseDTO>();
            Mapper.CreateMap<ClasseDTO, Classe>();

            Mapper.CreateMap<Caixa, CaixaDTO>();
            Mapper.CreateMap<CaixaDTO, Caixa>();

            Mapper.CreateMap<ParametrosUsuarioFinanceiro, ParametrosUsuarioFinanceiroDTO>();
            Mapper.CreateMap<ParametrosUsuarioFinanceiroDTO, ParametrosUsuarioFinanceiro>();

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

            Mapper.CreateMap<MotivoCancelamento, MotivoCancelamentoDTO>();
            Mapper.CreateMap<MotivoCancelamentoDTO, MotivoCancelamento>();

            Mapper.CreateMap<ParametrosUsuarioFinanceiro, ParametrosUsuarioFinanceiroDTO>();
            Mapper.CreateMap<ParametrosUsuarioFinanceiroDTO, ParametrosUsuarioFinanceiro>();

            Mapper.CreateMap<ParametrosFinanceiro, ParametrosFinanceiroDTO>();
            Mapper.CreateMap<ParametrosFinanceiroDTO, ParametrosFinanceiro>();

            Mapper.CreateMap<ImpostoFinanceiro, ImpostoFinanceiroDTO>();
            Mapper.CreateMap<ImpostoFinanceiroDTO, ImpostoFinanceiro>();

            Mapper.CreateMap<RateioAutomatico, RateioAutomaticoDTO>();
            Mapper.CreateMap<RateioAutomaticoDTO, RateioAutomatico>();

            Mapper.CreateMap<AssuntoContatoDTO, TabelaBasicaDTO>()
                .ForMember(d => d.TipoTabela, m => m.UseValue((int)TabelaBasicaFinanceiro.AssuntoContato));
            Mapper.CreateMap<TabelaBasicaDTO, AssuntoContatoDTO>();

            Mapper.CreateMap<InteresseBairroDTO, TabelaBasicaDTO>()
                .ForMember(d => d.TipoTabela, m => m.UseValue((int)TabelaBasicaFinanceiro.BairroInteresse));
            Mapper.CreateMap<TabelaBasicaDTO, InteresseBairroDTO>();

            Mapper.CreateMap<EstadoCivilDTO, TabelaBasicaDTO>()
                .ForMember(d => d.TipoTabela, m => m.UseValue((int)TabelaBasicaFinanceiro.EstadoCivil));
            Mapper.CreateMap<TabelaBasicaDTO, EstadoCivilDTO>();

            Mapper.CreateMap<FonteNegocioDTO, TabelaBasicaDTO>()
                .ForMember(d => d.TipoTabela, m => m.UseValue((int)TabelaBasicaFinanceiro.FonteNegocio));
            Mapper.CreateMap<TabelaBasicaDTO, FonteNegocioDTO>();

            Mapper.CreateMap<GrupoDTO, TabelaBasicaDTO>()
                .ForMember(d => d.TipoTabela, m => m.UseValue((int)TabelaBasicaFinanceiro.Grupo));
            Mapper.CreateMap<TabelaBasicaDTO, GrupoDTO>();

            Mapper.CreateMap<NacionalidadeDTO, TabelaBasicaDTO>()
                .ForMember(d => d.TipoTabela, m => m.UseValue((int)TabelaBasicaFinanceiro.Nacionalidade));
            Mapper.CreateMap<TabelaBasicaDTO, NacionalidadeDTO>();

            Mapper.CreateMap<ParentescoDTO, TabelaBasicaDTO>()
                .ForMember(d => d.TipoTabela, m => m.UseValue((int)TabelaBasicaFinanceiro.Parentesco));
            Mapper.CreateMap<TabelaBasicaDTO, ParentescoDTO>();

            Mapper.CreateMap<ProfissaoDTO, TabelaBasicaDTO>()
                .ForMember(d => d.TipoTabela, m => m.UseValue((int)TabelaBasicaFinanceiro.Profissao));
            Mapper.CreateMap<TabelaBasicaDTO, ProfissaoDTO>();

            Mapper.CreateMap<RamoAtividadeDTO, TabelaBasicaDTO>()
                .ForMember(d => d.TipoTabela, m => m.UseValue((int)TabelaBasicaFinanceiro.RamoAtividade));
            Mapper.CreateMap<TabelaBasicaDTO, RamoAtividadeDTO>();

            Mapper.CreateMap<RelacionamentoDTO, TabelaBasicaDTO>()
                .ForMember(d => d.TipoTabela, m => m.UseValue((int)TabelaBasicaFinanceiro.Relacionamento));
            Mapper.CreateMap<TabelaBasicaDTO, RelacionamentoDTO>();

            Mapper.CreateMap<TipologiaDTO, TabelaBasicaDTO>()
                .ForMember(d => d.TipoTabela, m => m.UseValue((int)TabelaBasicaFinanceiro.Tipologia));
            Mapper.CreateMap<TabelaBasicaDTO, TipologiaDTO>();

            Mapper.CreateMap<TratamentoDTO, TabelaBasicaDTO>()
                .ForMember(d => d.TipoTabela, m => m.UseValue((int)TabelaBasicaFinanceiro.Tratamento));
            Mapper.CreateMap<TabelaBasicaDTO, TratamentoDTO>();

            Mapper.CreateMap<TipoAreaDTO, TabelaBasicaDTO>()
                .ForMember(d => d.TipoTabela, m => m.UseValue((int)TabelaBasicaFinanceiro.TipoArea));
            Mapper.CreateMap<TabelaBasicaDTO, TipoAreaDTO>();

            Mapper.CreateMap<TipoCaracteristicaDTO, TabelaBasicaDTO>()
                .ForMember(d => d.TipoTabela, m => m.UseValue((int)TabelaBasicaFinanceiro.TipoCaracteristica));
            Mapper.CreateMap<TabelaBasicaDTO, TipoCaracteristicaDTO>();

            Mapper.CreateMap<TipoEspecificacaoDTO, TabelaBasicaDTO>()
                .ForMember(d => d.TipoTabela, m => m.UseValue((int)TabelaBasicaFinanceiro.TipoEspecificacao));
            Mapper.CreateMap<TabelaBasicaDTO, TipoEspecificacaoDTO>();
        }
    }
}