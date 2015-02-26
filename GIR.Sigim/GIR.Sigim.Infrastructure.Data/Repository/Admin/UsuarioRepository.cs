using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Admin;
using GIR.Sigim.Domain.Repository.Admin;

namespace GIR.Sigim.Infrastructure.Data.Repository.Admin
{
    public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
    {
        #region Constructor

        public UsuarioRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }

        #endregion

        #region IUsuarioRepository Members

        public Usuario ObterPeloLogin(string login)
        {
            return QueryableUnitOfWork.CreateSet<Usuario>().Where(l => l.Login == login).SingleOrDefault();
        }

        #endregion
    }
}