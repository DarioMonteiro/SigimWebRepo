using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.Sigim;
using GIR.Sigim.Domain.Repository.Sigim;
using GIR.Sigim.Domain.Specification;

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
                .Where(l => l.Situacao == "A" & l.ClienteContrato == "S")
                .OrderBy(l => l.Nome); 
        }

        public IEnumerable<ClienteFornecedor> Pesquisar(ISpecification<ClienteFornecedor> specification,
                                                        int pageIndex,
                                                        int pageCount,
                                                        string orderBy,
                                                        bool ascending,
                                                        out int totalRecords,
                                                        params Expression<Func<ClienteFornecedor, object>>[] includes)
        {
            var set = CreateSetAsQueryable(includes);
            set = set.Where(specification.SatisfiedBy());
            totalRecords = set.Count();
            set = ConfigurarOrdenacao(set, orderBy, ascending);

            //return set.ToList();
            return set.Skip(pageCount * pageIndex).Take(pageCount);
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

        #region Metodos Privados

        private static IQueryable<ClienteFornecedor> ConfigurarOrdenacao(IQueryable<ClienteFornecedor> set, string orderBy, bool ascending)
        {
            switch (orderBy)
            {
                case "rg":
                    set = ascending ? set.OrderBy(l => l.PessoaFisica.Rg) : set.OrderByDescending(l => l.PessoaFisica.Rg);
                    break;
                case "razaoSocial":
                    set = ascending ? set.OrderBy(l => l.PessoaJuridica.NomeFantasia) : set.OrderByDescending(l => l.PessoaJuridica.NomeFantasia);
                    break;
                case "cnpj":
                    set = ascending ? set.OrderBy(l => l.PessoaJuridica.Cnpj) : set.OrderByDescending(l => l.PessoaJuridica.Cnpj);
                    break;
                case "cpf":
                    set = ascending ? set.OrderBy(l => l.PessoaFisica.Cpf) : set.OrderByDescending(l => l.PessoaFisica.Cpf);
                    break;
                case "nomeFantasia":
                default:
                    set = ascending ? set.OrderBy(l => l.Nome) : set.OrderByDescending(l => l.Nome);
                    break;
            }
            return set;
        }

        #endregion
    }
}