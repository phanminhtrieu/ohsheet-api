using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Results;
using CleanArchitecture.Core.Domain.Enums;
using CleanArchitecture.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.UseCases.Backoffice.MusicSheets.Commands
{
    public record UpdateMusicSheetStatusCommand(int Id, MusicSheetStatus? Status, MusicSheetVisibility? Visibility) : IRequest<ApiResult<int>>;

    public class UpdateMusicSheetStatusCommandHandler(AppDbContext _context) : IRequestHandler<UpdateMusicSheetStatusCommand, ApiResult<int>>
    {
        public async Task<ApiResult<int>> Handle(UpdateMusicSheetStatusCommand request, CancellationToken cancellationToken)
        {
            var musicSheet = await _context.MusicSheets.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (musicSheet == null)
            {
                return new ApiErrorResult<int>("Music Sheet not found.");
            }

            if (request.Status.HasValue)
            {
                musicSheet.SetStatus(request.Status.Value);
            }

            if (request.Visibility.HasValue)
            {
                musicSheet.SetVisibility(request.Visibility.Value);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return new ApiSuccessResult<int>(musicSheet.Id);
        }
    }
}
