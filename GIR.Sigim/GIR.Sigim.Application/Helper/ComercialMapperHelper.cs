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

            Mapper.CreateMap<TipoParticipante, TipoParticipanteDTO>();
            Mapper.CreateMap<TipoParticipanteDTO, TipoParticipante>();

            Mapper.CreateMap<Unidade, UnidadeDTO>();
            Mapper.CreateMap<UnidadeDTO, Unidade>();

            Mapper.CreateMap<Venda, VendaDTO>();
            Mapper.CreateMap<VendaDTO, Venda>();

            Mapper.CreateMap<VendaParticipante, VendaParticipanteDTO>();
            Mapper.CreateMap<VendaParticipanteDTO, VendaParticipante>();

            //Mapper.CreateMap<VendaSerie, VendaSerieDTO>();
            //Mapper.CreateMap<VendaSerieDTO, VendaSerie>();

        }
    }
}
