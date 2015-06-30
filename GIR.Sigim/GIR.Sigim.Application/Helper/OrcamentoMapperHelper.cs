using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GIR.Sigim.Application.DTO.Orcamento;
using GIR.Sigim.Domain.Entity.Orcamento;

namespace GIR.Sigim.Application.Helper
{
    public class OrcamentoMapperHelper
    {
        public static void Initialise()
        {
            Mapper.CreateMap<Obra, ObraDTO>();
            Mapper.CreateMap<ObraDTO, Obra>();

            Mapper.CreateMap<OrcamentoComposicao, OrcamentoComposicaoDTO>();
            Mapper.CreateMap<OrcamentoComposicaoDTO, OrcamentoComposicao>();

            Mapper.CreateMap<OrcamentoComposicaoItem, OrcamentoComposicaoItemDTO>()
                .ForMember(d => d.Classe, m => m.MapFrom(s => s.OrcamentoComposicao.Classe))
                .ForMember(d => d.Composicao, m => m.MapFrom(s => s.OrcamentoComposicao.Composicao));
            Mapper.CreateMap<OrcamentoComposicaoItemDTO, OrcamentoComposicaoItem>();

            Mapper.CreateMap<Orcamento, OrcamentoDTO>();
            Mapper.CreateMap<OrcamentoDTO, Orcamento>();

            Mapper.CreateMap<OrcamentoInsumoRequisitado, OrcamentoInsumoRequisitadoDTO>();
            Mapper.CreateMap<OrcamentoInsumoRequisitadoDTO, OrcamentoInsumoRequisitado>();

            Mapper.CreateMap<ParametrosOrcamento, ParametrosOrcamentoDTO>();
            Mapper.CreateMap<ParametrosOrcamentoDTO, ParametrosOrcamento>()
                .ForMember(d => d.IconeRelatorio, m => m.ResolveUsing(s => s.IconeRelatorio == null ? null : s.IconeRelatorio));
        }
    }
}