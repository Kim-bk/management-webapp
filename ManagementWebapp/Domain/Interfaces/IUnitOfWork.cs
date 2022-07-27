using System;
using System.Threading;
using System.Threading.Tasks;

namespace Domain
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<T> Repository<T>() where T : class;

        Task<int> SaveChangesAsync();

        Task BeginTransaction();

        Task CommitTransaction();

        Task RollbackTransaction();
    }
}
