using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using CleanArchitecture.Core.UnitOfWork;

namespace CleanArchitecture.Infrastructure.Data.UnitOfWork
{
    public class UnitOfWork<TContext> : IUnitOfWork where TContext : DbContext
    {
        private readonly TContext _dbContext;

        public UnitOfWork(TContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public void Rollback()
        {
            _dbContext
                .ChangeTracker
                .Entries()
                .ToList()
                .ForEach(x => x.Reload());
        }

        public void Dispose()
        {
            _dbContext?.Dispose();
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Database.BeginTransactionAsync(cancellationToken);
        }

        public IDbContextTransaction BeginTransaction()
        {
            return _dbContext.Database.BeginTransaction();
        }
    }
}
