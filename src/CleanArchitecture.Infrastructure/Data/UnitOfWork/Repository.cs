using CleanArchitecture.Core.UnitOfWork;
using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Paging;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
using CleanArchitecture.Core.Common;

namespace CleanArchitecture.Infrastructure.Data.UnitOfWork
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected DbContext _dbContext { get; }
        protected DbSet<TEntity> _entities { get; }

        protected Repository(DbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentException(nameof(dbContext));
            _entities = dbContext.Set<TEntity>();
        }

        public Repository(AppDbContext dbContext) : this((DbContext)dbContext) { }

        public virtual async Task<DataTablePagedResult<TItem>> ListByPagingAsync<TItem>(PagingRequestBase paging,
        Expression<Func<TEntity, bool>> filter,
        Expression<Func<TEntity, TItem>> map,
        CancellationToken cancellationToken,
        params Expression<Func<TEntity, object>>[] includeProperties) where TItem : class
        {
            var query = _entities.AsQueryable();
            var pageIndex = paging?.PageIndex ?? 0;
            var pageSize = paging?.PageSize ?? 10;

            if (includeProperties != null)
            {
                query = includeProperties.Aggregate(query, (current, include) => current.Include(include));
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            var totalRecords = await query.CountAsync();

            if (paging != null)
            {
                if (!string.IsNullOrEmpty(paging.OrderCol) && !string.IsNullOrEmpty(paging.OrderDir))
                {
                    query = query.OrderBy(paging.OrderCol + " " + paging.OrderDir);
                }

                if (paging.PageIndex != null)
                {
                    query = query.Skip((paging.PageIndex.Value - 1) * paging.PageSize)
                                .Take(paging.PageSize);
                }
            }

            var items = await query.Select(map).ToListAsync(cancellationToken);

            var response = new DataTablePagedResult<TItem>(items, pageIndex, pageSize, totalRecords);
            response.Items ??= Enumerable.Empty<TItem>();

            return response;
        }

        public virtual async Task<IEnumerable<TEntity>> ListAsync(PagingRequestBase paging,
            Expression<Func<TEntity, bool>> filter = null,
            CancellationToken cancellationToken = default,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = _entities.AsQueryable();
            var pageIndex = paging?.PageIndex ?? 0;
            var pageSize = paging?.PageSize ?? 10;

            if (includeProperties != null)
            {
                query = includeProperties.Aggregate(query, (current, include) => current.Include(include));
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (paging != null)
            {
                if (!string.IsNullOrEmpty(paging.OrderCol) && !string.IsNullOrEmpty(paging.OrderDir))
                {
                    query = query.OrderBy(paging.OrderCol + " " + paging.OrderDir);
                }

                if (paging.PageIndex != null)
                {
                    query = query.Skip((paging.PageIndex.Value - 1) * paging.PageSize)
                                .Take(paging.PageSize);
                }
            }

            return await query.ToListAsync(cancellationToken);
        }

        public virtual async Task<ICollection<TEntity>> ListAsync_ReturnCollection(PagingRequestBase paging = null,
            Expression<Func<TEntity, bool>> filter = null,
            CancellationToken cancellationToken = default,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = _entities.AsQueryable();

            var pageIndex = paging?.PageIndex ?? 0;
            var pageSize = paging?.PageSize ?? 10;

            if (includeProperties != null)
            {
                query = includeProperties.Aggregate(query, (current, include) => current.Include(include));
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (paging != null)
            {
                if (!string.IsNullOrEmpty(paging.OrderCol) && !string.IsNullOrEmpty(paging.OrderDir))
                {
                    query = query.OrderBy(paging.OrderCol + " " + paging.OrderDir);
                }

                if (paging.PageIndex != null)
                {
                    query = query.Skip((paging.PageIndex.Value - 1) * paging.PageSize)
                                .Take(paging.PageSize);
                }
            }

            return await query.ToListAsync(cancellationToken);
        }

        public virtual IQueryable<TEntity> ListAsync_ReturnIQueryable(PagingRequestBase paging = null,
            Expression<Func<TEntity, bool>> filter = null,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = _entities.AsQueryable();

            var pageIndex = paging?.PageIndex ?? 0;
            var pageSize = paging?.PageSize ?? 10;

            if (includeProperties != null)
            {
                query = includeProperties.Aggregate(query, (current, include) => current.Include(include));
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (paging != null)
            {
                if (!string.IsNullOrEmpty(paging.OrderCol) && !string.IsNullOrEmpty(paging.OrderDir))
                {
                    query = query.OrderBy(paging.OrderCol + " " + paging.OrderDir);
                }

                if (paging.PageIndex != null)
                {
                    query = query.Skip((paging.PageIndex.Value - 1) * paging.PageSize)
                                .Take(paging.PageSize);
                }
            }

            return query;
        }

        public virtual async Task<TEntity> FindByIdAsync(int id)
        {
            return await _entities.FindAsync(id);
        }

        public virtual async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> filter,
            CancellationToken cancellationToken = default,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = _entities.AsQueryable();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                query = includeProperties.Aggregate(query, (current, include) => current.Include(include));
            }

            return await query.FirstOrDefaultAsync(filter, cancellationToken);
        }

        public virtual async Task<TEntity> GetAsync(
            Expression<Func<TEntity, bool>> filter = null,
            CancellationToken cancellationToken = default,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = _entities.AsQueryable();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                query = includeProperties.Aggregate(query, (current, include) => current.Include(include));
            }

            return await query.FirstOrDefaultAsync(filter, cancellationToken);
        }

        public virtual async Task<TEntity> GetAsync(
            Specification<TEntity> specification = null,
            CancellationToken cancellationToken = default,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = _entities.AsQueryable();

            if (specification != null)
            {
                query = query.Where(specification.ToExpression());
            }

            if (includeProperties != null)
            {
                query = includeProperties.Aggregate(query, (current, include) => current.Include(include));
            }

            return await query.FirstOrDefaultAsync(specification.ToExpression(), cancellationToken);
        }

        public virtual async Task<IEnumerable<TEntity>> ListAsNoTrackingAsync(Expression<Func<TEntity, bool>> filter = null,
            CancellationToken cancellationToken = default,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = _entities.AsNoTracking();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                query = includeProperties.Aggregate(query, (current, include) => current.Include(include));
            }

            return await query.ToListAsync(cancellationToken);
        }

        public virtual async Task AddAsync(TEntity entity)
        {
            await _entities.AddAsync(entity);
        }

        public virtual async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await _entities.AddRangeAsync(entities);
        }

        public virtual void Remove(TEntity entity)
        {
            _entities.Remove(entity);
        }

        public virtual void Remove(int id)
        {
            var entity = _entities.Find(id);
            if (entity is not null)
            {
                _entities.Remove(entity);
            }
        }

        public virtual void RemoveRange(IEnumerable<TEntity> entities)
        {
            _entities.RemoveRange(entities);
        }

        public virtual void Update(TEntity entity)
        {
            _entities.Update(entity);
        }

        public virtual void UpdateRange(IEnumerable<TEntity> entities)
        {
            _entities.UpdateRange(entities);
        }

        public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>> filter = null, CancellationToken cancellationToken = default)
        {
            if (filter == null)
            {
                return await _entities.CountAsync(cancellationToken);
            }

            return await _entities.CountAsync(filter, cancellationToken);
        }

        public virtual async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter = null, CancellationToken cancellationToken = default)
        {
            if (filter == null)
            {
                return await _entities.AnyAsync(cancellationToken);
            }

            return await _entities.AnyAsync(filter, cancellationToken);
        }
    }
}
