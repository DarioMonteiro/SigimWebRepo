using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.GirCliente;
using GIR.Sigim.Application.DTO.Sigim;


namespace GIR.Sigim.Application.Service.Sigim
{
    public interface IAcessoAppService
    {
        bool ValidaAcessoAoModulo(string nomeModulo, InformacaoConfiguracaoDTO informacaoConfiguracao);
        bool ValidaAcessoGirCliente(string nomeModulo, int usuarioId, InformacaoConfiguracaoDTO informacaoConfiguracao);
    }
}
