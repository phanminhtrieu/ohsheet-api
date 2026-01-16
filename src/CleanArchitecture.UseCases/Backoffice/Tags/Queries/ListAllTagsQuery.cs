using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Results;
using CleanArchitecture.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.UseCases.Backoffice.Tags.Queries
{
    public record ListAllTagsQuery : IRequest<ApiResult<List<TagDto>>>;

    public class TagDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int UsageCount { get; set; }
    }

    public class ListAllTagsQueryHandler(AppDbContext _context) : IRequestHandler<ListAllTagsQuery, ApiResult<List<TagDto>>>
    {
        public async Task<ApiResult<List<TagDto>>> Handle(ListAllTagsQuery request, CancellationToken cancellationToken)
        {
            var tags = await _context.MusicSheetTags
                .AsNoTracking()
                .Select(t => new TagDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    UsageCount = t.MusicSheets.Count
                })
                .OrderByDescending(t => t.UsageCount)
                .ToListAsync(cancellationToken);

            return new ApiSuccessResult<List<TagDto>>(tags);
        }
    }
}
