using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Financeiro;

namespace GIR.Sigim.Application.Service.Financeiro
{
    public interface ITipoDocumentoAppService
    {
        List<TipoDocumentoDTO> ListarTodos();
    }
}
