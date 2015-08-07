using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions ;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Sigim;
using GIR.Sigim.Domain.Specification;

namespace GIR.Sigim.Domain.Repository.Sigim
{
    public interface IClienteFornecedorRepository : IRepository<ClienteFornecedor>
    {
        IEnumerable<ClienteFornecedor> ListarAtivos();
        IEnumerable<ClienteFornecedor> ListarAtivosDeContrato();
        IEnumerable<ClienteFornecedor> Pesquisar(ISpecification<ClienteFornecedor> specification, int pageIndex, int pageCount, string orderBy, bool ascending, out int totalRecords, params Expression<Func<ClienteFornecedor, object>>[] includes);
        //IEnumerable<ClienteFornecedor> ListarClienteFornecedor(ClassificacaoClienteFornecedor classificacaoClienteFornecedor, SituacaoClienteFornecedor situacaoClienteFornecedor, TipoPessoa tipoPessoa);
    }
}