using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Sigim;

namespace GIR.Sigim.Domain.Repository.Sigim
{
    public interface IClienteFornecedorRepository : IRepository<ClienteFornecedor>
    {
        IEnumerable<ClienteFornecedor> ListarAtivos();
        IEnumerable<ClienteFornecedor> ListarAtivosDeContrato();
        //IEnumerable<ClienteFornecedor> ListarClienteFornecedor(ClassificacaoClienteFornecedor classificacaoClienteFornecedor, SituacaoClienteFornecedor situacaoClienteFornecedor, TipoPessoa tipoPessoa);
    }
}