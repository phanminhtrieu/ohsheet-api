using CleanArchitecture.Core.Domain.Entities;
using CleanArchitecture.Core.Repositories;
using MediatR;

namespace CleanArchitecture.UseCases.MusicSheets
{
    public record RecordMusicSheetViewCommand(int MusicSheetId, Guid UserId) : IRequest;

    public class RecordMusicSheetViewCommandHandler : IRequestHandler<RecordMusicSheetViewCommand>
    {
        private readonly IRecentlyViewedRepository _recentlyViewedRepository;
        private readonly CleanArchitecture.Core.UnitOfWork.IUnitOfWork _unitOfWork;

        public RecordMusicSheetViewCommandHandler(
            IRecentlyViewedRepository recentlyViewedRepository,
            CleanArchitecture.Core.UnitOfWork.IUnitOfWork unitOfWork)
        {
            _recentlyViewedRepository = recentlyViewedRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(RecordMusicSheetViewCommand request, CancellationToken cancellationToken)
        {
            var existingRecord = await _recentlyViewedRepository.GetByUserAndSheetIdAsync(request.UserId, request.MusicSheetId, cancellationToken);

            if (existingRecord != null)
            {
                existingRecord.UpdateLastViewedAt();
                _recentlyViewedRepository.Update(existingRecord);
            }
            else
            {
                var newRecord = new RecentlyViewedMusicSheet(request.UserId, request.MusicSheetId);
                await _recentlyViewedRepository.AddAsync(newRecord);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
