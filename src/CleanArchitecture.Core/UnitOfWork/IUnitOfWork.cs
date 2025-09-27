using Microsoft.EntityFrameworkCore.Storage;

namespace CleanArchitecture.Core.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        void Rollback();
        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
        IDbContextTransaction BeginTransaction();
    }
}
