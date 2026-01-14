using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Results;
using CleanArchitecture.Core.Domain.Entities.MusicSheetAggregate;
using CleanArchitecture.Core.Domain.Enums;
using CleanArchitecture.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using CleanArchitecture.Core.Interfaces;
using Microsoft.AspNetCore.Http;

namespace CleanArchitecture.UseCases.Backoffice.MusicSheets.Commands
{
    public record UpdateMusicSheetCommand(
        int Id,
        string Title,
        string? Description,
        string? TranscriptionId,
        MusicSheetStatus Status,
        MusicSheetVisibility Visibility,
        IFormFile? ThumbnailFile) : IRequest<ApiResult<bool>>;

    public class UpdateMusicSheetCommandHandler(AppDbContext _context, IBlobService _blobService) : IRequestHandler<UpdateMusicSheetCommand, ApiResult<bool>>
    {
        public async Task<ApiResult<bool>> Handle(UpdateMusicSheetCommand request, CancellationToken cancellationToken)
        {
            var musicSheet = await _context.MusicSheets.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (musicSheet == null)
            {
                return new ApiErrorResult<bool>("Music Sheet not found.");
            }

            musicSheet.UpdateMetadata(request.Title, request.Description ?? string.Empty);
            musicSheet.SetTranscriptionId(request.TranscriptionId ?? string.Empty);
            musicSheet.SetStatus(request.Status);
            musicSheet.SetVisibility(request.Visibility);

            if (request.ThumbnailFile != null)
            {
                using var stream = request.ThumbnailFile.OpenReadStream();
                var thumbnailUrl = await _blobService.UploadAsync(stream, request.ThumbnailFile.FileName, request.ThumbnailFile.ContentType);
                musicSheet.SetThumbnail(thumbnailUrl);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return new ApiSuccessResult<bool>(true);
        }
    }
}
