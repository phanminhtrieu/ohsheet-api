using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Results;
using CleanArchitecture.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.UseCases.Backoffice.Dashboard.Queries
{
    public record GetDashboardStatsQuery : IRequest<ApiResult<DashboardStatsDto>>;

    public class GetDashboardStatsQueryHandler(AppDbContext _context) : IRequestHandler<GetDashboardStatsQuery, ApiResult<DashboardStatsDto>>
    {
        public async Task<ApiResult<DashboardStatsDto>> Handle(GetDashboardStatsQuery request, CancellationToken cancellationToken)
        {
            var totalUsers = await _context.ApplicatioUsers.CountAsync(cancellationToken);
            var totalMusicSheets = await _context.MusicSheets.CountAsync(cancellationToken);
            
            var recentMusicSheets = await _context.MusicSheets
                .Where(x => x.CreatedDate >= DateTime.UtcNow.AddDays(-7))
                .CountAsync(cancellationToken);

             var recentUsers = await _context.ApplicatioUsers
                .Where(x => x.CreatedAt >= DateTime.UtcNow.AddDays(-7))
                .CountAsync(cancellationToken);

            return new ApiSuccessResult<DashboardStatsDto>(new DashboardStatsDto
            {
                TotalUsers = totalUsers,
                TotalMusicSheets = totalMusicSheets,
                NewUsersLast7Days = recentUsers,
                NewMusicSheetsLast7Days = recentMusicSheets
            });
        }
    }

    public class DashboardStatsDto
    {
        public int TotalUsers { get; set; }
        public int TotalMusicSheets { get; set; }
        public int NewUsersLast7Days { get; set; }
        public int NewMusicSheetsLast7Days { get; set; }
    }
}
