using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Financeiro;
using GIR.Sigim.Application.DTO.Sigim;

namespace GIR.Sigim.Application.Service.Financeiro
{
    public interface IClasseAppService
    {
        ClasseDTO ObterPeloCodigo(string codigo);
        bool EhClasseValida(ClasseDTO Classe);
        bool EhClasseUltimoNivelValida(ClasseDTO Classe);
        List<TreeNodeDTO> ListarRaizes();
    }
}