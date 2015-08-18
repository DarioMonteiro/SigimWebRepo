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

            Mapper.CreateMap<ContratoRetificacaoItemMedicao, ContratoRetificacaoItemMedicaoDTO>()
                .ForMember(d => d.QuantidadeTotalMedida, m => m.MapFrom(s => s.Contrato.ObterQuantidadeTotalMedida(s.SequencialItem, s.SequencialCronograma)))
                .ForMember(d => d.ValorTotalMedido, m => m.MapFrom(s => s.Contrato.ObterValorTotalMedido(s.SequencialItem, s.SequencialCronograma)))
                .ForMember(d => d.QuantidadeTotalLiberada, m => m.MapFrom(s => s.Contrato.ObterQuantidadeTotalLiberada(s.SequencialItem, s.SequencialCronograma)))
                .ForMember(d => d.ValorTotalLiberado, m => m.MapFrom(s => s.Contrato.ObterValorTotalLiberado(s.SequencialItem, s.SequencialCronograma)))
                .ForMember(d => d.QuantidadeTotalMedidaLiberada, m => m.MapFrom(s => s.Contrato.ObterQuantidadeTotalMedidaLiberada(s.SequencialItem, s.SequencialCronograma)))
                .ForMember(d => d.ValorTotalMedidoLiberado, m => m.MapFrom(s => s.Contrato.ObterValorTotalMedidoLiberado(s.SequencialItem, s.SequencialCronograma)))
                .ForMember(d => d.ValorImpostoRetido, m => m.MapFrom(s => s.Contrato.ObterValorImpostoRetido(s.SequencialItem, s.SequencialCronograma, s.ContratoRetificacaoItemId)))
                .ForMember(d => d.ValorImpostoRetidoMedicao, m => m.MapFrom(s => s.Contrato.ObterValorImpostoRetidoMedicao(s.SequencialItem, s.SequencialCronograma, s.ContratoRetificacaoItemId,s.Id)))
                .ForMember(d => d.ValorImpostoIndiretoMedicao, m => m.MapFrom(s => s.Contrato.ObterValorTotalImpostoIndiretoMedicao(s.SequencialItem, s.SequencialCronograma, s.ContratoRetificacaoItemId, s.Id)))
                .ForMember(d => d.ValorTotalMedidoIndireto, m => m.MapFrom(s => s.Contrato.ObterValorTotalMedidoIndireto(s.ContratoId, s.NumeroDocumento, s.TipoDocumentoId, s.DataVencimento)))
                .ForMember(d => d.ValorTotalMedidoNota, m => m.MapFrom(s => s.Contrato.ObterValorTotalMedidoNota(s.ContratoId, s.NumeroDocumento, s.TipoDocumentoId, s.DataVencimento)))
                .ForMember(d => d.ValorTotalMedidoLiberadoContrato, m => m.MapFrom(s => s.Contrato.ObterValorTotalMedidoLiberadoContrato(s.ContratoId)));
            Mapper.CreateMap<ContratoRetificacaoItemMedicaoDTO, ContratoRetificacaoItemMedicao>()
                .ForMember(d => d.MultiFornecedor, m => m.UseValue(null))
                .ForMember(d => d.MultiFornecedorId, m => m.MapFrom(s => s.MultiFornecedor.Id));

            Mapper.CreateMap<ContratoRetificacaoProvisao, ContratoRetificacaoProvisaoDTO>()
                .ForMember(d => d.QuantidadeTotalMedida, m => m.MapFrom(s => s.Contrato.ObterQuantidadeTotalMedida(s.SequencialItem, s.SequencialCronograma)))
                .ForMember(d => d.ValorTotalMedido, m => m.MapFrom(s => s.Contrato.ObterValorTotalMedido(s.SequencialItem, s.SequencialCronograma)))
                .ForMember(d => d.QuantidadeTotalLiberada, m => m.MapFrom(s => s.Contrato.ObterQuantidadeTotalLiberada(s.SequencialItem, s.SequencialCronograma)))
                .ForMember(d => d.ValorTotalLiberado, m => m.MapFrom(s => s.Contrato.ObterValorTotalLiberado(s.SequencialItem, s.SequencialCronograma)))
                .ForMember(d => d.QuantidadeTotalMedidaLiberada, m => m.MapFrom(s => s.Contrato.ObterQuantidadeTotalMedidaLiberada(s.SequencialItem, s.SequencialCronograma)))
                .ForMember(d => d.ValorTotalMedidoLiberado, m => m.MapFrom(s => s.Contrato.ObterValorTotalMedidoLiberado(s.SequencialItem, s.SequencialCronograma)));
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