using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Comercial;

namespace GIR.Sigim.Application.Service.Comercial
{
    public interface IBlocoAppService
    {
        List<BlocoDTO> ListarPeloEmpreendimento(int empreendimentoId);
    }
}