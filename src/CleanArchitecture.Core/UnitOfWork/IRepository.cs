using CleanArchitecture.Core.Common;
using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Paging;
using System.Linq.Expressions;

namespace CleanArchitecture.Core.UnitOfWork
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> ListAsync(PagingRequestBase request,
            Expression<Func<TEntity, bool>> filter = null,
            CancellationToken cancellationToken = default,
            params Expression<Func<TEntity, object>>[] includeProperties);

        Task<IEnumerable<TEntity>> ListAsNoTrackingAsync(Expression<Func<TEntity, bool>> filter = null,
            CancellationToken cancellationToken = default,
            params Expression<Func<TEntity, object>>[] includeProperties);

        Task<ICollection<TEntity>> ListAsync_ReturnCollection(PagingRequestBase request,
            Expression<Func<TEntity, bool>> filter = null,
            CancellationToken cancellationToken = default,
            params Expression<Func<TEntity, object>>[] includeProperties);

        IQueryable<TEntity> ListAsync_ReturnIQueryable(PagingRequestBase request,
            Expression<Func<TEntity, bool>> filter,
            params Expression<Func<TEntity, object>>[] includeProperties);

        Task<DataTablePagedResult<TItem>> ListByPagingAsync<TItem>(PagingRequestBase request,
            Expression<Func<TEntity, bool>> filter,
            Expression<Func<TEntity, TItem>> map,
            CancellationToken cancellationToken,
            params Expression<Func<TEntity, object>>[] includeProperties) where TItem : class;

        Task<TEntity> FindByIdAsync(int id);

        Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> filter,
            CancellationToken cancellationToken = default,
            params Expression<Func<TEntity, object>>[] includeProperties);

        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> filter = null,
            CancellationToken cancellationToken = default,
            params Expression<Func<TEntity, object>>[] includeProperties);

        Task<TEntity> GetAsync(Specification<TEntity> specification = null,
            CancellationToken cancellationToken = default,
            params Expression<Func<TEntity, object>>[] includeProperties);

        Task AddAsync(TEntity entity);

        Task AddRangeAsync(IEnumerable<TEntity> entities);

        void Remove(TEntity entity);

        void Remove(int id);

        void RemoveRange(IEnumerable<TEntity> entities);

        void Update(TEntity entity);

        void UpdateRange(IEnumerable<TEntity> entities);

        Task<int> CountAsync(Expression<Func<TEntity, bool>> filter = null, CancellationToken cancellationToken = default);

        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter = null, CancellationToken cancellationToken = default);
    }
}
