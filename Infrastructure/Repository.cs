using System;
using System.Collections.Generic;
using System.Linq;
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
        public DbContext DbContext { get; }

        public Repository(DbFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public Repository(DbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async void AddAsync(T entity)
        {
            await DbSet.AddAsync(entity);
        }

        public void Delete(T entity)
        {
            DbSet.Remove(entity);
        }
        public void Update(T entity)
        {

            DbSet.Update(entity);
        }
        public IQueryable<T> List(Expression<Func<T, bool>> expression)
        {
            return DbSet.Where(expression);
        }
        public async Task<IList<T>> GetAllAsync()
        {
            return await DbSet.ToListAsync();
        }
    }
}
