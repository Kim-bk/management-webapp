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
        private DbContext dbContext;

        protected DbSet<T> DbSet
        {
            get => _dbSet ?? (_dbSet = _dbFactory.DbContext.Set<T>());
        }

        public Repository( DbFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public Repository(DbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Add(T entity)
        {
            DbSet.Add(entity);
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
