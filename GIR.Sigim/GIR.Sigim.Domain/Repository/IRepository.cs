using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain.Entity;
using GIR.Sigim.Domain.Specification;

namespace GIR.Sigim.Domain.Repository
{
    public interface IRepository<TEntity> : IDisposable where TEntity : BaseEntity
    {
        IUnitOfWork UnitOfWork { get; }
        void Inserir(TEntity item);
        void Remover(TEntity item);
        void Alterar(TEntity item);
        TEntity ObterPeloId(int? id, params Expression<Func<TEntity, object>>[] includes);
        TEntity ObterPeloId(int? id, ISpecification<TEntity> specification, params Expression<Func<TEntity, object>>[] includes);
        IEnumerable<TEntity> ListarTodos();
        IEnumerable<TEntity> ListarPeloFiltro(Expression<Func<TEntity, bool>> filtro, params Expression<Func<TEntity, object>>[] includes);
        IEnumerable<TEntity> ListarPeloFiltro(ISpecification<TEntity> specification, params Expression<Func<TEntity, object>>[] includes);
        IEnumerable<TEntity> ListarPeloFiltroComPaginacao<KProperty>(
            ISpecification<TEntity> specification,
            int pageIndex,
            int pageCount,
            Expression<Func<TEntity, KProperty>> orderByExpression,
            bool ascending,
            out int totalRecords,
            params Expression<Func<TEntity, object>>[] includes);

        IEnumerable<TEntity> ListarPeloFiltroComPaginacao(
            ISpecification<TEntity> specification,
            int pageIndex,
            int pageCount,
            string orderBy,
            bool ascending,
            out int totalRecords,
            params Expression<Func<TEntity, object>>[] includes);
    }
}