using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Paging;
using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Results;
using CleanArchitecture.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace CleanArchitecture.UseCases.Backoffice.Users.Queries
{
    public record ListAllUsersQuery(UserPagingRequest Request) : IRequest<ApiResult<DataTablePagedResult<UserResponseDto>>>;

    public class ListAllUsersQueryHandler(AppDbContext _context) : IRequestHandler<ListAllUsersQuery, ApiResult<DataTablePagedResult<UserResponseDto>>>
    {
        public async Task<ApiResult<DataTablePagedResult<UserResponseDto>>> Handle(ListAllUsersQuery request, CancellationToken cancellationToken)
        {
            var query = _context.ApplicatioUsers.AsNoTracking().AsQueryable();

            if (!string.IsNullOrEmpty(request.Request.TextSearch))
            {
                var search = request.Request.TextSearch.ToLower();
                query = query.Where(x => x.UserName.ToLower().Contains(search) || 
                                         x.Email.ToLower().Contains(search) ||
                                         x.FirstName.ToLower().Contains(search) ||
                                         x.LastName.ToLower().Contains(search));
            }

            var totalRecords = await query.CountAsync(cancellationToken);

            if (!string.IsNullOrEmpty(request.Request.OrderCol) && !string.IsNullOrEmpty(request.Request.OrderDir))
            {
                query = query.OrderBy(request.Request.OrderCol + " " + request.Request.OrderDir);
            }
            else
            {
                query = query.OrderByDescending(x => x.CreatedAt);
            }

            var pageIndex = request.Request.PageIndex ?? 1;
            var pageSize = request.Request.PageSize;

            var items = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new UserResponseDto
                {
                    Id = x.Id,
                    UserName = x.UserName,
                    Email = x.Email,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    DisplayName = x.DisplayName,
                    Avatar = x.Avatar,
                    AvatarUrl = x.AvatarUrl,
                    CreatedAt = x.CreatedAt,
                    LastSignInDate = x.LastSignInDate,
                    LockoutEnd = x.LockoutEnd
                })
                .ToListAsync(cancellationToken);

            return new ApiSuccessResult<DataTablePagedResult<UserResponseDto>>(new DataTablePagedResult<UserResponseDto>(items, pageIndex, pageSize, totalRecords));
        }
    }

    public class UserResponseDto
    {
        public Guid Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? DisplayName { get; set; }
        public string Avatar { get; set; } = string.Empty;
        public string? AvatarUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastSignInDate { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
    }
}
