using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIR.Sigim.Domain;
using GIR.Sigim.Domain.Entity;

namespace GIR.Sigim.Infrastructure.Data
{
    public interface IQueryableUnitOfWork : IUnitOfWork
    {
        DbSet<TEntity> CreateSet<TEntity>() where TEntity : BaseEntity;
        void Attach<TEntity>(TEntity item) where TEntity : BaseEntity;
        void SetModified<TEntity>(TEntity item) where TEntity : BaseEntity;
        void ApplyCurrentValues<TEntity>(TEntity original, TEntity current) where TEntity : BaseEntity;
        IEnumerable<TEntity> ExecWithStoreProcedure<TEntity>(string query, params object[] parameters) where TEntity : BaseEntity;
    }
}