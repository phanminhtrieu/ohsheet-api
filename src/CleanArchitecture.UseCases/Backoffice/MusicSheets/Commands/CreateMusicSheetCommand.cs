using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Results;
using CleanArchitecture.Core.Domain.Entities.MusicSheetAggregate;
using CleanArchitecture.Core.Domain.Enums;
using CleanArchitecture.Infrastructure.Data;
using MediatR;
using CleanArchitecture.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using CleanArchitecture.Core.Repositories;

namespace CleanArchitecture.UseCases.Backoffice.MusicSheets.Commands
{
    public record CreateMusicSheetCommand(
        Guid UserId,
        string Title,
        string? Description,
        string? TranscriptionId,
        MusicSheetStatus Status,
        MusicSheetVisibility Visibility,
        IFormFile? ThumbnailFile,
        List<string>? Tags = null) : IRequest<ApiResult<int>>;

    public class CreateMusicSheetCommandHandler(AppDbContext _context, IBlobService _blobService, IMusicSheetRepository _musicSheetRepository) : IRequestHandler<CreateMusicSheetCommand, ApiResult<int>>
    {
        public async Task<ApiResult<int>> Handle(CreateMusicSheetCommand request, CancellationToken cancellationToken)
        {
            var musicSheet = MusicSheet.Create(
                request.UserId,
                request.Title,
                request.Description,
                request.TranscriptionId
            );

            if (request.ThumbnailFile != null)
            {
                using var stream = request.ThumbnailFile.OpenReadStream();
                var thumbnailUrl = await _blobService.UploadAsync(stream, request.ThumbnailFile.FileName, request.ThumbnailFile.ContentType);
                musicSheet.SetThumbnail(thumbnailUrl);
            }

            musicSheet.SetStatus(request.Status);
            musicSheet.SetVisibility(request.Visibility);

            if (request.Tags != null && request.Tags.Any())
            {
                var existingTags = await _musicSheetRepository.GetTagsByNamesAsync(request.Tags, cancellationToken);
                var existingTagNames = existingTags.Select(t => t.Name).ToList();
                var newTagNames = request.Tags.Except(existingTagNames).ToList();

                var allTags = new List<MusicSheetTag>(existingTags);
                foreach (var tagName in newTagNames)
                {
                    allTags.Add(new MusicSheetTag(tagName));
                }

                musicSheet.AddTags(allTags);
            }

            _context.MusicSheets.Add(musicSheet);
            await _context.SaveChangesAsync(cancellationToken);

            return new ApiSuccessResult<int>(musicSheet.Id);
        }
    }
}
