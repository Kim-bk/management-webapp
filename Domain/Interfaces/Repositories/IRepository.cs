using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Domain
{
    public interface IRepository<T> where T : class
    {
        Task<T> FindAsync(Expression<Func<T, bool>> expression);
        void AddAsync(T entity);
        void DeleteExp(Expression<Func<T, bool>> expression);
        void Update(T entity);
        void Delete(T entity);
    }
}
