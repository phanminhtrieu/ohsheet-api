using CleanArchitecture.Shared.CrossCuttingConcerns.Dtos.Results;
using CleanArchitecture.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.UseCases.Backoffice.Tags.Commands
{
    public record DeleteTagCommand(int Id) : IRequest<ApiResult<bool>>;

    public class DeleteTagCommandHandler(AppDbContext _context) : IRequestHandler<DeleteTagCommand, ApiResult<bool>>
    {
        public async Task<ApiResult<bool>> Handle(DeleteTagCommand request, CancellationToken cancellationToken)
        {
            var tag = await _context.MusicSheetTags
                .Include(t => t.MusicSheets)
                .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

            if (tag == null)
            {
                return new ApiErrorResult<bool>("Tag not found.");
            }

            _context.MusicSheetTags.Remove(tag);
            await _context.SaveChangesAsync(cancellationToken);

            return new ApiSuccessResult<bool>(true);
        }
    }
}
