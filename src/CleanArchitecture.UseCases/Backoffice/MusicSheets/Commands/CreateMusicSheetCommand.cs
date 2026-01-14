using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Results;
using CleanArchitecture.Core.Domain.Entities.MusicSheetAggregate;
using CleanArchitecture.Core.Domain.Enums;
using CleanArchitecture.Infrastructure.Data;
using MediatR;
using CleanArchitecture.Core.Interfaces;
using Microsoft.AspNetCore.Http;

namespace CleanArchitecture.UseCases.Backoffice.MusicSheets.Commands
{
    public record CreateMusicSheetCommand(
        Guid UserId,
        string Title,
        string? Description,
        string? TranscriptionId,
        MusicSheetStatus Status,
        MusicSheetVisibility Visibility,
        IFormFile? ThumbnailFile) : IRequest<ApiResult<int>>;

    public class CreateMusicSheetCommandHandler(AppDbContext _context, IBlobService _blobService) : IRequestHandler<CreateMusicSheetCommand, ApiResult<int>>
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

            _context.MusicSheets.Add(musicSheet);
            await _context.SaveChangesAsync(cancellationToken);

            return new ApiSuccessResult<int>(musicSheet.Id);
        }
    }
}
