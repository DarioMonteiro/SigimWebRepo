using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.OrdemCompra;
using GIR.Sigim.Application.Filtros.OrdemCompras;

namespace GIR.Sigim.Application.Service.OrdemCompra
{
    public interface IPreRequisicaoMaterialAppService : IBaseAppService
    {
        List<PreRequisicaoMaterialDTO> ListarPeloFiltro(PreRequisicaoMaterialFiltro filtro, int? idUsuario, out int totalRegistros);
        PreRequisicaoMaterialDTO ObterPeloId(int? id, int? idUsuario);
        bool Salvar(PreRequisicaoMaterialDTO dto);
        bool EhPermitidoSalvar(PreRequisicaoMaterialDTO dto);
        bool EhPermitidoCancelar(PreRequisicaoMaterialDTO dto);
        bool EhPermitidoAdicionarItem(PreRequisicaoMaterialDTO dto);
        bool EhPermitidoCancelarItem(PreRequisicaoMaterialDTO dto);
        bool EhPermitidoEditarItem(PreRequisicaoMaterialDTO dto);
    }
}