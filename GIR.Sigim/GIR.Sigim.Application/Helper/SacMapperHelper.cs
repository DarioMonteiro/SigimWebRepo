using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GIR.Sigim.Application.DTO.Sac;
using GIR.Sigim.Domain.Entity.Sac;

namespace GIR.Sigim.Application.Helper
{
    public class SacMapperHelper
    {
        public static void Initialise()
        {
            Mapper.CreateMap<ParametrosSac, ParametrosSacDTO>();
            Mapper.CreateMap<ParametrosSacDTO, ParametrosSac>()
                .ForMember(d => d.IconeRelatorio, m => m.ResolveUsing(s => s.IconeRelatorio == null ? null : s.IconeRelatorio));

            Mapper.CreateMap<Setor, SetorDTO>();
            Mapper.CreateMap<SetorDTO, Setor>();

            Mapper.CreateMap<ParametrosEmailSac, ParametrosEmailSacDTO>();
            Mapper.CreateMap<ParametrosEmailSacDTO, ParametrosEmailSac>();
        }
    }
}