using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GIR.Sigim.Application.DTO.Admin;
using GIR.Sigim.Domain.Entity.Admin;

namespace GIR.Sigim.Application.Helper
{
    public class AdminMapperHelper
    {
        public static void Initialise()
        {
            Mapper.CreateMap<Usuario, UsuarioDTO>();
            Mapper.CreateMap<UsuarioDTO, Usuario>();
        }
    }
}