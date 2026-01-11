using CleanArchitecture.Core.Interfaces.MusicSheetServices;

namespace CleanArchitecture.Core.Services.MusicSheetServices
{
    public class MusicSheetViewService(IViewCounterCacheService _viewCounterCacheService) : IMusicSheetViewService
    {
        public Task IncrementViewCount(int musicSheetId)
        {
            _viewCounterCacheService.IncrementViewCount(musicSheetId);

            return Task.CompletedTask;
        }
    }
}
