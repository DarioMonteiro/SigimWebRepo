using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Sigim;
using GIR.Sigim.Domain.Repository.Sigim;

namespace GIR.Sigim.Infrastructure.Data.Repository.Sigim
{
    public class ClienteFornecedorRepository : Repository<ClienteFornecedor>, IClienteFornecedorRepository
    {
        #region Constructor

        public ClienteFornecedorRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }

        #endregion

        #region IClienteFornecedorRepository Members

        public IEnumerable<ClienteFornecedor> ListarAtivos()
        {
            return QueryableUnitOfWork.CreateSet<ClienteFornecedor>()
                .Where(l => l.Situacao == "A")
                .OrderBy(l => l.Nome);
        }

        #endregion
    }
}