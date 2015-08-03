using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Sigim;

namespace GIR.Sigim.Application.Service.Sigim
{
    public interface IMaterialClasseInsumoAppService
    {
        MaterialClasseInsumoDTO ObterPeloCodigo(string codigo);
        bool EhClasseInsumoValida(MaterialClasseInsumoDTO classeInsumo);
        bool EhClasseInsumoUltimoNivelValida(MaterialClasseInsumoDTO classeInsumo);
        List<TreeNodeDTO> ListarRaizes();
    }
}
