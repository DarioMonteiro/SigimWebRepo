using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Contrato;
using GIR.Sigim.Application.Filtros.Contrato;

namespace GIR.Sigim.Application.Service.Contrato
{
    public interface IContratoAppService : IBaseAppService 
    {
        List<ContratoDTO> ListarPeloFiltro(MedicaoContratoFiltro filtro,int? idUsuario, out int totalRegistros);
        ContratoDTO ObterPeloId(int? id, int? idUsuario);
        bool EhContratoAssinado(ContratoDTO dto);
        bool EhContratoExistente(ContratoDTO dto);
        bool EhContratoComCentroCustoAtivo(ContratoDTO dto);
    }
}
