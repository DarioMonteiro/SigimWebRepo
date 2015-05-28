using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Financeiro;
using GIR.Sigim.Application.Filtros.Financeiro;

namespace GIR.Sigim.Application.Service.Financeiro
{
    public interface IMotivoCancelamentoAppService : IBaseAppService 
    {        
        List<MotivoCancelamentoDTO> ListarPeloFiltro(MotivoCancelamentoFiltro filtro, out int totalRegistros);
        MotivoCancelamentoDTO ObterPeloId(int? id);
        bool Salvar(MotivoCancelamentoDTO dto); 
    }
}