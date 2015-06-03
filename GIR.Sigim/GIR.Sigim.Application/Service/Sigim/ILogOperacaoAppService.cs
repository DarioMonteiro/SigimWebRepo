using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Sigim;

namespace GIR.Sigim.Application.Service.Sigim
{
    public interface ILogOperacaoAppService
    {
        void Gravar(string descricao, string nomeRotina, string nomeTabela, string nomeComando, string dados);
    }
}