using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Financeiro;

namespace GIR.Sigim.Application.Service.Financeiro
{
    public interface IParametrosUsuarioFinanceiroAppService : IBaseAppService
    {
        ParametrosUsuarioFinanceiroDTO ObterPeloIdUsuario(int? idUsuario);
        void Salvar(ParametrosUsuarioFinanceiroDTO dto);
    }
}
