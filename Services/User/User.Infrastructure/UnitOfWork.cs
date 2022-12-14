using System;
using System.Data;
using System.Threading.Tasks;
using Domain.Interfaces;
using Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;
        private IDbContextTransaction _transaction;
        private readonly IsolationLevel? _isolationLevel;

        public bool HasActiveTransaction => _transaction != null;

        public UnitOfWork(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
      
        private async Task StartNewTransactionIfNeeded()
        {
            if (_transaction == null)
            {
                _transaction = _isolationLevel.HasValue ?
                    await _dbContext.Database.BeginTransactionAsync(_isolationLevel.GetValueOrDefault()) : await _dbContext.Database.BeginTransactionAsync();
            }
        }

        public async Task BeginTransaction()
        {
            await StartNewTransactionIfNeeded();
        }
       
        public async Task CommitTransaction()
        {
            // Dispatch Domain Events collection. 
            // Choices:
            // A) Right BEFORE committing data (EF SaveChanges) into the DB will make a single transaction including  
            // side effects from the domain event handlers which are using the same _dbContext with "InstancePerLifetimeScope" or "scoped" lifetime
            // B) Right AFTER committing data (EF SaveChanges) into the DB will make multiple transactions. 
            // You will need to handle eventual consistency and compensatory actions in case of failures in any of the Handlers. 

            // Save history before save changes
            _dbContext.OnBeforeSaveChanges();
            await _dbContext.SaveChangesAsync();
         //  await _dbContext.SaveEntitiesAsync();


            if (_transaction == null) return;

            await _transaction.CommitAsync();

            await _transaction.DisposeAsync();
            _transaction = null;
        }
        public async Task RollbackTransaction()
        {
            if (_transaction == null) return;

            await _transaction.RollbackAsync();

            await _transaction.DisposeAsync();
            _transaction = null;
        }

        public void Dispose()
        {
            if (_dbContext == null)
                return;

            _dbContext.Dispose();
        }
    }
}