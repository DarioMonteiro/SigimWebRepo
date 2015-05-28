using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Application.DTO.Sac;

namespace GIR.Sigim.Application.Service.Sac
{
    public interface IParametrosSacAppService
    {
        ParametrosSacDTO Obter();
        void Salvar(ParametrosSacDTO dto);
    }
}