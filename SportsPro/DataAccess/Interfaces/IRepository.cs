using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SportsPro.DataAccess.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> GetAll(string? includeProperties = null);

        TEntity Find(Expression<Func<TEntity, bool>> predicate, string? includeProperties = null);

        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);


        void Delete(TEntity entity);
        void DeleteRange(IEnumerable<TEntity> entities);

    }
}
