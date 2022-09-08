using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DbFactory _dbFactory;
        private DbSet<T> _dbSet;

        protected DbSet<T> DbSet
        {
            get => _dbSet ?? (_dbSet = _dbFactory.DbContext.Set<T>());
        }

        public Repository(DbFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }
        public async void AddAsync(T entity)
        {
            await DbSet.AddAsync(entity);
        }
        public async void DeleteExp(Expression<Func<T, bool>> expression)
        {
            T entity = await FindAsync(expression);
            DbSet.Remove(entity);
        }
        public void Delete(T entity)
        {
            DbSet.Remove(entity);
        }
        public void Update(T entity)
        {
            DbSet.Update(entity);
        }
        public async Task<T> FindAsync(Expression<Func<T, bool>> expression)
        {
            return await DbSet.FirstOrDefaultAsync(expression);
        }
    }
}
