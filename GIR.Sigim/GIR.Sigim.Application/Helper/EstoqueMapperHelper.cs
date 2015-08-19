using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GIR.Sigim.Application.DTO.Estoque;
using GIR.Sigim.Domain.Entity.Estoque;

namespace GIR.Sigim.Application.Helper
{
    public class EstoqueMapperHelper
    {
        public static void Initialise()
        {
            Mapper.CreateMap<Movimento, MovimentoDTO>();
            Mapper.CreateMap<MovimentoDTO, Movimento>();
        }
    }
}