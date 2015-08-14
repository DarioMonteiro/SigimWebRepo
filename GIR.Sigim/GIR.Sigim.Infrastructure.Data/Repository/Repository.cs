using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain;
using GIR.Sigim.Domain.Entity;
using GIR.Sigim.Domain.Repository;
using GIR.Sigim.Domain.Specification;

namespace GIR.Sigim.Infrastructure.Data.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        private IQueryableUnitOfWork unitOfWork;

        #region Constructor

        public Repository(IQueryableUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
                throw new ArgumentNullException("unitOfWork");

            this.unitOfWork = unitOfWork;
        }

        #endregion

        protected IQueryableUnitOfWork QueryableUnitOfWork
        {
            get { return unitOfWork; }
        }

        #region IRepository<TEntity> Members

        public IUnitOfWork UnitOfWork
        {
            get { return unitOfWork; }
        }

        public void Inserir(TEntity item)
        {
            unitOfWork.CreateSet<TEntity>().Add(item);
        }

        public void Remover(TEntity item)
        {
            if (item != (TEntity)null)
            {
                unitOfWork.Attach(item);
                unitOfWork.CreateSet<TEntity>().Remove(item);
            }
        }

        public virtual void Alterar(TEntity item)
        {
            unitOfWork.SetModified<TEntity>(item);
        }

        protected IQueryable<TEntity> CreateSetAsQueryable(params Expression<Func<TEntity, object>>[] includes)
        {
            var set = unitOfWork.CreateSet<TEntity>().AsQueryable<TEntity>();

            if (includes.Any())
                set = includes.Aggregate(set, (current, expression) => current.Include(expression).DefaultIfEmpty());

            return set;
        }

        public TEntity ObterPeloId(int? id, params Expression<Func<TEntity, object>>[] includes)
        {
            var set = CreateSetAsQueryable(includes);

            return set.Where(l => l.Id == id).SingleOrDefault();
        }

        public TEntity ObterPeloId(int? id, ISpecification<TEntity> specification, params Expression<Func<TEntity, object>>[] includes)
        {
            var set = CreateSetAsQueryable(includes);

            set = set.Where(l => l.Id == id);

            if (specification != null)
                set = set.Where(specification.SatisfiedBy());

            return set.SingleOrDefault();
        }

        public IEnumerable<TEntity> ListarTodos()
        {
            return unitOfWork.CreateSet<TEntity>();
        }

        public IEnumerable<TEntity> ListarTodos(params Expression<Func<TEntity, object>>[] includes)
        {
            var set = CreateSetAsQueryable(includes);
            return set;
        }

        public IEnumerable<TEntity> ListarPeloFiltro(Expression<Func<TEntity, bool>> filtro, params Expression<Func<TEntity, object>>[] includes)
        {
            var set = CreateSetAsQueryable(includes);

            return set.Where(filtro);
        }

        public IEnumerable<TEntity> ListarPeloFiltro(ISpecification<TEntity> specification, params Expression<Func<TEntity, object>>[] includes)
        {
            var set = CreateSetAsQueryable(includes);

            return set.Where(specification.SatisfiedBy());
        }

        public IEnumerable<TEntity> ListarPeloFiltroComPaginacao<KProperty>(
            ISpecification<TEntity> specification,
            int pageIndex,
            int pageCount,
            Expression<Func<TEntity, KProperty>> orderByExpression,
            bool ascending,
            out int totalRecords,
            params Expression<Func<TEntity, object>>[] includes)
        {
            var set = CreateSetAsQueryable(includes);

            set = set.Where(specification.SatisfiedBy());

            totalRecords = set.Count();

            if (ascending)
            {
                return set.OrderBy(orderByExpression)
                          .Skip(pageCount * pageIndex)
                          .Take(pageCount);
            }
            else
            {
                return set.OrderByDescending(orderByExpression)
                          .Skip(pageCount * pageIndex)
                          .Take(pageCount);
            }
        }

        public virtual IEnumerable<TEntity> ListarPeloFiltroComPaginacao(ISpecification<TEntity> specification, int pageIndex, int pageCount, string orderBy, bool ascending, out int totalRecords, params Expression<Func<TEntity, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<TEntity> ListarPeloFiltroComPaginacao(Expression<Func<TEntity, bool>> filtro, int pageIndex, int pageCount, string orderBy, bool ascending, out int totalRecords, params Expression<Func<TEntity, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            if (unitOfWork != null)
                unitOfWork.Dispose();
        }

        #endregion
    }
}