using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Domain
{
    public interface IRepository<T> where T : class
    {
        void AddAsync(T entity);
        void Delete(T entity);
        IQueryable<T> List(Expression<Func<T, bool>> expression);
        void Update(T entity);
        Task<IList<T>> GetAllAsync();
    }
}
