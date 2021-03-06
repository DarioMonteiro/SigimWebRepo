﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity.OrdemCompra;
using GIR.Sigim.Domain.Repository.OrdemCompra;
using GIR.Sigim.Domain.Specification;
using GIR.Sigim.Domain.Specification.OrdemCompra;

namespace GIR.Sigim.Infrastructure.Data.Repository.OrdemCompra
{
    public class OrdemCompraRepository : Repository<Domain.Entity.OrdemCompra.OrdemCompra>, IOrdemCompraRepository
    {
        #region Contructor

        public OrdemCompraRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {

        }

        #endregion

        #region IRepository<OrdemCompra> Métodos

        public IEnumerable<Domain.Entity.OrdemCompra.OrdemCompra> Pesquisar(
            ISpecification<Domain.Entity.OrdemCompra.OrdemCompra> specification,
            int pageIndex,
            int pageCount,
            string orderBy,
            bool ascending,
            out int totalRecords,
            params Expression<Func<Domain.Entity.OrdemCompra.OrdemCompra, object>>[] includes)
        {
            var set = CreateSetAsQueryable(includes);
            set = set.Where(specification.SatisfiedBy());
            totalRecords = set.Count();
            set = ConfigurarOrdenacao(set, orderBy, ascending);

            return set.Skip(pageCount * pageIndex).Take(pageCount);
        }

        public IEnumerable<OrdemCompraItem> ListarItensPeloId(int?[] itensId, params Expression<Func<OrdemCompraItem, object>>[] includes)
        {
            var set = QueryableUnitOfWork.CreateSet<OrdemCompraItem>().AsQueryable<OrdemCompraItem>();

            if (includes.Any())
                set = includes.Aggregate(set, (current, expression) => current.Include(expression).DefaultIfEmpty());

            set = set.Where(l => itensId.Contains(l.Id));
            return set.ToList();
        }

        public IEnumerable<OrdemCompraFormaPagamento> ListarFormasPagamentoPeloId(int?[] formasPagamentoId, params Expression<Func<OrdemCompraFormaPagamento, object>>[] includes)
        {
            var set = QueryableUnitOfWork.CreateSet<OrdemCompraFormaPagamento>().AsQueryable<OrdemCompraFormaPagamento>();

            if (includes.Any())
                set = includes.Aggregate(set, (current, expression) => current.Include(expression).DefaultIfEmpty());

            set = set.Where(l => formasPagamentoId.Contains(l.Id));
            return set.ToList();
        }

        #endregion

        #region Métodos Privados

        private static IQueryable<Domain.Entity.OrdemCompra.OrdemCompra> ConfigurarOrdenacao(IQueryable<Domain.Entity.OrdemCompra.OrdemCompra> set, string orderBy, bool ascending)
        {
            switch (orderBy)
            {
                case "centroCusto":
                    set = ascending ? set.OrderBy(l => l.CodigoCentroCusto) : set.OrderByDescending(l => l.CodigoCentroCusto);
                    break;
                case "fornecedor":
                    set = ascending ? set.OrderBy(l => l.ClienteFornecedor.Nome) : set.OrderByDescending(l => l.ClienteFornecedor.Nome);
                    break;
                case "dataOrdemCompra":
                    set = ascending ? set.OrderBy(l => l.Data) : set.OrderByDescending(l => l.Data);
                    break;
                case "id":
                default:
                    set = ascending ? set.OrderBy(l => l.Id) : set.OrderByDescending(l => l.Id);
                    break;
            }
            return set;
        }

        #endregion
    }
}