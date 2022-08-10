using System;
using API.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class DbFactory : IDisposable
    {
        private bool _disposed;
        private Func<API.Context.AppDbContext> _instanceFunc;
        private Microsoft.EntityFrameworkCore.DbContext _dbContext;
        public Microsoft.EntityFrameworkCore.DbContext DbContext => _dbContext ?? (_dbContext = _instanceFunc.Invoke());

        public DbFactory(Func<API.Context.AppDbContext> dbContextFactory)
        {
            _instanceFunc = dbContextFactory;
        }

        public void Dispose()
        {
            if (!_disposed && _dbContext != null)
            {
                _disposed = true;
                _dbContext.Dispose();
            }
        }
    }
}

