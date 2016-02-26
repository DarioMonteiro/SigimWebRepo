using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GIR.Sigim.Domain.Entity.CredCob;
using GIR.Sigim.Application.DTO.CredCob;

namespace GIR.Sigim.Application.Helper
{
    public class CredCobMapperHelper
    {
        public static void Initialise()
        {
            Mapper.CreateMap<TituloCredCob, TituloCredCobDTO>();
            Mapper.CreateMap<TituloCredCobDTO, TituloCredCob>();

            Mapper.CreateMap<TituloCredCob,TituloDetalheCredCobDTO>();

            Mapper.CreateMap<VerbaCobranca, VerbaCobrancaDTO>();
            Mapper.CreateMap<VerbaCobrancaDTO, VerbaCobranca>();

        }
    }
}
