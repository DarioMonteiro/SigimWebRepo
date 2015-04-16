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

        public IEnumerable<ClienteFornecedor> ListarAtivosDeContrato()
        {
            return QueryableUnitOfWork.CreateSet<ClienteFornecedor>()
                .Where(l => l.ClienteContrato == "S")
                .OrderBy(l => l.Nome); 
        }

        //public IEnumerable<ClienteFornecedor> ListarClienteFornecedor(ClassificacaoClienteFornecedor classificacaoClienteFornecedor, SituacaoClienteFornecedor situacaoClienteFornecedor, TipoPessoa tipoPessoa)
        //{

        //    var set = CreateSetAsQueryable();

        //    switch (classificacaoClienteFornecedor)
        //    {
        //        case ClassificacaoClienteFornecedor.APagar :
        //            set = set.Where(l => l.ClienteAPagar == "S");
        //            break;
        //        case ClassificacaoClienteFornecedor.aReceber :
        //            set = set.Where(l => l.ClienteAReceber == "S");
        //            break;
        //        case ClassificacaoClienteFornecedor.OrdemCompra :
        //            set = set.Where(l => l.ClienteOrdemCompra == "S");
        //            break;
        //        case ClassificacaoClienteFornecedor.Contrato :
        //            set = set.Where(l => l.ClienteContrato == "S");
        //            break;
        //        case ClassificacaoClienteFornecedor.Aluguel :
        //            set = set.Where(l => l.ClienteAluguel == "S");
        //            break;
        //        case ClassificacaoClienteFornecedor.Empreitada :
        //            set = set.Where(l => l.ClienteEmpreitada == "S");
        //            break;
        //    }

        //    switch (situacaoClienteFornecedor) 
        //    {
        //        case SituacaoClienteFornecedor.Ativo :
        //            set = set.Where(l => l.Situacao == "A");
        //            break;
        //        case SituacaoClienteFornecedor.Inativo :
        //            set = set.Where(l => l.Situacao == "I");
        //            break;
        //    }

        //    switch (tipoPessoa)
        //    { 
        //        case TipoPessoa.Fisica :
        //            set = set.Where(l => l.TipoPessoa == "F");
        //            break;
        //        case TipoPessoa.Juridica :
        //            set = set.Where(l => l.TipoPessoa == "J");
        //            break;
        //    }

        //    set = set.OrderBy(l => l.Nome);

        //    return set;

        //}

        #endregion
    }
}