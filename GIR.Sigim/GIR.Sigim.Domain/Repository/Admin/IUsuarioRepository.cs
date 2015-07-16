using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Admin;

namespace GIR.Sigim.Domain.Repository.Admin
{
    public interface IUsuarioRepository : IRepository<Usuario>
    {
        Usuario ObterPeloLogin(string login);
        void RemoverFuncionalidade(UsuarioFuncionalidade item);
        void RemoverPerfil(UsuarioPerfil item);
    }
}