using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Results;
using CleanArchitecture.Core.Domain.Enums;
using CleanArchitecture.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.UseCases.Backoffice.MusicSheets.Commands
{
    public record DeleteMusicSheetCommand(int Id) : IRequest<ApiResult<int>>;

    public class DeleteMusicSheetCommandHandler(AppDbContext _context) : IRequestHandler<DeleteMusicSheetCommand, ApiResult<int>>
    {
        public async Task<ApiResult<int>> Handle(DeleteMusicSheetCommand request, CancellationToken cancellationToken)
        {
            var musicSheet = await _context.MusicSheets.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (musicSheet == null)
            {
                return new ApiErrorResult<int>("Music Sheet not found.");
            }

            // Soft delete
            musicSheet.SetStatus(MusicSheetStatus.Deleted);

            await _context.SaveChangesAsync(cancellationToken);

            return new ApiSuccessResult<int>(musicSheet.Id);
        }
    }
}
