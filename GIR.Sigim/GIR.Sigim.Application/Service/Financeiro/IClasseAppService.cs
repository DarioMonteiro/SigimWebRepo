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
        ClasseDTO ObterPeloCodigoEOrcamento(string codigo, int orcamentoId);
        bool EhClasseValida(ClasseDTO Classe, int orcamentoId);
        bool EhClasseUltimoNivelValida(ClasseDTO Classe, int orcamentoId);
        List<TreeNodeDTO> ListarPeloOrcamento(int? orcamentoId);
    }
}