using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Admin;
using GIR.Sigim.Application.Filtros;
using GIR.Sigim.Application.Filtros.Admin;
using GIR.Sigim.Application.DTO.Sigim;

namespace GIR.Sigim.Application.Service.Admin
{
    public interface IFuncionalidadeAppService
    {
        List<FuncionalidadeDTO> ListarPeloModulo(int moduloId);
    }
}