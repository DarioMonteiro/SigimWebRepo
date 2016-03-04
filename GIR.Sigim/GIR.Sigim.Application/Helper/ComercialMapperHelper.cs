using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GIR.Sigim.Domain.Entity.Comercial;
using GIR.Sigim.Application.DTO.Comercial;

namespace GIR.Sigim.Application.Helper
{
    public class ComercialMapperHelper
    {
        public static void Initialise()
        {

            Mapper.CreateMap<Bloco, BlocoDTO>();
            Mapper.CreateMap<BlocoDTO, Bloco>();

            Mapper.CreateMap<ContratoComercial, ContratoComercialDTO>();
            Mapper.CreateMap<ContratoComercialDTO, ContratoComercial>();

            Mapper.CreateMap<Empreendimento, EmpreendimentoDTO>();
            Mapper.CreateMap<EmpreendimentoDTO, Empreendimento>();

            Mapper.CreateMap<Incorporador, IncorporadorDTO>();
            Mapper.CreateMap<IncorporadorDTO, Incorporador>();

            Mapper.CreateMap<Renegociacao, RenegociacaoDTO>();
            Mapper.CreateMap<RenegociacaoDTO, Renegociacao>();

            Mapper.CreateMap<TipoParticipante, TipoParticipanteDTO>();
            Mapper.CreateMap<TipoParticipanteDTO, TipoParticipante>();

            Mapper.CreateMap<Unidade, UnidadeDTO>();
            Mapper.CreateMap<UnidadeDTO, Unidade>();

            Mapper.CreateMap<Venda, VendaDTO>();
            Mapper.CreateMap<VendaDTO, Venda>();

            Mapper.CreateMap<Venda, RelStatusVendaDTO>();
                //.ForMember(d => d.Venda.Id, m => m.MapFrom(s => s.Id))
                //.ForMember(d => d.Valor, m => m.MapFrom(s => s.ValorFrete))
                //.ForMember(d => d.TituloPagarId, m => m.MapFrom(s => s.TituloFreteId));
            //Mapper.CreateMap<RelStatusVendaDTO, Venda>();

            Mapper.CreateMap<VendaParticipante, VendaParticipanteDTO>();
            Mapper.CreateMap<VendaParticipanteDTO, VendaParticipante>();

            Mapper.CreateMap<VendaSerie, VendaSerieDTO>();
            Mapper.CreateMap<VendaSerieDTO, VendaSerie>();

            Mapper.CreateMap<TabelaVenda, TabelaVendaDTO>();
            Mapper.CreateMap<TabelaVendaDTO, TabelaVenda>();

        }
    }
}
