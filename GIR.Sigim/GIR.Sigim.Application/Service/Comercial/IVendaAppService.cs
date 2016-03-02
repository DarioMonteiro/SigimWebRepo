using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Comercial;
using GIR.Sigim.Application.Filtros;
using GIR.Sigim.Application.Filtros.Comercial;
using GIR.Sigim.Application.DTO.Sigim;

namespace GIR.Sigim.Application.Service.Comercial
{
    public interface IVendaAppService
    {
        List<RelStatusVendaDTO> ListarPeloFiltroRelStatusVenda(RelStatusVendaFiltro filtro, out int totalRegistros);
        bool EhPermitidoImprimirRelStatusVenda();
    }
}