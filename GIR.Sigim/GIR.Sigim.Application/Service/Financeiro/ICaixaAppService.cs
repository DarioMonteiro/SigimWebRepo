using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Financeiro;
using GIR.Sigim.Application.Filtros.Financeiro;

namespace GIR.Sigim.Application.Service.Financeiro
{
    public interface ICaixaAppService
    {
        List<CaixaDTO> ListarPeloFiltro(CaixaFiltro filtro, out int totalRegistros);
        CaixaDTO ObterPeloId(int? id);
        bool Salvar(CaixaDTO dto);
        bool Deletar(int? id);
        bool EhPermitidoSalvar();
        bool EhPermitidoDeletar();
        //bool EhPermitidoImprimir();
    }
}