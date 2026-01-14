using CleanArchitecture.Core.Domain.Models.Profile;
using CleanArchitecture.Core.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;
using CleanArchitecture.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using CleanArchitecture.Core.Domain.Models.MusicSheet;

namespace CleanArchitecture.UseCases.Profile
{
    public record GetRecentlyViewedMusicSheetsQuery(Guid UserId, int Limit = 10) : IRequest<List<RecentlyViewedMusicSheetDto>>;

    public class GetRecentlyViewedMusicSheetsQueryHandler : IRequestHandler<GetRecentlyViewedMusicSheetsQuery, List<RecentlyViewedMusicSheetDto>>
    {
        private readonly IRecentlyViewedRepository _recentlyViewedRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public GetRecentlyViewedMusicSheetsQueryHandler(
            IRecentlyViewedRepository recentlyViewedRepository,
            UserManager<ApplicationUser> userManager)
        {
            _recentlyViewedRepository = recentlyViewedRepository;
            _userManager = userManager;
        }

        public async Task<List<RecentlyViewedMusicSheetDto>> Handle(GetRecentlyViewedMusicSheetsQuery request, CancellationToken cancellationToken)
        {
            var entities = await _recentlyViewedRepository.GetByUserIdAsync(request.UserId, request.Limit, cancellationToken);

            var uploaderIds = entities.Select(x => x.MusicSheet.UserId).Distinct().ToList();
            var uploaders = await _userManager.Users
                .Where(u => uploaderIds.Contains(u.Id))
                .ToDictionaryAsync(u => u.Id, u => u, cancellationToken);

            return entities.Select(x => 
            {
                var uploader = uploaders.ContainsKey(x.MusicSheet.UserId) ? uploaders[x.MusicSheet.UserId] : null;
                var uploaderName = uploader != null 
                    ? (!string.IsNullOrEmpty(uploader.DisplayName) ? uploader.DisplayName : (uploader.FirstName + " " + uploader.LastName).Trim()) 
                    : string.Empty;
                var uploaderAvatar = uploader?.AvatarUrl ?? string.Empty;

                return new RecentlyViewedMusicSheetDto
                {
                    Id = x.MusicSheetId,
                    Title = x.MusicSheet.Title.Value,
                    UploaderName = uploaderName,
                    UploaderAvatar = uploaderAvatar,
                    Thumbnail = x.MusicSheet.Thumbnail,
                    LastViewedAt = x.LastViewedAt,
                    ViewCount = x.MusicSheet.ViewCount,
                    LikeCount = x.MusicSheet.LikeCount,
                    CommentCount = x.MusicSheet.CommentCount,
                    ShareCount = x.MusicSheet.ShareCount,
                    CreatedDate = x.MusicSheet.CreatedDate,
                    ModifiedDate = x.MusicSheet.ModifiedDate,
                    MusicSheetUIState = new MusicSheetUIState 
                    { 
                        IsLiked = x.MusicSheet.Likes.Any(l => l.UserId == request.UserId) 
                    },
                    Status = x.MusicSheet.Status,
                    MusicSheetVisibility = x.MusicSheet.MusicSheetVisibility,
                    IsForked = x.MusicSheet.IsForked,
                    Description = x.MusicSheet.Description,
                    TranscriptionId = x.MusicSheet.TranscriptionId,
                    ParentId = x.MusicSheet.ParentId
                };
            }).ToList();
        }
    }
}
