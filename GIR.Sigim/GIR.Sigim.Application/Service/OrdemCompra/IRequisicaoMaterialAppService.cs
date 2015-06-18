using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.OrdemCompra;
using GIR.Sigim.Application.DTO.Sigim;
using GIR.Sigim.Application.Filtros.OrdemCompras;

namespace GIR.Sigim.Application.Service.OrdemCompra
{
    public interface IRequisicaoMaterialAppService : IBaseAppService
    {
        List<RequisicaoMaterialDTO> ListarPeloFiltro(RequisicaoMaterialFiltro filtro, int? idUsuario, out int totalRegistros);
        RequisicaoMaterialDTO ObterPeloId(int? id);
        bool Salvar(RequisicaoMaterialDTO dto);
        bool Aprovar(int? id);
        bool CancelarAprovacao(int? id);
        bool CancelarRequisicao(int? id, string motivo);
        FileDownloadDTO Exportar(int? id, FormatoExportacaoArquivo formato);
        bool EhPermitidoSalvar(RequisicaoMaterialDTO dto);
        bool EhPermitidoCancelar(RequisicaoMaterialDTO dto);
        bool EhPermitidoImprimir(RequisicaoMaterialDTO dto);
        bool EhPermitidoAdicionarItem(RequisicaoMaterialDTO dto);
        bool EhPermitidoCancelarItem(RequisicaoMaterialDTO dto);
        bool EhPermitidoEditarItem(RequisicaoMaterialDTO dto);
        bool EhPermitidoAprovarRequisicao(RequisicaoMaterialDTO dto);
        bool EhPermitidoCancelarAprovacao(RequisicaoMaterialDTO dto);
        bool EhPermitidoEditarCentroCusto(RequisicaoMaterialDTO dto);
    }
}