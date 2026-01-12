using CleanArchitecture.Core.Domain.Entities.MusicSheetAggregate;
using CleanArchitecture.Core.Domain.Models.MusicSheet;
using CleanArchitecture.Core.Repositories;
using CleanArchitecture.Infrastructure.Data.UnitOfWork;
using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Paging;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Data.Repositories
{
    public class MusicSheetRepository : Repository<MusicSheet>, IMusicSheetRepository
    {
        private readonly AppDbContext _context;

        public MusicSheetRepository(AppDbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        public async Task<DataTablePagedResult<MusicSheetResponse>> ListByPagingAsync(MusicSheetPagingRequest request, Guid? userId, CancellationToken cancellationToken)
        {
            var musicSheetQuery = _context.MusicSheets.AsNoTracking().AsQueryable();
            var userQuery = _context.ApplicatioUsers.AsNoTracking().AsQueryable();

            var query = from ms in musicSheetQuery
                        join u in userQuery on ms.UserId equals u.Id into userGroup
                        from u in userGroup.DefaultIfEmpty()
                        select new { ms, u };

            if (!string.IsNullOrEmpty(request.TextSearch))
            {
                query = query.Where(x => x.ms.Title.Value.Contains(request.TextSearch));
            }

            var totalRecords = await query.CountAsync(cancellationToken);

            // Apply sorting
            if (!string.IsNullOrEmpty(request.OrderCol) && !string.IsNullOrEmpty(request.OrderDir))
            {
                var orderCol = request.OrderCol;
                if (orderCol.Equals("Id", StringComparison.OrdinalIgnoreCase) || 
                    orderCol.Equals("Title", StringComparison.OrdinalIgnoreCase) ||
                    orderCol.Equals("ModifiedDate", StringComparison.OrdinalIgnoreCase))
                {
                    orderCol = "ms." + orderCol;
                }
                query = System.Linq.Dynamic.Core.DynamicQueryableExtensions.OrderBy(query, orderCol + " " + request.OrderDir);
            }

            query = query.OrderByDescending(x => x.ms.LikeCount).ThenByDescending(x => x.ms.ModifiedDate);

            // Apply paging
            var pageIndex = request.PageIndex ?? 1;
            var pageSize = request.PageSize;
            var items = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new MusicSheetResponse
                {
                    Id = x.ms.Id,
                    Title = x.ms.Title.Value,
                    ParentId = x.ms.ParentId,
                    Description = x.ms.Description,
                    TranscriptionId = x.ms.TranscriptionId,
                    Thumbnail = x.ms.Thumbnail,
                    Status = x.ms.Status,
                    MusicSheetVisibility = x.ms.MusicSheetVisibility,
                    ViewCount = x.ms.ViewCount,
                    LikeCount = x.ms.LikeCount,
                    CommentCount = x.ms.CommentCount,
                    ShareCount = x.ms.ShareCount,
                    IsForked = x.ms.IsForked,
                    CreatedDate = x.ms.CreatedDate,
                    ModifiedDate = x.ms.ModifiedDate,
                    UploaderName = x.u != null ? (x.u.FirstName + " " + x.u.LastName).Trim() : string.Empty,
                    UploaderAvatar = x.u != null ? x.u.Avatar : string.Empty,
                    MusicSheetUIState = new MusicSheetUIState
                    {
                        IsLiked = userId.HasValue && x.ms.Likes.Any(l => l.UserId == userId.Value)
                    }
                })
                .ToListAsync(cancellationToken);

            return new DataTablePagedResult<MusicSheetResponse>(items, pageIndex, pageSize, totalRecords);
        }

        public async Task<MusicSheetResponse?> GetDetailByIdAsync(int id, Guid? userId)
        {
            var musicSheetQuery = _context.MusicSheets.AsNoTracking().AsQueryable();
            var userQuery = _context.ApplicatioUsers.AsNoTracking().AsQueryable();

            var query = from ms in musicSheetQuery
                        join u in userQuery on ms.UserId equals u.Id into userGroup
                        from u in userGroup.DefaultIfEmpty()
                        where ms.Id == id
                        select new MusicSheetResponse
                        {
                            Id = ms.Id,
                            Title = ms.Title.Value,
                            ParentId = ms.ParentId,
                            Description = ms.Description,
                            TranscriptionId = ms.TranscriptionId,
                            Thumbnail = ms.Thumbnail,
                            Status = ms.Status,
                            MusicSheetVisibility = ms.MusicSheetVisibility,
                            MidiData = ms.MidiData,
                            ViewCount = ms.ViewCount,
                            LikeCount = ms.LikeCount,
                            CommentCount = ms.CommentCount,
                            ShareCount = ms.ShareCount,
                            IsForked = ms.IsForked,
                            Comments = ms.Comments,
                            Tags = ms.Tags,
                            CreatedDate = ms.CreatedDate,
                            ModifiedDate = ms.ModifiedDate,
                            UploaderName = u != null ? (u.FirstName + " " + u.LastName).Trim() : string.Empty,
                            UploaderAvatar = u != null ? u.Avatar : string.Empty,
                            MusicSheetUIState = new MusicSheetUIState
                            {
                                IsLiked = userId.HasValue && ms.Likes.Any(l => l.UserId == userId.Value)
                            }
                        };

            return await query.FirstOrDefaultAsync();
        }

        public async Task<MusicSheet?> GetWithLikesAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.MusicSheets
                .Include(x => x.Likes)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task IncrementViewCountAsync(int id, int incrementBy)
        {
            await _context.Database
                .ExecuteSqlInterpolatedAsync($"UPDATE MusicSheets SET ViewCount = ViewCount + {incrementBy} WHERE Id = {id}");
        }
    }
}
