using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Financeiro;
using GIR.Sigim.Application.DTO.Sigim;

namespace GIR.Sigim.Application.Service.Financeiro
{
    public interface ICentroCustoAppService
    {
        CentroCustoDTO ObterPeloCodigo(string codigo);
        bool EhCentroCustoValido(CentroCustoDTO centroCusto);
        bool EhCentroCustoUltimoNivelValido(CentroCustoDTO centroCusto);
        bool UsuarioPossuiAcessoCentroCusto(CentroCustoDTO centroCusto, int? idUsuario, string modulo);
        List<TreeNodeDTO> ListarRaizesAtivas();
        byte[] ObterIconeRelatorioPeloCentroCusto(string codigo);
    }
}