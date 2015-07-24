using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.OrdemCompra;

namespace GIR.Sigim.Application.Service.OrdemCompra
{
    public interface IParametrosOrdemCompraAppService
    {
        ParametrosOrdemCompraDTO Obter();
        void Salvar(ParametrosOrdemCompraDTO dto);
        bool EhPermitidoSalvar();
    }
}