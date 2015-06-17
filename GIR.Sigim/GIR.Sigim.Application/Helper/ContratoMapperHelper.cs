using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GIR.Sigim.Application.Adapter;
using GIR.Sigim.Application.DTO.Contrato;
using GIR.Sigim.Domain.Entity.Contrato;

namespace GIR.Sigim.Application.Helper
{
    public class ContratoMapperHelper
    {
        public static void Initialise()
        {
            Mapper.CreateMap<Contrato, ContratoDTO>()
                .ForMember(d => d.SituacaoDescricao, m => m.MapFrom(s => s.Situacao.ObterDescricao()));
            Mapper.CreateMap<ContratoDTO, Contrato>()
                .ForMember(d => d.CentroCusto, m => m.UseValue(null))
                .ForMember(d => d.CodigoCentroCusto, m => m.MapFrom(s => s.CentroCusto.Codigo));

            Mapper.CreateMap<ContratoRetificacao, ContratoRetificacaoDTO>();
            Mapper.CreateMap<ContratoRetificacaoDTO, ContratoRetificacao>();

            Mapper.CreateMap<ContratoRetificacaoItem, ContratoRetificacaoItemDTO>();
            Mapper.CreateMap<ContratoRetificacaoItemDTO, ContratoRetificacaoItem>();

            Mapper.CreateMap<ContratoRetificacaoItemCronograma, ContratoRetificacaoItemCronogramaDTO>();
            Mapper.CreateMap<ContratoRetificacaoItemCronogramaDTO, ContratoRetificacaoItemCronograma>();

            Mapper.CreateMap<ContratoRetificacaoItemImposto, ContratoRetificacaoItemImpostoDTO>();
            Mapper.CreateMap<ContratoRetificacaoItemImpostoDTO, ContratoRetificacaoItemImposto>();

            Mapper.CreateMap<ContratoRetificacaoItemMedicao, ContratoRetificacaoItemMedicaoDTO>();
            Mapper.CreateMap<ContratoRetificacaoItemMedicaoDTO, ContratoRetificacaoItemMedicao>();

            Mapper.CreateMap<ContratoRetificacaoProvisao, ContratoRetificacaoProvisaoDTO>()
                .ForMember(d => d.ValorTotalMedido, m => m.MapFrom(s => s.Contrato.ObterValorTotalMedido(s.SequencialItem, s.SequencialCronograma)));
            Mapper.CreateMap<ContratoRetificacaoProvisaoDTO, ContratoRetificacaoProvisao>();

            Mapper.CreateMap<Licitacao, LicitacaoDTO>();
            Mapper.CreateMap<LicitacaoDTO, Licitacao>();

            Mapper.CreateMap<LicitacaoCronograma, LicitacaoCronogramaDTO>();
            Mapper.CreateMap<LicitacaoCronogramaDTO, LicitacaoCronograma>();

            Mapper.CreateMap<LicitacaoDescricao, LicitacaoDescricaoDTO>();
            Mapper.CreateMap<LicitacaoDescricaoDTO, LicitacaoDescricao>();

            Mapper.CreateMap<ParametrosContrato, ParametrosContratoDTO>();
            Mapper.CreateMap<ParametrosContratoDTO, ParametrosContrato>()
                .ForMember(d => d.IconeRelatorio, m => m.ResolveUsing(s => s.IconeRelatorio == null ? null : s.IconeRelatorio));
        }
    }
}