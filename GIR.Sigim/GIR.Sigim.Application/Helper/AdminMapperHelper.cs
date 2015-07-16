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

            Mapper.CreateMap<UsuarioFuncionalidade, UsuarioFuncionalidadeDTO>();
            Mapper.CreateMap<UsuarioFuncionalidadeDTO, UsuarioFuncionalidade>();

            Mapper.CreateMap<UsuarioPerfil, UsuarioPerfilDTO>();
            Mapper.CreateMap<UsuarioPerfilDTO, UsuarioPerfil>();

            Mapper.CreateMap<Modulo, ModuloDTO>();
            Mapper.CreateMap<ModuloDTO, Modulo>();

            Mapper.CreateMap<Perfil, PerfilDTO>();
            Mapper.CreateMap<PerfilDTO, Perfil>();

            Mapper.CreateMap<PerfilFuncionalidade, PerfilFuncionalidadeDTO>();
            Mapper.CreateMap<PerfilFuncionalidadeDTO, PerfilFuncionalidade>();

            
        }
    }
}