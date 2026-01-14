using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Paging;
using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Results;
using CleanArchitecture.Core.Domain.Models.MusicSheet;
using CleanArchitecture.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace CleanArchitecture.UseCases.Backoffice.MusicSheets.Queries
{
    public record ListAllMusicSheetsQuery(MusicSheetPagingRequest Request) : IRequest<ApiResult<DataTablePagedResult<MusicSheetResponse>>>;

    public class ListAllMusicSheetsQueryHandler(AppDbContext _context) : IRequestHandler<ListAllMusicSheetsQuery, ApiResult<DataTablePagedResult<MusicSheetResponse>>>
    {
        public async Task<ApiResult<DataTablePagedResult<MusicSheetResponse>>> Handle(ListAllMusicSheetsQuery request, CancellationToken cancellationToken)
        {
            var query = _context.MusicSheets.AsNoTracking().AsQueryable();

            if (!string.IsNullOrEmpty(request.Request.TextSearch))
            {
                var search = request.Request.TextSearch.ToLower();
                query = query.Where(x => x.Title.Value.ToLower().Contains(search));
            }

            var totalRecords = await query.CountAsync(cancellationToken);

            if (!string.IsNullOrEmpty(request.Request.OrderCol) && !string.IsNullOrEmpty(request.Request.OrderDir))
            {
                query = query.OrderBy(request.Request.OrderCol + " " + request.Request.OrderDir);
            }
            else
            {
                query = query.OrderByDescending(x => x.ModifiedDate);
            }

            var pageIndex = request.Request.PageIndex ?? 1;
            var pageSize = request.Request.PageSize;

            var items = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new MusicSheetResponse
                {
                    Id = x.Id,
                    Title = x.Title.Value,
                    ParentId = x.ParentId,
                    Description = x.Description,
                    TranscriptionId = x.TranscriptionId,
                    Thumbnail = x.Thumbnail,
                    Status = x.Status,
                    MusicSheetVisibility = x.MusicSheetVisibility,
                    ViewCount = x.ViewCount,
                    LikeCount = x.LikeCount,
                    CommentCount = x.CommentCount,
                    ShareCount = x.ShareCount,
                    IsForked = x.IsForked,
                    CreatedDate = x.CreatedDate,
                    ModifiedDate = x.ModifiedDate
                })
                .ToListAsync(cancellationToken);

            return new ApiSuccessResult<DataTablePagedResult<MusicSheetResponse>>(new DataTablePagedResult<MusicSheetResponse>(items, pageIndex, pageSize, totalRecords));
        }
    }
}
